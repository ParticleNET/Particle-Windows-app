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
using GalaSoft.MvvmLight.Command;
using ParticleApp.Business.Interfaces;
using ParticleApp.Business.Models;
using System.ComponentModel;
using System.Windows.Input;

namespace ParticleApp.Business.Design
{
	public class DesignPinViewModel : IPinViewModel
	{
		private ICommand cmd = new RelayCommand(() =>
		{

		});
		public ICommand AnalogRead
		{
			get
			{
				return cmd;
			}
		}

		public ICommand AnalogWrite
		{
			get
			{
				return cmd;
			}
		}

		public IDeviceWrapper Device
		{
			get;

			set;
		}

		public ICommand DigitalRead
		{
			get
			{
				return cmd;
			}
		}

		public ICommand DigitalWrite
		{
			get
			{
				return cmd;
			}
		}

		public ICommand AnalogManipulationComplete
		{
			get
			{
				return cmd;
			}
		}

		public ICommand Holding
		{
			get
			{
				return cmd;
			}
		}

		public bool IsRight
		{
			get;
			set;
		}

		public PinMode Mode
		{
			get;
			set;
		}

		public string PinDisplayName
		{
			get;

			set;
		}

		public string PinId
		{
			get;

			set;
		}

		public ICommand Refresh
		{
			get
			{
				return cmd;
			}
		}

		public bool ShowSelect
		{
			get;
			set;
		}

		public PinMode SupportedModes
		{
			get;

			set;
		}

		public short Value
		{
			get;

			set;
		}

		public IPinValue PinValue
		{
			get
			{
				return new PinValueModel
				{
					Value = Value,
					Mode = Mode
				};
			}
		}

		//This is a design class so ignore the Property not being used warning
#pragma warning disable CS0067
		public event PropertyChangedEventHandler PropertyChanged;
#pragma warning restore CS0067
	}
}
