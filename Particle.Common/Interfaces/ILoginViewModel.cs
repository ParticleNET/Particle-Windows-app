/*
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
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Particle.Common.Interfaces
{
	public interface ILoginViewModel : INotifyPropertyChanged
	{
		/// <summary>
		/// The username for logging into the Particle Cloud Api
		/// </summary>
		String Username { get; set; }
		/// <summary>
		/// The password for logging into the Particle Cloud Api
		/// </summary>
		String Password { get; set; }
		/// <summary>
		/// Should we store the password in the local settings
		/// </summary>
		bool RememberPassword { get; set; }

		/// <summary>
		/// Should we auto login when the app starts
		/// </summary>
		/// <value>
		///   <c>true</c> if [automatic login]; otherwise, <c>false</c>.
		/// </value>
		bool AutoLogin { get; set; }
		/// <summary>
		/// Are we currently trying to loggin?
		/// </summary>
		bool IsAuthenticating { get; set; }
		/// <summary>
		/// Loads the stored data into the view
		/// </summary>
		void Load();
		/// <summary>
		/// Represents the ICommand for the login action
		/// </summary>
		ICommand Command { get; }
	}
}
