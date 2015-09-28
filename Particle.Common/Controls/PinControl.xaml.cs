using Particle.Common.Interfaces;
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

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace Particle.Common.Controls
{
	public sealed partial class PinControl : UserControl
	{
		public PinControl()
		{
			this.InitializeComponent();
		}

		private void holding()
		{
			var vm = (IPinViewModel)DataContext;
			if (vm.Holding.CanExecute(null))
			{
				vm.Holding.Execute(null);
			}
		}

		private void Button_Holding(object sender, HoldingRoutedEventArgs e)
		{
			holding();
		}

		private void Button_RightTapped(object sender, RightTappedRoutedEventArgs e)
		{
			holding();
		}

		private void Button_DoubleTapped(object sender, DoubleTappedRoutedEventArgs e)
		{
			holding();
		}
	}
}
