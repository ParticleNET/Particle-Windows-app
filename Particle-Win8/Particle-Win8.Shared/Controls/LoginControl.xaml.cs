using Particle.Common.ViewModel;
using Particle_Win8.Pages;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace Particle_Win8.Controls
{
	public sealed partial class LoginControl : UserControl
	{
		public LoginControl()
		{
			this.InitializeComponent();
			if (!String.IsNullOrWhiteSpace(AppSettings.Current.Username))
			{
				LoginFrame.Navigate(typeof(LoginPage));
			}
			else
			{
				LoginFrame.Navigate(typeof(RegisterPage));
			}
		}
	}
}
