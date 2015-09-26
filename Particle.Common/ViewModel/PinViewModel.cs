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

namespace Particle.Common.ViewModel
{
	public class PinViewModel : ViewModelBase, IPinViewModel
	{
		public ICommand AnalogRead
		{
			get
			{
				throw new NotImplementedException();
			}
		}

		public ICommand AnalogWrite
		{
			get
			{
				throw new NotImplementedException();
			}
		}

		public ParticleDeviceWrapper Device
		{
			get
			{
				throw new NotImplementedException();
			}

			set
			{
				throw new NotImplementedException();
			}
		}

		public ICommand DigitalRead
		{
			get
			{
				throw new NotImplementedException();
			}
		}

		public ICommand DigitalWrite
		{
			get
			{
				throw new NotImplementedException();
			}
		}

		public PinMode Mode
		{
			get
			{
				throw new NotImplementedException();
			}
		}

		public string PinDisplayName
		{
			get
			{
				throw new NotImplementedException();
			}

			set
			{
				throw new NotImplementedException();
			}
		}

		public string PinId
		{
			get
			{
				throw new NotImplementedException();
			}

			set
			{
				throw new NotImplementedException();
			}
		}

		public ICommand Refresh
		{
			get
			{
				throw new NotImplementedException();
			}
		}

		public PinMode SupportedModes
		{
			get
			{
				throw new NotImplementedException();
			}

			set
			{
				throw new NotImplementedException();
			}
		}

		public short Value
		{
			get
			{
				throw new NotImplementedException();
			}

			set
			{
				throw new NotImplementedException();
			}
		}
	}
}
