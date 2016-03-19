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
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace Particle_Win8
{
	/// <summary>
	/// An empty page that can be used on its own or navigated to within a Frame.
	/// </summary>
	public sealed partial class AuthPage : Page
	{
		public AuthPage()
		{
			this.InitializeComponent();
			ViewModelLocator.Messenger.Register<LoggedInMessage>(this, loggedIn);

		}

		private void loggedIn(LoggedInMessage obj)
		{
			if (ViewModelLocator.Cloud.IsAuthenticated)
			{
				Frame.Navigate(typeof(MainPage));
				Frame.BackStack.Clear();
			}
		}

		/// <summary>
		/// Invoked when this page is about to be displayed in a Frame.
		/// </summary>
		/// <param name="e">Event data that describes how this page was reached.
		/// This parameter is typically used to configure the page.</param>
		protected override void OnNavigatedTo(NavigationEventArgs e)
		{
			ViewModelLocator.Messenger.Register<DialogMessage>(this, (mes) => { Dialog.ShowMessageDialog(mes); });
			ViewModelLocator.Messenger.Register<InputDialogMessage>(this, (mes) => { InputDialog.ShowInputDialog(mes); });
			App.CheckInternetAccess();
		}

		protected override void OnNavigatedFrom(NavigationEventArgs e)
		{
			base.OnNavigatedFrom(e);
			ViewModelLocator.Messenger.Unregister<DialogMessage>(this);
			ViewModelLocator.Messenger.Unregister<InputDialogMessage>(this);
		}
	}
}
