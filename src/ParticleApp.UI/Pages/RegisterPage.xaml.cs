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
using ParticleApp.Business.Interfaces;
using ParticleApp.Business.Messages;
using ParticleApp.Business.ViewModel;
using System;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace ParticleApp.Business.Pages
{
	/// <summary>
	/// An empty page that can be used on its own or navigated to within a Frame.
	/// </summary>
	public sealed partial class RegisterPage : Page
	{
		private IRegisterViewModel _viewModel;

		private IRegisterViewModel viewModel
		{
			get
			{
				return _viewModel ?? (_viewModel = (IRegisterViewModel)DataContext);
			}
		}

		public RegisterPage()
		{
			this.InitializeComponent();
		}

#if WINDOWS_UWP
		protected override async void OnNavigatedTo(NavigationEventArgs e)
		{
			base.OnNavigatedTo(e);
			await Task.Delay(200);
			ViewModelLocator.Messenger.Send(new AuthSwitchMessage(MM.M.GetString("Register_Register"), doAuth));
			RegisterAction.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
		}
#endif

#if WINDOWS_UWP
		private void doAuth(ICancelable cancel)
		{
			var t = new Tuple<bool, ICancelable>(false, cancel);
			if (viewModel.Command.CanExecute(t))
			{
				viewModel.Command.Execute(t);
			}
		}
#endif

		private void HyperlinkButton_Tapped(object sender, TappedRoutedEventArgs e)
		{
			this.Frame.Navigate(typeof(LoginPage));
		}
	}
}
