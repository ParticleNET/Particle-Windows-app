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
using Particle.Common.Interfaces;
using Particle.Common.Messages;
using System;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Particle.Common.ViewModel
{
	public class LogoutViewModel : ILogoutViewModel
	{
		private ICommand logout;
		public ICommand Logout
		{
			get
			{
				return logout ?? (logout = new RelayCommand(() =>
				{
					ViewModelLocator.Cloud.Logout();
					AppSettings.Current.RememberPassword = false;
					AppSettings.Current.DeleteStoredPassword();
					AppSettings.Current.AutoLogin = false;
					ViewModelLocator.Messenger.Send(new LoggedOutMessage());
				}));
			}
		}
	}
}