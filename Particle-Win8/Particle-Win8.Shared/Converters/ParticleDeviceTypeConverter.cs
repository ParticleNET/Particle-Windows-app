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
using Particle;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;

namespace ParticleApp.Business.Converters
{
	public class ParticleDeviceTypeConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, string language)
		{
			ParticleDeviceType t = (ParticleDeviceType)value;
			switch(t)
			{
				case ParticleDeviceType.Core:
					return MM.M.GetString("Core");

				case ParticleDeviceType.Photon:
					return MM.M.GetString("Photon");

				case ParticleDeviceType.Electron:
					return MM.M.GetString("Electron");
			}

			return "Unknown";
		}

		public object ConvertBack(object value, Type targetType, object parameter, string language)
		{
			throw new NotImplementedException();
		}
	}
}
