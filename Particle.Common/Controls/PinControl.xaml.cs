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
		private IPinViewModel viewModel
		{
			get
			{
				return DataContext as IPinViewModel;
			}
		}
		public PinControl()
		{
			this.InitializeComponent();
		}

		private void ViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
		{
			if (String.Compare(e.PropertyName, nameof(IPinViewModel.ShowSelect)) == 0)
			{
				if (viewModel.ShowSelect)
				{
					ButtonFlyout.ShowAt(this);
				}
				else
				{
					//PinAction.Flyout = null;
				}
			}
		}

		private void holding()
		{
			if (viewModel != null)
			{
				if (viewModel.Holding.CanExecute(null))
				{
					viewModel.Holding.Execute(null);
				}
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

		private void UserControl_DataContextChanged(FrameworkElement sender, DataContextChangedEventArgs args)
		{
			if (args.NewValue is IPinViewModel)
			{
				(args.NewValue as IPinViewModel).PropertyChanged += ViewModel_PropertyChanged;
			}
		}

		private void Slider_PointerCaptureLost(object sender, PointerRoutedEventArgs e)
		{
			if(viewModel != null)
			{
				if (viewModel.AnalogManipulationComplete.CanExecute(null))
				{
					viewModel.AnalogManipulationComplete.Execute(null);
				}
			}
		}
	}
}
