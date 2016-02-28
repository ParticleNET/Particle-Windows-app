/*
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
using Particle.Common;
using Particle.Common.Messages;
using Particle.Common.ViewModel;
using System;
using Windows.ApplicationModel.DataTransfer;
using Windows.Networking.Connectivity;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using WinRTXamlToolkit.Async;

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
			ViewModelLocator.Messenger.Register<LoggedInMessage>(this, (a) => { checkLoggedIn(); });
			ViewModelLocator.Messenger.Register<LoggedOutMessage>(this, (a) => { checkLoggedIn(); });
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
			GalaSoft.MvvmLight.Messaging.Messenger.Default.Register<CopyToClipboardMessage>(this, copyToClipboardReceiver);
		}

		private void copyToClipboardReceiver(CopyToClipboardMessage message)
		{
			DataPackage package = new DataPackage();
			package.SetText(message.Content);
			Clipboard.SetContent(package);
			if (!String.IsNullOrWhiteSpace(message.SuccessMessage))
			{
				dialogMessageReceiver(new DialogMessage
				{
					Description = message.SuccessMessage
				});
			}
		}

		private AsyncLock alock = new AsyncLock();

		private async void inputDialogMessageReceiver(InputDialogMessage dm)
		{
			InputDialog.InputText = "";
			String result = "";
			using (await alock.LockAsync())
			{
				if (dm.Buttons != null)
				{
					result = await InputDialog.ShowAsync(dm.Title ?? "", dm.Description ?? "", dm.Buttons);
				}
				else
				{
					result = await InputDialog.ShowAsync(dm.Title ?? "", dm.Description ?? "");
				}
			}
			if (dm.CallBack != null)
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

			IUICommand result;
			using (await alock.LockAsync())
			{
				result = await dialog.ShowAsync();
			}
			if (dm.CallBack != null)
			{
				dm.CallBack(Convert.ToInt32(result.Id));
			}
		}

		private void checkInternetAccess()
		{
			var profile = NetworkInformation.GetInternetConnectionProfile();
			if (profile != null)
			{
				var state = profile.GetNetworkConnectivityLevel();
				switch (state)
				{
					case NetworkConnectivityLevel.None:
						DialogMessage dm = new DialogMessage();
						dm.Description = MM.M.GetString("Network_NoConnectivity");
						ViewModelLocator.Messenger.Send(dm);
						break;

					case NetworkConnectivityLevel.ConstrainedInternetAccess:
						DialogMessage dm1 = new DialogMessage();
						dm1.Description = MM.M.GetString("Network_ConstrainedInternetAccess");
						ViewModelLocator.Messenger.Send(dm1);
						break;

					case NetworkConnectivityLevel.LocalAccess:
						DialogMessage dm2 = new DialogMessage();
						dm2.Description = MM.M.GetString("Network_LocalAccess");
						ViewModelLocator.Messenger.Send(dm2);
						break;

					default:
						break;
				}
			}
			else
			{
				DialogMessage d = new DialogMessage();
				d.Description = MM.M.GetString("Network_NoConnectivity");
				ViewModelLocator.Messenger.Send(d);
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
				if (Window.Current.Bounds.Width < 800)
				{
					DevicesDropDown.Visibility = Visibility.Visible;
					DevicesList.Visibility = Visibility.Collapsed;
					Header.Orientation = Orientation.Vertical;
					LogoutControl.VerticalAlignment = VerticalAlignment.Top;
					LogoImage.Margin = new Thickness(0, 0, 0, 24);
				}
				else
				{
					DevicesDropDown.Visibility = Visibility.Collapsed;
					DevicesList.Visibility = Visibility.Visible;
					Header.Orientation = Orientation.Horizontal;
					LogoutControl.VerticalAlignment = VerticalAlignment.Center;
					LogoImage.Margin = new Thickness(0, 0, 24, 0);
				}
			}
		}

		private void MainPage_Unloaded(object sender, RoutedEventArgs e)
		{
			Window.Current.SizeChanged -= Current_SizeChanged;
		}

		private void checkLoggedIn()
		{
			if (ViewModelLocator.Cloud.IsAuthenticated)
			{
				LoginPopup.IsOpen = false;
				TinkerContainer.Visibility = Visibility.Visible;
				LogoutControl.Visibility = Visibility.Visible;
				checkSize();
			}
			else
			{
				LoginPopup.IsOpen = true;
				TinkerContainer.Visibility = Visibility.Collapsed;
				LogoutControl.Visibility = Visibility.Collapsed;
			}
		}

		protected override void OnNavigatedTo(NavigationEventArgs e)
		{
			base.OnNavigatedTo(e);
			checkInternetAccess();
			checkLoggedIn();
		}
	}
}
