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
using Newtonsoft.Json.Linq;
using ParticleApp.Business.Interfaces;
using ParticleApp.Business.ViewModel;
using System.Collections.Generic;
using System.ComponentModel;

namespace ParticleApp.Business.Design
{
	/// <summary>
	/// 
	/// </summary>
	/// <seealso cref="ParticleApp.Business.Interfaces.ITinkerViewModel" />
	public class DesignTinkerViewModel : ITinkerViewModel
	{
		/// <summary>
		/// The device
		/// </summary>
		private IDeviceWrapper device;
		public IDeviceWrapper Device
		{
			get
			{
				return device ?? (device = ViewModelLocator.CrateDeviceWrapper(new DesignParticleDevice(JObject.Parse(
@"{
name: 'Test1',
id: 'abc123',
connected: false,
product_id: 0
}"))));
			}
		}

		private List<ITinkerRowViewModel> pinRows;

		public IEnumerable<ITinkerRowViewModel> PinRows
		{
			get
			{
				return pinRows ?? (pinRows = new List<ITinkerRowViewModel>
				{
					new DesignTinkerRowViewModel
					{
						Left = new DesignPinViewModel
						{
							Device = Device,
							Mode = PinMode.AnalogWrite,
							PinDisplayName = "A7",
							Value = 122
						},
						Right = new DesignPinViewModel
						{
							Device = Device,
							PinDisplayName = "D7",
							Mode = PinMode.DigitalWrite,
							Value = 1,
							IsRight = true
						}
					},
					new DesignTinkerRowViewModel
					{
						Left = new DesignPinViewModel
						{
							Device = Device,
							Mode = PinMode.AnalogRead,
							PinDisplayName = "A6",
							Value = 1024
						},
						Right = new DesignPinViewModel
						{
							Device = device,
							Mode = PinMode.DigitalRead,
							PinDisplayName = "D6",
							Value = 1,
							IsRight = true
						}
					},
					new DesignTinkerRowViewModel
					{
						Left = new DesignPinViewModel
						{
							Device = Device,
							Mode = PinMode.Unknown,
							PinDisplayName = "A5"
						},
						Right = new DesignPinViewModel
						{
							Device = Device,
							Mode = PinMode.Unknown,
							PinDisplayName = "D5",
							IsRight = true
						}
					},
					new DesignTinkerRowViewModel
					{
						Left = new DesignPinViewModel
						{
							Device = Device,
							Mode = PinMode.Unknown,
							PinDisplayName = "A4"
						},
						Right = new DesignPinViewModel
						{
							Device = Device,
							Mode = PinMode.Unknown,
							PinDisplayName = "D4",
							IsRight = true
						}
					},
					new DesignTinkerRowViewModel
					{
						Left = new DesignPinViewModel
						{
							Device = Device,
							Mode = PinMode.Unknown,
							PinDisplayName = "A3"
						},
						Right = new DesignPinViewModel
						{
							Device = Device,
							Mode = PinMode.Unknown,
							PinDisplayName = "D3",
							IsRight = true
						}
					},
					new DesignTinkerRowViewModel
					{
						Left = new DesignPinViewModel
						{
							Device = Device,
							Mode = PinMode.Unknown,
							PinDisplayName = "A2"
						},
						Right = new DesignPinViewModel
						{
							Device = Device,
							Mode = PinMode.Unknown,
							PinDisplayName = "D2",
							IsRight = true
						}
					},
					new DesignTinkerRowViewModel
					{
						Left = new DesignPinViewModel
						{
							Device = Device,
							Mode = PinMode.Unknown,
							PinDisplayName = "A1"
						},
						Right = new DesignPinViewModel
						{
							Device = Device,
							Mode = PinMode.Unknown,
							PinDisplayName = "D1",
							IsRight = true
						}
					},
					new DesignTinkerRowViewModel
					{
						Left = new DesignPinViewModel
						{
							Device = Device,
							Mode = PinMode.Unknown,
							PinDisplayName = "A0"
						},
						Right = new DesignPinViewModel
						{
							Device = Device,
							Mode = PinMode.Unknown,
							PinDisplayName = "D0",
							IsRight = true
						}
					},
				});
			}
		}

		//This is a design class so ignore the Property not being used warning
#pragma warning disable CS0067
		public event PropertyChangedEventHandler PropertyChanged;
#pragma warning restore CS0067
	}
}
