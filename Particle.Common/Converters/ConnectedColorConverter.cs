using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;

namespace Particle.Common.Converters
{
	public class ConnectedColorConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, string language)
		{
			if ((bool)value)
			{
				return new SolidColorBrush(Windows.UI.Colors.ForestGreen);
			}
			return new SolidColorBrush(Windows.UI.Colors.Red);
		}

		public object ConvertBack(object value, Type targetType, object parameter, string language)
		{
			throw new NotImplementedException();
		}
	}
}
