using Particle.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;

namespace Particle.Common.Converters
{
	public class PinBorderColorConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, string language)
		{
			var mode = (PinMode)value;
			switch(mode)
			{
				case PinMode.AnalogRead:
					return new SolidColorBrush(Colors.LightGreen);

				case PinMode.AnalogWrite:
					return new SolidColorBrush(Colors.Yellow);

				case PinMode.DigitalRead:
					return new SolidColorBrush(Colors.LightCyan);

				case PinMode.DigitalWrite:
					return new SolidColorBrush(Colors.Red);

				default:
					return new SolidColorBrush(Colors.Transparent);
			}
		}

		public object ConvertBack(object value, Type targetType, object parameter, string language)
		{
			throw new NotImplementedException();
		}
	}
}
