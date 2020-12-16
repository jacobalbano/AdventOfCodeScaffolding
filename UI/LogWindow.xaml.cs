using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace AdventOfCodeScaffolding.UI
{
	/// <summary>
	/// Interaction logic for LogWindow.xaml.  Application must ensure there is only
	/// one such non-closed window at a time.
	/// </summary>
	public partial class LogWindow : Window
	{
		/*
		 * How this works:
		 * 
		 * MainWindow ensures there's only zero or one LogWindow active (not-closed)
		 * at a time.
		 * 
		 * LogWindow has its own simple, private, singleton implementation of ILogger.
		 * 
		 * That Logger uses a concurrent FIFO queue for incoming calls on the interface,
		 * then raises an event after each call, which LogWindow consumes by using
		 * Dispatcher.Invoke() to handle those events on the UI thread.
		 * 
		 * The UI work happens in UpdateLogView() always on the UI thread in a safe manner.
		 * 
		 * When the LogWindow is closed, a flag is set so that UpdateLogView() will do
		 * nothing, even if some trailing calls are incoming.  After that flag is set,
		 * the state is saved in a static variable for carry over to the next instance
		 * of the LogWindow, if any.  State is then restored before the Logger_LogUpdate
		 * event is subscribed, the next time the LogWindow is shown.
		 * 
		 * This arrangement (specific responsibilities and sequence of events) is designed
		 * to guarantee that:
		 * 
		 * 1. No log events are lost,
		 * 2. No log events are processed out of order.
		 * 3. The data for each log event is only rooted in one place at a time.
		 * 
		 * In short: this design makes the LogWindow reliable and memory efficient.
		 * 
		 * The reliability would be more simply gained by having the Logger itself
		 * maintain state, but that would come at the expense of duplicated storage for
		 * the accumulating log text.  The current design is much more memory efficient
		 * for cases where there is a huge volume of log events.  In that case,
		 * each log event is exclusively stored either in the Queue, in the 
		 * CarryOverState when the LogWindow is closed, or in the LogWindow.TexBox
		 * when the LogWindow is visible.  No wasted memory.
		 * 
		 */

		private static readonly Logger logger = new();
		internal static ILogger SingletonLogger => logger;

		public LogWindow()
		{
			InitializeComponent();

			depth = carryOverState?.Depth ?? 0;
			logTextBox.Text = carryOverState?.AllText ?? "";

			logger.LogUpdated += Logger_LogUpdated;
			this.Closed += LogWindow_Closed;

			this.Loaded += LogWindow_Loaded;
		}

		private void LogWindow_Loaded(object sender, RoutedEventArgs e)
		{
			UpdateLogView();
			logTextBox.ScrollToEnd();
		}

		private bool closed = false;

		private record CarryOverState
		{
			public int Depth {get; init;}
			public string AllText {get; init;}
		}

		private static CarryOverState carryOverState = null;

		private void LogWindow_Closed(object sender, EventArgs e)
		{
			logger.LogUpdated -= Logger_LogUpdated;
			closed = true;
			carryOverState = new CarryOverState{Depth = depth, AllText = logTextBox.Text};
		}

		private void Logger_LogUpdated()
		{
			Dispatcher.Invoke(UpdateLogView);
		}

		private void Clear_Click(object sender, RoutedEventArgs e)
		{
			logTextBox.Clear();
		}

		private int depth = 0;

		private void UpdateLogView()
		{
			if (closed)
				return;

			StringBuilder sb = null;

			bool anyUpdate = false;

			while (logger.Queue.TryDequeue(out var logEvent))
			{
				if (!anyUpdate)
					sb = new StringBuilder();

				anyUpdate = true;

				switch (logEvent.Type)
				{
					case LogType.EnterContext:
						AppendIndentedLines(sb, logEvent.Text);
						depth++;
						break;

					case LogType.LogLine:
						AppendIndentedLines(sb, logEvent.Text);
						break;

					case LogType.LeaveContext:
						depth--;
						break;

					default:
						throw new Exception($"Unexpected logEvent.Type: {logEvent.Type} ({(int)logEvent.Type})");
				}
			}

			if (anyUpdate)
			{
				var newText = sb.ToString();
				logTextBox.AppendText(newText);

				logTextBox.ScrollToEnd();
			}
		}

		private void AppendIndentedLines(StringBuilder sb, string text)
		{
			foreach (var line in (text ?? string.Empty).Split('\n'))
			{
				sb.Append(new string('\t', depth));
				sb.Append(line.TrimEnd('\r'));
				sb.Append('\n');
			}
		}

		private enum LogType
		{
			None = 0,

			EnterContext = 1,
			LogLine = 2,
			LeaveContext = 3
		}

		private record LogEvent
		{
			public LogType Type { get; init; }
			public string Text { get; init; }
		}

		private class Logger : ILogger
		{
			internal ConcurrentQueue<LogEvent> Queue {get;}
			
			internal delegate void LogUpdatedEventHandler();
			internal event LogUpdatedEventHandler LogUpdated;

			internal Logger()
			{
				this.Queue = new ConcurrentQueue<LogEvent>();
			}

			IDisposable ILogger.Context(string section)
			{
				Queue.Enqueue(new LogEvent{Type = LogType.EnterContext, Text = section});
				LogUpdated?.Invoke();
				return new Context(this);
			}

			void ILogger.LogLine(string message)
			{
				Queue.Enqueue(new LogEvent{Type = LogType.LogLine, Text = message});
				LogUpdated?.Invoke();
			}

			private void LeaveContext()
			{
				Queue.Enqueue(new LogEvent{Type = LogType.LeaveContext, Text = null});
				LogUpdated?.Invoke();
			}

			private class Context : IDisposable
			{
				private readonly Logger logger;
				internal Context(Logger logger)
				{
					this.logger = logger;
				}

				private bool disposedValue = false;

				protected virtual void Dispose(bool disposing)
				{
					if (!disposedValue)
					{
						if (disposing)
						{
							logger.LeaveContext();
						}

						disposedValue = true;
					}
				}

				public void Dispose()
				{
					Dispose(disposing: true);
					GC.SuppressFinalize(this);
				}
			}
		}
	}
}
