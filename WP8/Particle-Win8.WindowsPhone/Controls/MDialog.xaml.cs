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
using ParticleApp.Business.Messages;
using System;
using System.Linq;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
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

				if (message.CallBack != null)
				{
					callback = message.CallBack;
				}



				await ShowAsync();
			}
		}

		private void Button_Tapped(object sender, TappedRoutedEventArgs e)
		{
			HyperlinkButton button = sender as HyperlinkButton;
			int id = (int)button.Tag;
			if (callback != null)
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
