using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;

namespace AdventOfCodeScaffolding
{
    [AttributeUsage(AttributeTargets.Class)]
    public class ChallengeAttribute : Attribute
    {
        public int DayNum { get; }
        public string Title { get; }

        public ChallengeAttribute(int dayNumber, string title)
        {
            DayNum = dayNumber;
            Title = title;
        }
    }

    public abstract class ChallengeBase
    {
        public virtual object Part1(string input) => throw new NotImplementedException();
        public virtual void Part1Test() => throw new NotImplementedException();

        public virtual object Part2(string input) => throw new NotImplementedException();
        public virtual void Part2Test() => throw new NotImplementedException();

        [ThreadStatic]
        private static ILogger threadLogger = null;

		protected ChallengeBase()
		{
            this.CancellationTokenSource = new CancellationTokenSource();
            this.CancellationToken = this.CancellationTokenSource.Token;
            threadLogger = this.Logger = notReadyLogger;
        }

        /// <summary>
        /// Will throw an OperationCanceledException if the user hits the Cancel button since running this challenge.
        /// Please call this during inner loops at convenient places if the calculation may be very long.
        /// </summary>
        protected void AllowCancel()
        {
            this.CancellationToken.ThrowIfCancellationRequested();
        }

        /// <summary>
        /// Enables convenient log output to the scaffolding Log Window, including automatic indentation within contextual blocks.
        /// </summary>
        /// <remarks>
        /// Log output is appended to the content of the log even when the log window is not visible.
        /// Do not log excessively.  The log accumulates in memory until manually cleared or the program is exitted.
        /// 
        /// Use only within part runs - do not use within challenge constructor (if any) or InvalidOperationException will be thrown
        /// from any method call.
        /// </remarks>
        protected ILogger Logger {get; private set;}

		internal CancellationTokenSource CancellationTokenSource {get;}
        private CancellationToken CancellationToken {get;}

        internal ILogger InternalLogger
        {
            get {return Logger;}
            set {threadLogger = Logger = value;}
        }

        /// <summary>
        /// Enables even more convenient log output to the scaffolding Log Window, including automatic indentation within contextual blocks,
        /// without having to have an instance of a ChallengeBase-derived object.
        /// </summary>
        /// <remarks>
        /// This property is only intended for use by types that are strictly assisting a ChallengeBase-derived object
        /// during its execution of the Part1() or Part2() overrides.  Because it is static, it can be used for that purpose
        /// without having to pass around the ILogger return value.
        /// 
        /// When accessed from the same thread that is running a challenge, this property returns that challenge's equivalent of base.Logger.
        /// It should not be accessed by any other thread.  See ChallengeBase.Logger for more info.
        /// </remarks>
        public static ILogger ThreadLogger => threadLogger;

        internal readonly static NotReadyLogger notReadyLogger = new();
		internal class NotReadyLogger : ILogger
		{
			public IDisposable Context(string section)
			{
                throw new InvalidOperationException("Cannot use ChallengeBase.Logger outside of part runs.");
			}

			public void LogLine(string message)
			{
                throw new InvalidOperationException("Cannot use ChallengeBase.Logger outside of part runs.");
			}
		}
	}
}
