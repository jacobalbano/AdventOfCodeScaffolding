using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace AdventOfCodeScaffolding.UI
{
	/// <summary>
	/// Converts a boolean value to its inverse.
	/// </summary>
	/// <remarks>
	/// Based on <a href="https://stackoverflow.com/questions/1039636/how-to-bind-inverse-boolean-properties-in-wpf">
	/// "How to bind inverse boolean properties in WPF?" on Stack Overflow</a>
	/// </remarks>
	[ValueConversion(typeof(bool), typeof(bool))]
	public class InverseBooleanConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (targetType != typeof(bool))
				throw new InvalidOperationException("Target must be boolean.");

			return !(bool)value;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotSupportedException();
		}
	}
}
