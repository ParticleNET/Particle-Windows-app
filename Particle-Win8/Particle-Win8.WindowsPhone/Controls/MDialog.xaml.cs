using Particle.Common.Messages;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage.Pickers.Provider;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using WinRTXamlToolkit.Async;

// The Content Dialog item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace Particle_Win8.Controls
{
	public sealed partial class MDialog : ContentDialog
	{
		public MDialog()
		{
			this.InitializeComponent();
		}

		public readonly static AsyncLock ALocker = new AsyncLock();

		private Action<int> callback;

		public async void ShowMessageDialog(DialogMessage message)
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
					HyperlinkButton button = new HyperlinkButton();
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

			if(message.CallBack != null)
			{
				callback = message.CallBack;
			}


			using (await ALocker.LockAsync())
			{
				await ShowAsync();
			}
		}

		private void Button_Tapped(object sender, TappedRoutedEventArgs e)
		{
			HyperlinkButton button = sender as HyperlinkButton;
			int id = (int)button.Tag;
			if(callback != null)
			{
				callback(id);
			}
			Hide();
		}

		private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
		{
			if (callback != null)
			{
				callback(0);
			}
		}
	}
}
