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
using Windows.Networking.Connectivity;
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
			NetworkInformation.NetworkStatusChanged += async (s) =>
			{
				await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
				{
					checkInternetAccess();
				});
			};
			GalaSoft.MvvmLight.Messaging.Messenger.Default.Register<InputDialogMessage>(this, inputDialogMessageReceiver);

			GalaSoft.MvvmLight.Messaging.Messenger.Default.Register<DialogMessage>(this, dialogMessageReceiver);
		}

		private async void inputDialogMessageReceiver(InputDialogMessage dm)
		{
			InputDialog.InputText = "";
			String result = "";
			if (dm.Buttons != null)
			{
				result = await InputDialog.ShowAsync(dm.Title ?? "", dm.Description ?? "", dm.Buttons);
			}
			else
			{
				result = await InputDialog.ShowAsync(dm.Title ?? "", dm.Description ?? "");
			}
			if(dm.CallBack != null)
			{
				dm.CallBack(result, InputDialog.InputText);
			}
		}

		private async void dialogMessageReceiver(DialogMessage dm)
		{
			MessageDialog dialog = null;
			if (dm.Description != null && dm.Title != null)
			{
				dialog = new MessageDialog(dm.Description, dm.Title);
			}
			else
			{
				dialog = new MessageDialog(dm.Description ?? "");
			}

			if (dm.Buttons != null && dm.Buttons.Count > 0)
			{
				foreach (var b in dm.Buttons)
				{
					dialog.Commands.Add(new UICommand(b.Text) { Id = b.Id });
				}
			}

			var result = await dialog.ShowAsync();
			if (dm.CallBack != null)
			{
				dm.CallBack(Convert.ToInt32(result.Id));
			}
		}

		private async void checkInternetAccess()
		{
			var profile = NetworkInformation.GetInternetConnectionProfile();
			if(profile != null)
			{
				var state = profile.GetNetworkConnectivityLevel();
				switch (state)
				{
					case NetworkConnectivityLevel.None:
						MessageDialog dialog = new MessageDialog("There is currently no network connectivity.");
						await dialog.ShowAsync();
						break;

					case NetworkConnectivityLevel.ConstrainedInternetAccess:
						MessageDialog d1 = new MessageDialog("There is currently constrained Internet access and this app may not function correctly.");
						await d1.ShowAsync();
						break;

					case NetworkConnectivityLevel.LocalAccess:
						MessageDialog d2 = new MessageDialog("You currently only have local network access. This app may not function correctly.");
						await d2.ShowAsync();
						break;

					default:
						break;
				}
			}
			else
			{
				MessageDialog dialog = new MessageDialog("There is currently no network connectivity. This app may not function correctly.");
				await dialog.ShowAsync();
			}
		}

		private void Current_SizeChanged(object sender, Windows.UI.Core.WindowSizeChangedEventArgs e)
		{
			checkSize();
		}

		private void checkSize()
		{
			if (ViewModelLocator.Cloud.IsAuthenticated)
			{
				if(Window.Current.Bounds.Width < 800)
				{
					DevicesDropDown.Visibility = Visibility.Visible;
					DevicesList.Visibility = Visibility.Collapsed;
				}
				else
				{
					DevicesDropDown.Visibility = Visibility.Collapsed;
					DevicesList.Visibility = Visibility.Visible;
				}
			}
		}

		private void MainPage_Unloaded(object sender, RoutedEventArgs e)
		{
			Window.Current.SizeChanged -= Current_SizeChanged;
		}

		private void loggedIn(LoggedInMessage message)
		{
			LoginPopup.IsOpen = false;
			TinkerContainer.Visibility = Visibility.Visible;
			checkSize();
		}

		protected override void OnNavigatedTo(NavigationEventArgs e)
		{
			base.OnNavigatedTo(e);
			checkInternetAccess();
			if (!ViewModelLocator.Cloud.IsAuthenticated)
			{
				LoginPopup.IsOpen = true;
				TinkerContainer.Visibility = Visibility.Collapsed;
			}
			else
			{
				LoginPopup.IsOpen = false;
				TinkerContainer.Visibility = Visibility.Visible;
				checkSize();
			}
		}
	}
}
