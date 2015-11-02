﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;

namespace Particle.Common.Converters
{
	public class NotNullConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, string language)
		{
			if(value == null)
			{
				return false;
			}
			return true;
		}

		public object ConvertBack(object value, Type targetType, object parameter, string language)
		{
			throw new NotImplementedException();
		}
	}
}
