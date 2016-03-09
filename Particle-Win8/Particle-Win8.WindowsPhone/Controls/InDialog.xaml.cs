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
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
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

// The Content Dialog item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace Particle_Win8.Controls
{
	public sealed partial class InDialog : ContentDialog
	{
		public InDialog()
		{
			this.InitializeComponent();
		}

		public async void ShowInputDialog(InputDialogMessage message)
		{
			using(var loc = await MDialog.ALocker.LockAsync())
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
				else if(message.Buttons?.Length > 1)
				{
					IsSecondaryButtonEnabled = true;
					PrimaryButtonText = message.Buttons[0];
					SecondaryButtonText = message.Buttons[1];
				}

				var result = await ShowAsync();
				if(message.CallBack != null)
				{
					if(result == ContentDialogResult.Primary)
					{
						message.CallBack(PrimaryButtonText, InputBox.Text);
					}
					else if(result == ContentDialogResult.Secondary)
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

		private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
		{
		}

		private void ContentDialog_SecondaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
		{
		}
	}
}
