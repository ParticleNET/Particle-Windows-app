using InTheHand.ApplicationModel.DataTransfer;
using Particle.Common.Messages;
using Particle.Common.ViewModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.ApplicationModel.DataTransfer;
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

			this.NavigationCacheMode = NavigationCacheMode.Required;
			Window.Current.Activated += Current_Activated;
			ViewModelLocator.Messenger.Register<LoggedInMessage>(this, loggedIn);
		}

		private void loggedIn(LoggedInMessage message)
		{
			LoginDialog.Hide();
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
			if (dm.CallBack != null)
			{
				dm.CallBack(result, InputDialog.InputText);
			}
		}

		private void copyToClipboardReceiver(CopyToClipboardMessage message)
		{
			
			DataPackage package = new DataPackage();
			package.SetText(message.Content);
			Clipboard.SetContent(package);
			if (!String.IsNullOrWhiteSpace(message.SuccessMessage))
			{
				ViewModelLocator.Messenger.Send(new DialogMessage
				{
					Description = message.SuccessMessage
				});
			}
		}

		protected override void OnNavigatedTo(NavigationEventArgs e)
		{
			base.OnNavigatedTo(e);
			ViewModelLocator.Messenger.Register<DialogMessage>(this, (mes)=> { Dialog.ShowMessageDialog(mes); });
			ViewModelLocator.Messenger.Register<InputDialogMessage>(this, inputDialogMessageReceiver);
			ViewModelLocator.Messenger.Register<CopyToClipboardMessage>(this, copyToClipboardReceiver);
			ViewModelLocator.DevicesListViewModel.PropertyChanged += DevicesListViewModel_PropertyChanged;
		}

		private void DevicesListViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
		{
			if(String.Compare(e.PropertyName, nameof(DevicesListViewModel.SelectedDevice)) == 0)
			{
				if(ViewModelLocator.DevicesListViewModel.SelectedDevice != null)
				{
					Frame.Navigate(typeof(DevicePage));
				}
			}
		}

		protected override void OnNavigatedFrom(NavigationEventArgs e)
		{
			base.OnNavigatedFrom(e);
			ViewModelLocator.Messenger.Unregister<DialogMessage>(this);
			ViewModelLocator.Messenger.Unregister<InputDialogMessage>(this);
			ViewModelLocator.Messenger.Unregister<CopyToClipboardMessage>(this);
			ViewModelLocator.DevicesListViewModel.PropertyChanged -= DevicesListViewModel_PropertyChanged;
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
