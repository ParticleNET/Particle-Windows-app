using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace Particle.Common.Converters
{
	public class PinModeConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, string language)
		{
			PinMode pmode = (PinMode)value;
			
			PinMode smode = (PinMode)Enum.Parse(typeof(PinMode),parameter as String);
			if(smode == (pmode & smode))
			{
				return Visibility.Visible;
			}

			return Visibility.Collapsed;
		}

		public object ConvertBack(object value, Type targetType, object parameter, string language)
		{
			throw new NotImplementedException();
		}
	}
}
