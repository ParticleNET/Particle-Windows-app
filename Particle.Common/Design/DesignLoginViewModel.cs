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
using Particle.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;

namespace Particle.Common.Design
{
	public class DesignLoginViewModel : ILoginViewModel
	{
		public bool IsAuthenticating
		{
			get
			{
				return true;
			}
			set { }
		}

		public string Password
		{
			get
			{
				return "test1";
			}
			set { }
		}

		public bool RememberPassword
		{
			get { return true; }
			set { }
		}

		public string Username
		{
			get { return "test2"; }
			set { }
		}

		public bool AutoLogin
		{
			get
			{
				return false;
			}

			set
			{
			}
		}

		public ICommand Command
		{
			get
			{
				return new RelayCommand(() => { });
			}
		}

		public bool ShouldAutoLogin
		{
			get
			{
				return false;
			}
		}

		//This is a design class so ignore the Property not being used warning
#pragma warning disable CS0067
		public event PropertyChangedEventHandler PropertyChanged;
#pragma warning restore CS0067

		public void Load(bool isLogout = false)
		{
		}
	}
}
