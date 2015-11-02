/*
   Copyright 2015 Sannel Software, L.L.C.

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
using Particle.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Particle.Common.Models;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;
using Particle.Common.Messages;
using Windows.UI.Xaml.Input;

namespace Particle.Common.ViewModel
{
	public class PinViewModel : ViewModelBase, IPinViewModel
	{
		public PinViewModel()
		{
			ViewModelLocator.Messenger.Register<ModeDialogMessage>(this, (m) =>
			{
				if(this != m.Source && m.IsOpen && ShowSelect)
				{
					ShowSelect = false;
				}
			});
		}

		~PinViewModel()
		{
			ViewModelLocator.Messenger.Unregister(this);
		}

		private ICommand analogRead;
		public ICommand AnalogRead
		{
			get
			{
				return analogRead ?? (analogRead = new RelayCommand(() =>
				{
					if(Mode == PinMode.AnalogRead && ShowSelect)
					{
						ShowSelect = false;
					}
					Value = 0;
					Mode = PinMode.AnalogRead;
					if (Refresh.CanExecute(null))
					{
						Refresh.Execute(null);
					}
				}));
			}
		}

		private ICommand analogWrite;
		public ICommand AnalogWrite
		{
			get
			{
				return analogWrite ?? (analogWrite = new RelayCommand(() =>
				{
					if (Mode == PinMode.AnalogWrite && ShowSelect)
					{
						ShowSelect = false;
					}
					Value = 0;
					Mode = PinMode.AnalogWrite;
				}));
			}
		}

		private ParticleDeviceWrapper device;
		public ParticleDeviceWrapper Device
		{
			get
			{
				return device;
			}

			set
			{
				Set(nameof(Device), ref device, value);
			}
		}

		private ICommand digitalRead;
		public ICommand DigitalRead
		{
			get
			{
				return digitalRead ?? (digitalRead = new RelayCommand(() =>
				{
					if (Mode == PinMode.DigitalRead && ShowSelect)
					{
						ShowSelect = false;
					}
					Value = 0;
					Mode = PinMode.DigitalRead;
					if (Refresh.CanExecute(null))
					{
						Refresh.Execute(null);
					}
				}));
			}
		}

		private ICommand digitalWrite;
		public ICommand DigitalWrite
		{
			get
			{
				return digitalWrite ?? (digitalWrite = new RelayCommand(() =>
				{
					if (Mode == PinMode.DigitalWrite && ShowSelect)
					{
						ShowSelect = false;
					}
					Value = 0;
					Mode = PinMode.DigitalWrite;
				}));
			}
		}

		private PinMode mode;
		public PinMode Mode
		{
			get
			{
				return mode;
			}
			set
			{
				if(Set(nameof(Mode), ref mode, value))
				{
					if (mode != PinMode.Unknown && ShowSelect)
					{
						ShowSelect = false;
					}
				}
			}
		}

		private String pinDisplayName;
		public string PinDisplayName
		{
			get
			{
				return pinDisplayName;
			}
			set
			{
				Set(nameof(PinDisplayName), ref pinDisplayName, value);
			}
		}

		private String pinId;
		public string PinId
		{
			get
			{
				return pinId;
			}
			set
			{
				Set(nameof(PinId), ref pinId, value);
			}
		}

		private ICommand refresh;
		public ICommand Refresh
		{
			get
			{
				return refresh ?? (refresh = new RelayCommand(async () =>
				{
					if (Mode == PinMode.Unknown)
					{
						if (!ShowSelect)
						{
							ShowSelect = true;
						}
						else
						{
							ShowSelect = false;
						}
					}
					else if(Mode == PinMode.AnalogRead)
					{
						var result = await Device.Device.CallFunctionAsync("analogread", PinId);
						if (result.Success)
						{
							Value = (short)result.Data;
						}
					}
					else if(Mode == PinMode.DigitalRead)
					{
						var result = await Device.Device.CallFunctionAsync("digitalread", PinId);
						if (result.Success)
						{
							Value = (short)result.Data;
						}
					}
					else if(Mode == PinMode.DigitalWrite)
					{
						Value = (short)((Value > 0) ? 0 : 1);
						String sendValue = (Value > 0) ? $"{PinId} HIGH" : $"{PinId} LOW";
						var result = await Device.Device.CallFunctionAsync("digitalwrite", sendValue);
						if (result.Success)
						{

						}
					}
				}));
			}
		}

		private PinMode supportedModes;
		public PinMode SupportedModes
		{
			get
			{
				return supportedModes;
			}

			set
			{
				Set(nameof(SupportedModes), ref supportedModes, value);
			}
		}

		private short _value;
		public short Value
		{
			get
			{
				return _value;
			}

			set
			{
				Set(nameof(Value), ref _value, value);
			}
		}

		private bool isRight;
		public bool IsRight
		{
			get
			{
				return isRight;
			}
			set
			{
				Set(nameof(IsRight), ref isRight, value);
			}
		}

		private bool showSelect = false;
		public bool ShowSelect
		{
			get
			{
				return showSelect;
			}

			set
			{
				if(Set(nameof(ShowSelect), ref showSelect, value))
				{
					ViewModelLocator.Messenger.Send(new ModeDialogMessage
					{
						Source = this,
						IsOpen = showSelect
					});
				}
			}
		}

		private ICommand holding;
		public ICommand Holding
		{
			get
			{
				return holding ?? (holding = new RelayCommand(() =>
				{
					if (!ShowSelect)
					{
						ShowSelect = true;
					}
				}));
			}
		}
	}
}
