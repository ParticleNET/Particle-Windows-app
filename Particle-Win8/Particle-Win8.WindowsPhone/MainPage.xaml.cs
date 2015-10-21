using Particle.Common.Messages;
using Particle.Common.ViewModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
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

			this.NavigationCacheMode = NavigationCacheMode.Required;
			Window.Current.Activated += Current_Activated;
			ViewModelLocator.Messenger.Register<LoggedInMessage>(this, loggedIn);
		}
		
		private void loggedIn(LoggedInMessage message)
		{
			LoginDialog.Hide();
		}

		private void showDialogMessage(DialogMessage mes)
		{
			Dialog.ShowMessageDialog(mes);
		}

		protected override void OnNavigatedTo(NavigationEventArgs e)
		{
			base.OnNavigatedTo(e);
			ViewModelLocator.Messenger.Register<DialogMessage>(this, showDialogMessage);
		}

		protected override void OnNavigatedFrom(NavigationEventArgs e)
		{
			base.OnNavigatedFrom(e);
			ViewModelLocator.Messenger.Unregister<DialogMessage>(this);
		}

		private async void Current_Activated(object sender, Windows.UI.Core.WindowActivatedEventArgs e)
		{
			if (!ViewModelLocator.Cloud.IsAuthenticated)
			{
				if (!LoginDialog.IsOpen)
				{
					await LoginDialog.ShowAsync();
				}
			}
		}

		private void LoginDialog_Closing(ContentDialog sender, ContentDialogClosingEventArgs args)
		{
			if (!ViewModelLocator.Cloud.IsAuthenticated)
			{
				Application.Current.Exit();
			}
		}
	}
}
