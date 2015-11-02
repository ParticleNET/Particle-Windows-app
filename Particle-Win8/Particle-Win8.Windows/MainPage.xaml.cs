using Particle.Common.Messages;
using Particle.Common.ViewModel;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Devices.Enumeration;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace Particle_Win8
{
	/// <summary>
	/// An empty page that can be used on its own or navigated to within a Frame.
	/// </summary>
	public sealed partial class MainPage : Page
	{
		public MainPage()
		{
			this.InitializeComponent();
			ViewModelLocator.Messenger.Register<LoggedInMessage>(this, loggedIn);
			Window.Current.SizeChanged += Current_SizeChanged;
			Unloaded += MainPage_Unloaded;
		}

		private void Current_SizeChanged(object sender, Windows.UI.Core.WindowSizeChangedEventArgs e)
		{
			
		}

		private void MainPage_Unloaded(object sender, RoutedEventArgs e)
		{
			Window.Current.SizeChanged -= Current_SizeChanged;
		}

		private void loggedIn(LoggedInMessage message)
		{
			LoginPopup.IsOpen = false;
		}

		protected override void OnNavigatedTo(NavigationEventArgs e)
		{
			base.OnNavigatedTo(e);
			if (!ViewModelLocator.Cloud.IsAuthenticated)
			{
				LoginPopup.IsOpen = true;
			}
			else
			{
				LoginPopup.IsOpen = false;
			}
		}

		private void LoginControl_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
		{
			if(String.Compare(e.PropertyName, "IsAuthenticating") == 0)
			{

			}
		}
	}
}
