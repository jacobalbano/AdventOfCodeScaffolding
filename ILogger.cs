using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCodeScaffolding
{
	/// <summary>
	/// Standard interface for writing to a text-based log, with support for automatic indentation within Context blocks.
	/// </summary>
	/// <remarks>
	/// NOT THREADSAFE.  If your challenge has only one thread, you don't need to do anything else to be safe.
	/// If your challenge has more than one thread, you must either log only from the main thread (which called Part1() or Part2())
	/// or manually synchronize so only one thread calls at a time.  In no case should you allow any thread you create to make use of this
	/// interface after the corresponding Part1() or Part2() call returns.
	/// </remarks>
	public interface ILogger
	{
	#region basic members

		/// <summary>
		/// Logs a message as a line (appending newline).
		/// </summary>
		/// <param name="message">The message to be logged.  May be null, which is treated as an empty string.
		/// May be multiple lines (containing LF or CR/LF any number of times), in which case each line is separately indented
		/// as necessary for consistent indentation of the entire message.</param>
		void LogLine(string message);

		/// <summary>
		/// Logs a section header as a line (appending newline) and indents all subsequent lines, until the returned value is disposed.
		/// Intended to be used exclusively within using() statements.
		/// </summary>
		/// <param name="section">The section message to be logged to give "context" to the subsequently indented lines.
		/// The section message is essentially written via a direct call to LogLine(section) before increasing the indent count.
		/// See LogLine() for details.
		/// </param>
		IDisposable Context(string section);

	#endregion // basic members


	#region convenience members

		/// <summary>
		/// Logs a section header as a line and indents all subsequent lines, until the returned value is disposed.
		/// Intended to be used exclusively within using() statements.  Null is treated as an empty header.
		/// </summary>
		IDisposable Context<T>(T section) => Context(section?.ToString() ?? string.Empty);

		/// <summary>
		/// Logs an object as a message line (via ToString() and appending newline).  Null is logged as an empty line.
		/// </summary>
		void LogLine<T>(T message) => LogLine(message?.ToString() ?? string.Empty);

		/// <summary>
		/// Logs an empty line.
		/// </summary>
		void LogLine() => LogLine(string.Empty);

		/// <summary>
		/// Logs an exception in a standard way, by first using Context("EXCEPTION") then logging the details of the exception
		/// via ex.ToString(), unless ex is null, in which case the line logged is "(ex == null)".  In either case, the context
		/// is automatically disposed.
		/// </summary>
		void LogException(Exception ex)
		{
			using (Context("EXCEPTION:"))
				LogLine(ex?.ToString() ?? "(ex == null)");
		}

	#endregion // convenience members
	}
}
