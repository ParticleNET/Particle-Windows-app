﻿/*
   Copyright 2016 ParticleNET

   Licensed under the Apache License, Version 2.0 (the "License");
   you may not use this file except in compliance with the License.
   You may obtain a copy of the License at

	   http://www.apache.org/licenses/LICENSE-2.0

   Unless required by applicable law or agreed to in writing, software
   distributed under the License is distributed on an "AS IS" BASIS,
   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
   See the License for the specific language governing permissions and
   limitations under the License.
*/
using Particle.Common.Models;
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
	public class StatusColorConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, string language)
		{
			var status = (DeviceStatus)value;
			switch (status)
			{
				case DeviceStatus.Connected:
					return new SolidColorBrush(Colors.Yellow);

				case DeviceStatus.Tinker:
					return new SolidColorBrush(Colors.Green);

				case DeviceStatus.Failed:
				case DeviceStatus.Flashing:
					return new SolidColorBrush(Colors.Magenta);

				case DeviceStatus.Offline:
					return new SolidColorBrush(Colors.Red);
			}

			return new SolidColorBrush(Colors.Red);
		}

		public object ConvertBack(object value, Type targetType, object parameter, string language)
		{
			throw new NotImplementedException();
		}
	}
}
