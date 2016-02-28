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
using Particle.Common.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Particle.Common.Interfaces
{
	public interface ITinkerViewModel : INotifyPropertyChanged
	{
		/// <summary>
		/// Gets the device.
		/// </summary>
		/// <value>
		/// The device.
		/// </value>
		ParticleDeviceWrapper Device { get; }
		/// <summary>
		/// Gets the pin rows.
		/// </summary>
		/// <value>
		/// The pin rows.
		/// </value>
		IEnumerable<ITinkerRowViewModel> PinRows { get; }
	}
}
