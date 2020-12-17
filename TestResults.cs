using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows.Data;

namespace AdventOfCodeScaffolding.Util
{
    public enum TestResults
    {
        None,
        NotImplemented,
        Failed,
        Passed
    }

    class TestResultsToIconConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return ((TestResults)value) switch
            {
                TestResults.None => "../Resources/StatusHelp_16x.png",
                TestResults.NotImplemented => "../Resources/StatusWarning_16x.png",
                TestResults.Failed => "../Resources/StatusInvalid_16x.png",
                TestResults.Passed => "../Resources/StatusOK_16x.png",
                _ => throw new NotSupportedException(),
            };
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }

    class TestResultsToSummaryConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return ((TestResults)value) switch
            {
                TestResults.None => "Pending test completion",
                TestResults.NotImplemented => "Test is not yet implemented",
                TestResults.Failed => "Test failed",
                TestResults.Passed => "Test passed",
                _ => throw new NotSupportedException(),
            };
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
