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
		public string VerifyPassword
		{
			get;
			set;
		}
		
		public override ICommand Command
		{
			get
			{
				return command ?? (command = new RelayCommand(()=>
				{
					register();
				})) ;
			}
		}

		private void register()
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
			}
		}
	}
}
