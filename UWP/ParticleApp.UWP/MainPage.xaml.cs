using ParticleApp.Business.Messages;
using ParticleApp.Business.ViewModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
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

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace ParticleApp.UWP
{
	/// <summary>
	/// An empty page that can be used on its own or navigated to within a Frame.
	/// </summary>
	public sealed partial class MainPage : Page
	{
		public MainPage()
		{
			this.InitializeComponent();
			ViewModelLocator.Messenger.Register<DialogMessage>(this, dialogMessage);
			ViewModelLocator.Messenger.Register<InputDialogMessage>(this, (mes) => { InputDialog.ShowInputDialog(mes); });
			ViewModelLocator.Messenger.Register<LoggedInMessage>(this, (a) => { checkLoggedIn(); });
			ViewModelLocator.Messenger.Register<LoggedOutMessage>(this, (a) => { checkLoggedIn(); });
		}


		

		private async void dialogMessage(DialogMessage dm)
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

		protected override void OnNavigatedTo(NavigationEventArgs e)
		{
			base.OnNavigatedTo(e);
			checkLoggedIn();
		}

		private async void checkLoggedIn()
		{
			if (!ViewModelLocator.Cloud.IsAuthenticated)
			{
				LogoutControl.Visibility = Visibility.Collapsed;
				await Task.Delay(200);
				await AuthDialog.ShowAsync();
			}
			else
			{
				LogoutControl.Visibility = Visibility.Visible;
			}
		}

		private void Button_Click(object sender, RoutedEventArgs e)
		{
			Splitter.IsPaneOpen = !Splitter.IsPaneOpen;
		}
	}
}
