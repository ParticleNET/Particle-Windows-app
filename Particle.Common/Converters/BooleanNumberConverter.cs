using Particle.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;

namespace Particle.Common.Converters
{
	public class BooleanNumberConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, string language)
		{
			var bToN = (BooleanToNumber)parameter;
			if ((bool)value)
			{
				return bToN.TrueValue;
			}
			else
			{
				return bToN.FalseValue;
			}
		}

		public object ConvertBack(object value, Type targetType, object parameter, string language)
		{
			throw new NotImplementedException();
		}
	}
}
