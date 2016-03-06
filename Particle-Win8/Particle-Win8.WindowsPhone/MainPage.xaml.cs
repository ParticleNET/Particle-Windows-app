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
using Particle.Common.Messages;
using Particle.Common.ViewModel;
using Particle_Win8.Controls;
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
using WinRTXamlToolkit.Async;
using WinRTXamlToolkit.Controls;

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

			//this.NavigationCacheMode = NavigationCacheMode.Required;
			Window.Current.Activated += Current_Activated;
			ViewModelLocator.Messenger.Register<LoggedInMessage>(this, loggedIn);
		}

		private void loggedIn(LoggedInMessage message)
		{
			LoginDialog.Hide();
		}
		
		private void copyToClipboardReceiver(CopyToClipboardMessage message)
		{
			DataPackage package = new DataPackage();
			package.SetText(message.Content);
			// Clipboard api is missing from Winrt apps for windows phone 8.1
			//Clipboard.SetContent(package);
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
			if(ViewModelLocator.DevicesListViewModel.SelectedDevice != null)
			{
				ViewModelLocator.DevicesListViewModel.SelectedDevice = null;
			}
			ViewModelLocator.Messenger.Register<DialogMessage>(this, (mes)=> { Dialog.ShowMessageDialog(mes); });
			ViewModelLocator.Messenger.Register<InputDialogMessage>(this,(mes) => { InputDialog.ShowInputDialog(mes); });
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
