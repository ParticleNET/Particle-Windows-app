using ParticleApp.Business.Interfaces;
using ParticleApp.Business.Messages;
using ParticleApp.Business.ViewModel;
using ParticleApp.UWP.Common;
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
	public sealed partial class AuthDialog : ContentDialog
	{
		private Action<ICancelable> callback;

		public AuthDialog()
		{
			this.InitializeComponent();
			ViewModelLocator.Messenger.Register<AuthSwitchMessage>(this, (m) =>
			{
				PrimaryButtonText = m.ButtonText;
				callback = m.Callback;
			});
		}

		private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
		{
			callback?.Invoke(new ContentDialogCancelable(args));
		}

		private void ContentDialog_SecondaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
		{
		}
	}
}
