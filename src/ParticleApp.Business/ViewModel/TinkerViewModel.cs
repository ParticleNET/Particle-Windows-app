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
using GalaSoft.MvvmLight;
using Particle;
using ParticleApp.Business.Interfaces;
using System.Collections.Generic;

namespace ParticleApp.Business.ViewModel
{
	public class TinkerViewModel : ViewModelBase, ITinkerViewModel
	{
		public TinkerViewModel()
		{
			ViewModelLocator.Messenger.Register<Messages.SelectedDeviceMessage>(this, async (d) =>
			{
				Device = d.Device;
				if (Device != null)
				{
					await Device.Device.RefreshAsync();
				}
			});
		}

		private IDeviceWrapper device;
		public IDeviceWrapper Device
		{
			get
			{
				return device;
			}
			set
			{
				if(Set(nameof(Device), ref device, value))
				{
					if (Device != null)
					{
						switch (device.Device.DeviceType)
						{
							case ParticleDeviceType.Photon:
								setupPhotonPins();
								break;

							case ParticleDeviceType.Electron:
								setupPhotonPins();
								break;

							case ParticleDeviceType.Core:
							default:
								setupCorePins();
								break;
						}
					}
					else
					{
						pinRows = null;
						RaisePropertyChanged(nameof(PinRows));
					}
				}
			}
		}

		private void setupCorePins()
		{
			pinRows = new List<ITinkerRowViewModel>();
			pinRows.Add(new TinkerRowViewModel
			{
				Left = new PinViewModel
				{
					Device = Device,
					PinId = "A7",
					PinDisplayName = "A7",
					SupportedModes = PinMode.AnalogRead | PinMode.AnalogWrite | PinMode.DigitalRead | PinMode.DigitalWrite
				},
				Right = new PinViewModel
				{
					Device = Device,
					PinId = "D7",
					PinDisplayName = "D7",
					SupportedModes = PinMode.DigitalRead | PinMode.DigitalWrite,
					IsRight = true
				}
			});
			pinRows.Add(new TinkerRowViewModel
			{
				Left = new PinViewModel
				{
					Device = Device,
					PinId = "A6",
					PinDisplayName = "A6",
					SupportedModes = PinMode.AnalogRead | PinMode.AnalogWrite | PinMode.DigitalRead | PinMode.DigitalWrite
				},
				Right = new PinViewModel
				{
					Device = Device,
					PinId = "D6",
					PinDisplayName = "D6",
					SupportedModes = PinMode.DigitalRead | PinMode.DigitalWrite,
					IsRight = true
				}
			});
			pinRows.Add(new TinkerRowViewModel
			{
				Left = new PinViewModel
				{
					Device = Device,
					PinId = "A5",
					PinDisplayName = "A5",
					SupportedModes = PinMode.AnalogRead | PinMode.AnalogWrite | PinMode.DigitalRead | PinMode.DigitalWrite
				},
				Right = new PinViewModel
				{
					Device = Device,
					PinId = "D5",
					PinDisplayName = "D5",
					SupportedModes = PinMode.DigitalRead | PinMode.DigitalWrite,
					IsRight = true
				}
			});
			pinRows.Add(new TinkerRowViewModel
			{
				Left = new PinViewModel
				{
					Device = Device,
					PinId = "A4",
					PinDisplayName = "A4",
					SupportedModes = PinMode.AnalogRead | PinMode.DigitalRead | PinMode.DigitalWrite
				},
				Right = new PinViewModel
				{
					Device = Device,
					PinId = "D4",
					PinDisplayName = "D4",
					SupportedModes = PinMode.DigitalRead | PinMode.DigitalWrite,
					IsRight = true
				}
			});
			pinRows.Add(new TinkerRowViewModel
			{
				Left = new PinViewModel
				{
					Device = Device,
					PinId = "A3",
					PinDisplayName = "A3",
					SupportedModes = PinMode.AnalogRead | PinMode.DigitalRead | PinMode.DigitalWrite
				},
				Right = new PinViewModel
				{
					Device = Device,
					PinId = "D3",
					PinDisplayName = "D3",
					SupportedModes = PinMode.DigitalRead | PinMode.DigitalWrite,
					IsRight = true
				}
			});
			pinRows.Add(new TinkerRowViewModel
			{
				Left = new PinViewModel
				{
					Device = Device,
					PinId = "A2",
					PinDisplayName = "A2",
					SupportedModes = PinMode.AnalogRead | PinMode.AnalogWrite | PinMode.DigitalRead | PinMode.DigitalWrite
				},
				Right = new PinViewModel
				{
					Device = Device,
					PinId = "D2",
					PinDisplayName = "D2",
					SupportedModes = PinMode.DigitalRead | PinMode.DigitalWrite,
					IsRight = true
				}
			});
			pinRows.Add(new TinkerRowViewModel
			{
				Left = new PinViewModel
				{
					Device = Device,
					PinId = "A1",
					PinDisplayName = "A1",
					SupportedModes = PinMode.AnalogRead | PinMode.AnalogWrite | PinMode.DigitalRead | PinMode.DigitalWrite
				},
				Right = new PinViewModel
				{
					Device = Device,
					PinId = "D1",
					PinDisplayName = "D1",
					SupportedModes = PinMode.AnalogWrite | PinMode.DigitalRead | PinMode.DigitalWrite,
					IsRight = true
				}
			});
			pinRows.Add(new TinkerRowViewModel
			{
				Left = new PinViewModel
				{
					Device = Device,
					PinId = "A0",
					PinDisplayName = "A0",
					SupportedModes = PinMode.AnalogRead | PinMode.AnalogWrite | PinMode.DigitalRead | PinMode.DigitalWrite
				},
				Right = new PinViewModel
				{
					Device = Device,
					PinId = "D0",
					PinDisplayName = "D0",
					SupportedModes = PinMode.AnalogWrite | PinMode.DigitalRead | PinMode.DigitalWrite,
					IsRight = true
				}
			});

			RaisePropertyChanged(nameof(PinRows));
		}

