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

// The Content Dialog item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace ParticleApp.UWP.Dialogs
{
	public sealed partial class InputDialog : ContentDialog
	{
		public InputDialog()
		{
			this.InitializeComponent();
		}


		public async void ShowInputDialog(InputDialogMessage message)
		{
			using (var loc = await App.locker.LockAsync())
			{
				InputBox.Text = String.Empty;
				if (!String.IsNullOrWhiteSpace(message.Title))
				{
					Title = message.Title;
				}
				else
				{
					Title = "";
				}

				if (!String.IsNullOrWhiteSpace(message.Description))
				{
					Description.Text = message.Description;
				}
				else
				{
					Description.Text = "";
				}

				if (message.Buttons?.Length == 1)
				{
					IsSecondaryButtonEnabled = false;
					PrimaryButtonText = message.Buttons[0];
				}
				else if (message.Buttons?.Length > 1)
				{
					IsSecondaryButtonEnabled = true;
					PrimaryButtonText = message.Buttons[0];
					SecondaryButtonText = message.Buttons[1];
				}

				var result = await ShowAsync();
				if (message.CallBack != null)
				{
					if (result == ContentDialogResult.Primary)
					{
						message.CallBack(PrimaryButtonText, InputBox.Text);
					}
					else if (result == ContentDialogResult.Secondary)
					{
						message.CallBack(SecondaryButtonText, InputBox.Text);
					}
					else
					{
						message.CallBack(null, InputBox.Text);
					}
				}
			}
		}
	}
}
