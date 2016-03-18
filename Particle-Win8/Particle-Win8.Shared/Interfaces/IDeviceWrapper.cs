/*
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
using ParticleApp.Business.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows.Input;

namespace ParticleApp.Business.Interfaces
{
	public interface IDeviceWrapper : INotifyPropertyChanged
	{
		bool IsRefreshing { get; }

		DeviceStatus Status { get; }

		ParticleDevice Device { get; }

		bool HasTinker { get; }

		ICommand CopyDeviceIdCommand { get; }

		ICommand FlashTinkerCommand { get; }

		ICommand UnclaimDeviceCommand { get; }

	}
}
