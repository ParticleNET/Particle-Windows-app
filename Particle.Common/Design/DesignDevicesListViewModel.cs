﻿/*
   Copyright 2015 ParticleNET

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
using Particle.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.ComponentModel;
using Newtonsoft.Json.Linq;

namespace Particle.Common.Design
{
	public class DesignDevicesListViewModel : IDevicesListViewModel
	{
		public ObservableCollection<ParticleDevice> Devices
		{
			get;
		} = new ObservableCollection<ParticleDevice>()
		{
			new DesignParticleDevice(JObject.Parse(
@"{
name: 'Test1',
id: 'abc123',
connected: false,
product_id: 0
}")),
			new DesignParticleDevice(JObject.Parse(
@"{
name: 'Test2',
id: 'def456',
connected: true,
product_id: 6
}")),
			new DesignParticleDevice(JObject.Parse(
@"{
name: 'Test3',
id: 'ghi789',
connected: true,
product_id: 0
}"))
		};

		public bool IsRefreshing
		{
			get
			{
				return false;
			}
		}

		public ParticleDevice SelectedDevice
		{
			get;
			set;
		}

		public event PropertyChangedEventHandler PropertyChanged;
	}
}
