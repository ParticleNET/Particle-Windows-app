using GalaSoft.MvvmLight.Command;
using Particle.Common.Interfaces;
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
					registerAsync();
				})) ;
			}
		}

		private async void registerAsync()
		{
			List<String> errors = new List<string>();
			if (String.IsNullOrWhiteSpace(Username) || !Username.Contains("@"))
			{
				errors.Add(Messages.MM.GetString("MustBeAValidEmailAddress"));
			}
		}
	}
}
