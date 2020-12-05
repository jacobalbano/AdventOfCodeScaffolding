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
            switch ((TestResults)value)
            {
                case TestResults.None:
                    return "../Resources/StatusHelp_16x.png";
                case TestResults.NotImplemented:
                    return "../Resources/StatusWarning_16x.png";
                case TestResults.Failed:
                    return "../Resources/StatusInvalid_16x.png";
                case TestResults.Passed:
                    return "../Resources/StatusOK_16x.png";
                default:
                    throw new NotSupportedException();
            }
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
            switch ((TestResults)value)
            {
                case TestResults.None:
                    return "Pending test completion";
                case TestResults.NotImplemented:
                    return "Test is not yet implemented";
                case TestResults.Failed:
                    return "Test failed";
                case TestResults.Passed:
                    return "Test passed";
                default:
                    throw new NotSupportedException();
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
