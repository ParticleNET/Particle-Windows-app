/*
   Copyright 2016 Sannel Software, L.L.C.

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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Particle.Common.ViewModel
{
	public class RegisterViewModel : LoginViewModel, IRegisterViewModel
	{
		/// <summary>
		/// The verify password for checking against
		/// </summary>
		/// <value>
		/// The verify password.
		/// </value>
		public string VerifyPassword
		{
			get;
			set;
		}

		/// <summary>
		/// Determines whether form is valid.
		/// </summary>
		/// <returns></returns>
		private bool isFormValid()
		{
			List<String> errors = new List<string>();
			if (String.IsNullOrWhiteSpace(Username) || !Username.Contains("@"))
			{
				errors.Add(MM.M.GetString("MustBeAValidEmailAddress"));
			}

			if (String.IsNullOrEmpty(Password))
			{
				errors.Add(MM.M.GetString("PasswordIsRequired"));
			}
			if(String.IsNullOrEmpty(VerifyPassword))
			{
				errors.Add(MM.M.GetString("VerifyPasswordIsRequired"));
			}
			if(String.Compare(Password, VerifyPassword) != 0)
			{
				errors.Add(MM.M.GetString("PasswordMustMatch"));
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
			return true;
		}

		protected override async Task<bool> loginAsync()
		{
			if (isFormValid())
			{
				
				var result = await ViewModelLocator.Cloud.SignupWithUserAsync(Username, Password);
				if (result.Success)
				{
					return await base.loginAsync();
				}
				else
				{
					ViewModelLocator.Messenger.Send<DialogMessage>(new DialogMessage()
					{
						Title = result.Error ?? "",
						Description = result.ErrorDescription ?? ""
					});
					return false;
				}
			}
			return false;
		}
	}
}
