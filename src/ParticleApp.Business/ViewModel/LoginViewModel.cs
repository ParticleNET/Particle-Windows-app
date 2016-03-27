﻿/*
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
using ParticleApp.Business.Messages;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ParticleApp.Business.ViewModel
{
	public class LoginViewModel : GalaSoft.MvvmLight.ViewModelBase, ILoginViewModel
	{
		public LoginViewModel()
		{
			AppSettings.Current.PropertyChanged += Current_PropertyChanged;
		}

		private void Current_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
		{
			if(String.Compare(e.PropertyName, nameof(AppSettings.Current.AutoLogin)) == 0)
			{
				RaisePropertyChanged(nameof(AutoLogin));
			}
		}

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

		protected RelayCommand<Tuple<bool, ICancelable>> command;
		public virtual ICommand Command
		{
			get
			{
				return command ?? (command =
					new RelayCommand<Tuple<bool, ICancelable>>(async (t) =>
				{
					IsAuthenticating = true;
					var def = t.Item2.GetDeferal();
					if (await loginAsync(t.Item1) && ViewModelLocator.Cloud.IsAuthenticated)
					{
						ViewModelLocator.Messenger.Send<LoggedInMessage>(new LoggedInMessage());
					}
					else
					{
						t.Item2.Cancel = true;
					}
					IsAuthenticating = false;
					def.Complete();
				})
				);
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
			get
			{
				return AppSettings.Current.AutoLogin;
			}
			set
			{
				AppSettings.Current.AutoLogin = value;
			}
		}

		/// <summary>
		/// Gets a value indicating whether we should automatically login
		/// </summary>
		/// <value>
		/// <c>true</c> if [should automatic login]; otherwise, <c>false</c>.
		/// </value>
		/// <exception cref="System.NotImplementedException"></exception>
		public bool ShouldAutoLogin
		{
			get
			{
				return AutoLogin && !String.IsNullOrWhiteSpace(Username) && !String.IsNullOrWhiteSpace(Password);
			}
		}

		private ICommand forgotPasswordCommand;

		/// <summary>
		/// Gets the forgot password command.
		/// </summary>
		/// <value>
		/// The forgot password command.
		/// </value>
		public ICommand ForgotPasswordCommand
		{
			get
			{
				return forgotPasswordCommand ?? (forgotPasswordCommand = new RelayCommand(async () =>
				{
					await Windows.System.Launcher.LaunchUriAsync(new Uri("https://build.particle.io/forgot-password"));
				}));
			}
		}

		/// <summary>
		/// Loads values from the store
		/// </summary>
		public void Load(bool isLogout = false)
		{
			var settings = AppSettings.Current;
			Username = settings.Username;
			RememberPassword = settings.RememberPassword;
			if (RememberPassword)
			{
				if (!isLogout)
				{
					Password = settings.GetStoredPassword();
				}
				else
				{
					Password = "";
				}
				AutoLogin = settings.AutoLogin;
			}			
		}

		protected AppSettings SavePresists()
		{
			var settings = AppSettings.Current;
			settings.Username = username;
			if (RememberPassword)
			{
				settings.StorePassword(password);
			}
			settings.RememberPassword = rememberPassword;
			return settings;
		}

		/// <summary>
		/// Attempts to Login to the cloud
		/// </summary>
		/// <returns></returns>
		protected virtual async Task<bool> loginAsync(bool isAutologin)
		{
			var errors = new List<String>();
			if (String.IsNullOrWhiteSpace(Username) || !Username.Contains("@"))
			{
				errors.Add(MM.M.GetString("MustBeAValidEmailAddress"));
			}
			if(String.IsNullOrWhiteSpace(Password))
			{
				errors.Add(MM.M.GetString("PasswordIsRequired"));
			}

			if(errors.Count > 0)
			{
				ViewModelLocator.Messenger.Send<DialogMessage>(new DialogMessage()
				{
					Title = "Error",
					Description = String.Join(Environment.NewLine, errors)
				});
				return false;
			}
			AppSettings settings;
			if (!isAutologin)
			{
				settings = SavePresists();
			}
			else
			{
				settings = AppSettings.Current;
			}

			var result = await ViewModelLocator.Cloud.LoginWithUserAsync(username, password);
			if(result.Success)
			{
				return true;
			}
			else
			{
				AutoLogin = false;
				ViewModelLocator.Messenger.Send<DialogMessage>(new DialogMessage()
				{
					Title = result.Error ?? "",
					Description = result.ErrorDescription ?? ""
				});
				return false;
			}
		}
	}
}