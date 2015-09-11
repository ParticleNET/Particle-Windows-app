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

using GalaSoft.MvvmLight.Command;
using Particle.Common.Interfaces;
using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.UI.Popups;

namespace Particle.Common.ViewModel
{
	public class LoginViewModel : GalaSoft.MvvmLight.ViewModelBase, ILoginViewModel
	{
		private String username;
		/// <summary>
		/// The username to attempt to login with
		/// </summary>
		public String Username
		{
			get { return username; }
			set { Set(nameof(Username), ref username, value); }
		}

		private String password;
		/// <summary>
		/// The password to attempt to login with
		/// </summary>
		public String Password
		{
			get { return password; }
			set { Set(nameof(Password), ref password, value); }
		}

		private bool rememberPassword;
		/// <summary>
		/// If true stores the password in the appSettings
		/// </summary>
		public bool RememberPassword
		{
			get { return rememberPassword; }
			set { Set(nameof(RememberPassword),ref rememberPassword, value); }
		}

		private bool isAuthenticating;
		/// <summary>
		/// True if were trying to loggin.
		/// </summary>
		public bool IsAuthenticating
		{
			get { return isAuthenticating; }
			set { Set(nameof(IsAuthenticating),ref isAuthenticating, value); }
		}

		protected RelayCommand command;
		public virtual ICommand Command
		{
			get
			{
				return command ?? (command = new RelayCommand(async () =>
				{
					if(await loginAsync())
					{

					}
				}));
			}
		}

		/// <summary>
		/// Should we auto login when the app starts
		/// </summary>
		/// <value>
		///   <c>true</c> if [automatic login]; otherwise, <c>false</c>.
		/// </value>
		public bool AutoLogin
		{
			get;
			set;
		}

		/// <summary>
		/// Loads values from the store
		/// </summary>
		public void Load()
		{
			var settings = AppSettings.Current;
			Username = settings.Username;
			RememberPassword = settings.RememberPassword;
			if (RememberPassword)
			{
				Password = settings.Password;
			}
		}

		/// <summary>
		/// Attempts to Login to the cloud
		/// </summary>
		/// <returns></returns>
		private async Task<bool> loginAsync()
		{
			IsAuthenticating = true;
			var settings = AppSettings.Current;
			settings.Username = username;
			if (RememberPassword)
			{
				settings.Password = password;
			}
			settings.RememberPassword = rememberPassword;
			await Task.Delay(2000);
			IsAuthenticating = false;
			return false;
		}
	}
}