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
		public bool IsProcessing
		{
			get
			{
				return false;
			}
			set { }
		}

		private RelayCommand loginCommand;
		public ICommand LoginCommand
		{
			get
			{
				return loginCommand ?? (loginCommand = new RelayCommand(() => { }));
			}
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

		public event PropertyChangedEventHandler PropertyChanged;

		public void Load()
		{
		}
	}
}
