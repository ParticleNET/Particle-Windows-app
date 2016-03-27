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
using ParticleApp.Business.Models;
using ParticleApp.Business.ViewModel;
using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace ParticleApp.Business.Pages
{
	/// <summary>
	/// An empty page that can be used on its own or navigated to within a Frame.
	/// </summary>
	public sealed partial class LoginPage : Page
	{
		private ILoginViewModel _viewModel;

		private ILoginViewModel viewModel
		{
			get
			{
				return _viewModel ?? (_viewModel = (ILoginViewModel)DataContext);
			}
		}

		public LoginPage()
		{
			this.InitializeComponent();
			viewModel.Load();
		}

		protected override void OnNavigatedTo(NavigationEventArgs e)
		{
			base.OnNavigatedTo(e);
#if WINDOWS_UWP
			ViewModelLocator.Messenger.Send(new AuthSwitchMessage(LoginAction.Content.ToString(), doAuth));
			LoginAction.Visibility = Visibility.Collapsed;
#endif
			if (((bool?)e.Parameter) == true)
			{
				if (viewModel.ShouldAutoLogin)
				{
					doAuth(new BasicCancelable());
				}
			}
		}

		private void doAuth(ICancelable cancel)
		{
			var t = new Tuple<bool, ICancelable>(false, cancel);
			if(viewModel.Command.CanExecute(t))
			{
				viewModel.Command.Execute(t);
			}
		}

		private void RegisterAction_Click(object sender, RoutedEventArgs e)
		{
			Frame.Navigate(typeof(RegisterPage));
		}
	}
}
