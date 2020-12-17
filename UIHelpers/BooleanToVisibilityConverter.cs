using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows;

namespace AdventOfCodeScaffolding.UI
{
	/// <summary>
	/// Converts a boolean value to true = Visibility.Visible and false = Visibility.Hidden.
	/// </summary>
	[ValueConversion(typeof(bool), typeof(Visibility))]
	public class BooleanToVisibilityConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value is bool b && targetType == typeof(Visibility))
				return b ? Visibility.Visible : Visibility.Hidden;
			else
				throw new InvalidOperationException("Target type must be Visibility and source must be boolean.");
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotSupportedException();
		}
	}
}
