using ParticleApp.Business.Messages;
using ParticleApp.Business.ViewModel;
using ParticleApp.UWP.Dialogs;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.ApplicationModel.DataTransfer;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.System;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using WinRTXamlToolkit.Async;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace ParticleApp.UWP
{
	/// <summary>
	/// An empty page that can be used on its own or navigated to within a Frame.
	/// </summary>
	public sealed partial class MainPage : Page
	{
		private bool isIot = false;
		private bool isMobile = false;
		private AltMessageDialog dialog;

		public MainPage()
		{
			this.InitializeComponent();
			//Windows.System.ShutdownManager
			//var hasShutdownManager = Windows.Foundation.Metadata.ApiInformation.IsTypePresent("Windows.System.ShutdownManager");
			isIot = String.Compare(Windows.System.Profile.AnalyticsInfo.VersionInfo.DeviceFamily, "Windows.IoT") == 0;
			isMobile = String.Compare(Windows.System.Profile.AnalyticsInfo.VersionInfo.DeviceFamily, "Windows.Mobile") == 0;
			if(isIot || isMobile) // IoT does not support the default MessageDialog. Mobile does not support more then 2 buttons so switch to our custom dialog instead.
			{
				dialog = new AltMessageDialog();
				GridContainer.Children.Add(dialog);
				dialog.SetValue(Grid.RowSpanProperty, GridContainer.RowDefinitions.Count);
			}
			ViewModelLocator.Messenger.Register<DialogMessage>(this, dialogMessage);
			ViewModelLocator.Messenger.Register<InputDialogMessage>(this, (mes) => { InputDialog.ShowInputDialog(mes); });
			ViewModelLocator.Messenger.Register<LoggedInMessage>(this, (a) => { checkLoggedIn(); });
			ViewModelLocator.Messenger.Register<LoggedOutMessage>(this, (a) => { checkLoggedIn(); });
			ViewModelLocator.Messenger.Register<CopyToClipboardMessage>(this, copyToClipboardReceiver);
			ViewModelLocator.Messenger.Register<SelectedDeviceMessage>(this, selectedDeviceChanged);
		}

		private void selectedDeviceChanged(SelectedDeviceMessage message)
		{
			if(Window.Current.Bounds.Width < 720)
			{
				if(message.Device == null)
				{
					if (!Splitter.IsPaneOpen)
					{
						Splitter.IsPaneOpen = true;
					}
				}
				else
				{
					if (Splitter.IsPaneOpen)
					{
						Splitter.IsPaneOpen = false;
					}
				}
			}
		}


		private void copyToClipboardReceiver(CopyToClipboardMessage message)
		{
			DataPackage package = new DataPackage();
			package.SetText(message.Content);
			Clipboard.SetContent(package);
			if (!String.IsNullOrWhiteSpace(message.SuccessMessage))
			{
				dialogMessage(new DialogMessage
				{
					Description = message.SuccessMessage
				});
			}
		}

		private async void dialogMessage(DialogMessage dm)
		{
			if (this.dialog != null)
			{
				this.dialog.ShowMessageDialog(dm);
			}
			else
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
				using (await App.locker.LockAsync())
				{
					result = await dialog.ShowAsync();
				}
				if (dm.CallBack != null)
				{
					dm.CallBack(Convert.ToInt32(result.Id));
				}
			}
		}

		protected override void OnNavigatedTo(NavigationEventArgs e)
		{
			base.OnNavigatedTo(e);
			checkLoggedIn();
		}

		private async void checkLoggedIn()
		{
			if (!ViewModelLocator.Cloud.IsAuthenticated)
			{
				LogoutButton.Visibility = Visibility.Collapsed;
				await Task.Delay(200);
				await AuthDialog.ShowAsync();
			}
			else
			{
				LogoutButton.Visibility = Visibility.Visible;
				AuthDialog.Hide();
			}
		}

		private void Button_Click(object sender, RoutedEventArgs e)
		{
			Splitter.IsPaneOpen = !Splitter.IsPaneOpen;
		}
	}
}