		private void setupPhotonPins()
		{
			pinRows = new List<ITinkerRowViewModel>();
			pinRows.Add(new TinkerRowViewModel
			{
				Left = new PinViewModel
				{
					Device = Device,
					PinId = "A7",
					PinDisplayName = "WKP",
					SupportedModes = PinMode.AnalogRead | PinMode.AnalogWrite | PinMode.DigitalRead | PinMode.DigitalWrite
				},
				Right = new PinViewModel
				{
					Device = Device,
					PinId = "D7",
					PinDisplayName = "D7",
					SupportedModes = PinMode.DigitalRead | PinMode.DigitalWrite,
					IsRight = true
				}
			});
			pinRows.Add(new TinkerRowViewModel
			{
				Left = new PinViewModel
				{
					Device = Device,
					PinId = "A6",
					PinDisplayName = "DAK",
					SupportedModes = PinMode.AnalogRead | PinMode.AnalogWrite | PinMode.DigitalRead | PinMode.DigitalWrite
				},
				Right = new PinViewModel
				{
					Device = Device,
					PinId = "D6",
					PinDisplayName = "D6",
					SupportedModes = PinMode.DigitalRead | PinMode.DigitalWrite,
					IsRight = true
				}
			});
			pinRows.Add(new TinkerRowViewModel
			{
				Left = new PinViewModel
				{
					Device = Device,
					PinId = "A5",
					PinDisplayName = "A5",
					SupportedModes = PinMode.AnalogRead | PinMode.AnalogWrite | PinMode.DigitalRead | PinMode.DigitalWrite
				},
				Right = new PinViewModel
				{
					Device = Device,
					PinId = "D5",
					PinDisplayName = "D5",
					SupportedModes = PinMode.DigitalRead | PinMode.DigitalWrite,
					IsRight = true
				}
			});
			pinRows.Add(new TinkerRowViewModel
			{
				Left = new PinViewModel
				{
					Device = Device,
					PinId = "A4",
					PinDisplayName = "A4",
					SupportedModes = PinMode.AnalogRead | PinMode.AnalogWrite | PinMode.DigitalRead | PinMode.DigitalWrite
				},
				Right = new PinViewModel
				{
					Device = Device,
					PinId = "D4",
					PinDisplayName = "D4",
					SupportedModes = PinMode.DigitalRead | PinMode.DigitalWrite,
					IsRight = true
				}
			});
			pinRows.Add(new TinkerRowViewModel
			{
				Left = new PinViewModel
				{
					Device = Device,
					PinId = "A3",
					PinDisplayName = "A3",
					SupportedModes = PinMode.AnalogRead | PinMode.AnalogWrite | PinMode.DigitalRead | PinMode.DigitalWrite
				},
				Right = new PinViewModel
				{
					Device = Device,
					PinId = "D3",
					PinDisplayName = "D3",
					SupportedModes = PinMode.AnalogWrite | PinMode.DigitalRead | PinMode.DigitalWrite,
					IsRight = true
				}
			});
			pinRows.Add(new TinkerRowViewModel
			{
				Left = new PinViewModel
				{
					Device = Device,
					PinId = "A2",
					PinDisplayName = "A2",
					SupportedModes = PinMode.AnalogRead | PinMode.DigitalRead | PinMode.DigitalWrite
				},
				Right = new PinViewModel
				{
					Device = Device,
					PinId = "D2",
					PinDisplayName = "D2",
					SupportedModes = PinMode.AnalogWrite | PinMode.DigitalRead | PinMode.DigitalWrite,
					IsRight = true
				}
			});
			pinRows.Add(new TinkerRowViewModel
			{
				Left = new PinViewModel
				{
					Device = Device,
					PinId = "A1",
					PinDisplayName = "A1",
					SupportedModes = PinMode.AnalogRead | PinMode.DigitalRead | PinMode.DigitalWrite
				},
				Right = new PinViewModel
				{
					Device = Device,
					PinId = "D1",
					PinDisplayName = "D1",
					SupportedModes = PinMode.AnalogWrite | PinMode.DigitalRead | PinMode.DigitalWrite,
					IsRight = true
				}
			});
			pinRows.Add(new TinkerRowViewModel
			{
				Left = new PinViewModel
				{
					Device = Device,
					PinId = "A0",
					PinDisplayName = "A0",
					SupportedModes = PinMode.AnalogRead | PinMode.DigitalRead | PinMode.DigitalWrite
				},
				Right = new PinViewModel
				{
					Device = Device,
					PinId = "D0",
					PinDisplayName = "D0",
					SupportedModes = PinMode.AnalogWrite | PinMode.DigitalRead | PinMode.DigitalWrite,
					IsRight = true
				}
			});
			RaisePropertyChanged(nameof(PinRows));
		}
		
		private IList<ITinkerRowViewModel> pinRows;

		public IEnumerable<ITinkerRowViewModel> PinRows
		{
			get
			{
				return pinRows;
			}
		}
	}
}
