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

		protected ChallengeBase()
		{
            this.CancellationTokenSource = new CancellationTokenSource();
            this.CancellationToken = this.CancellationTokenSource.Token;
        }

        /// <summary>
        /// Will throw an OperationCanceledException if the user hits the Cancel button since running this challenge.
        /// Please call this during inner loops at convenient places if the calculation may be very long.
        /// </summary>
        protected void AllowCancel()
        {
            this.CancellationToken.ThrowIfCancellationRequested();
        }

		internal CancellationTokenSource CancellationTokenSource {get;}
        private CancellationToken CancellationToken {get;}
	}
}
