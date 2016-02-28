using Particle.Common.ViewModel;
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
	public sealed partial class AuthDialog : ContentDialog
	{
		public bool IsOpen { get; set; }
		public AuthDialog()
		{
			IsOpen = false;
			this.InitializeComponent();
			this.Opened += AuthDialog_Opened;
			this.Closed += AuthDialog_Closed;
		}

		private void AuthDialog_Closed(ContentDialog sender, ContentDialogClosedEventArgs args)
		{
			IsOpen = false;
		}

		private void AuthDialog_Opened(ContentDialog sender, ContentDialogOpenedEventArgs args)
		{
			IsOpen = true;
		}

		private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
		{
		}

		private void ContentDialog_SecondaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
		{
		}
	}
}
