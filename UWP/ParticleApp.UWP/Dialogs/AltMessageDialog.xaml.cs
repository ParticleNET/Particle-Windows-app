using ParticleApp.Business.Messages;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using WinRTXamlToolkit.Async;

// The Content Dialog item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace ParticleApp.UWP.Dialogs
{
	public sealed partial class AltMessageDialog : ContentDialog
	{
		public AltMessageDialog()
		{
			this.InitializeComponent();
		}

		public readonly static AsyncLock ALocker = new AsyncLock();

		private Action<int> callback;

		public async void ShowMessageDialog(DialogMessage message)
		{
			using (await ALocker.LockAsync())
			{
				callback = null;
				if (message.Description != null && message.Title != null)
				{
					Title = message.Title;
					Body.Text = message.Description;
				}
				else
				{
					Title = "";
					Body.Text = message.Description;
				}

				LinkContainer.Children.Clear();
				if (message.Buttons != null && message.Buttons.Count > 0)
				{
					IsPrimaryButtonEnabled = false;
					foreach (var b in message.Buttons)
					{
						Button button = new Button();
						button.Margin = new Thickness(0, 12, 12, 0);
						button.Content = b.Text;
						button.Tag = b.Id;
						button.Tapped += Button_Tapped;
						LinkContainer.Children.Add(button);
					}
				}
				else
				{
					IsPrimaryButtonEnabled = true;
					PrimaryButtonText = "Ok";
				}

				if (message.CallBack != null)
				{
					callback = message.CallBack;
				}



				await ShowAsync();
			}
		}

		private void Button_Tapped(object sender, TappedRoutedEventArgs e)
		{
			Button button = sender as Button;
			int id = (int)button.Tag;
			if (callback != null)
			{
				callback(id);
			}
			Hide();
		}
	}
}
