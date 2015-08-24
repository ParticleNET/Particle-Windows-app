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
		/// Are we currently trying to loggin?
		/// </summary>
		bool IsProcessing { get; set; }
		/// <summary>
		/// Loads the stored data into the view
		/// </summary>
		void Load();
		/// <summary>
		/// Represents the ICommand for the login action
		/// </summary>
		ICommand LoginCommand { get; }
	}
}
