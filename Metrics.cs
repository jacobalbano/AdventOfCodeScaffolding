using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace AdventOfCodeScaffolding.Util
{
    public class Metrics
    {
        public static readonly Metrics Empty = new Metrics(new List<long>(0));

        private const long tpus = TimeSpan.TicksPerMillisecond / 1000;

        public static Metrics Measure(long maxMilliseconds, int minReps, Action action, bool runOnceOnly = false)
        {
            var times = new List<long>(minReps);
            var totalTime = Stopwatch.StartNew();
            for (int i = 0; i < minReps || totalTime.ElapsedMilliseconds < maxMilliseconds; i++)
            {
                var timer = Stopwatch.StartNew();
                action();
                times.Add(timer.Elapsed.Ticks / tpus);
                if (runOnceOnly)
                    break;
            }

            return new Metrics(times);
        }

        private Metrics(List<long> times)
        {
            Times = times;
        }

        public override string ToString()
        {
            Times.Sort();
            var mean = Times.Any() ? (int)Math.Floor(Times.Sum() / (float)Times.Count) : 0;
            var median = Times.Any() ? Times[Times.Count / 2] : 0;
            string meanUnit = "μs", medianUnit = "μs";
            if (mean > 1000) { mean /= 1000; meanUnit = "ms"; }
            if (median > 1000) { median /= 1000; medianUnit = "ms"; }

            return $"Reps({Times.Count}) ・ Mean({mean}{meanUnit}) ・ Median({median}{medianUnit})";
        }

        private List<long> Times { get; }
    }
}
