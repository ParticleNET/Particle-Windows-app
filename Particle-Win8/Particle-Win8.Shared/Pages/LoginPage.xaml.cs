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

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace Particle_Win8.Pages
{
	/// <summary>
	/// An empty page that can be used on its own or navigated to within a Frame.
	/// </summary>
	public sealed partial class LoginPage : Page
	{
		private ILoginViewModel _viewModel;

		private ILoginViewModel viewModel
		{
			get
			{
				return _viewModel ?? (_viewModel = (ILoginViewModel)DataContext);
			}
		}

		public LoginPage()
		{
			this.InitializeComponent();
			viewModel.Load();
		}

		protected override void OnNavigatedTo(NavigationEventArgs e)
		{
			base.OnNavigatedTo(e);
			if(((bool?)e.Parameter) == true)
			{
				if (viewModel.ShouldAutoLogin)
				{
					if (viewModel.Command.CanExecute(true))
					{
						viewModel.Command.Execute(true);
					}
				}
			}
		}

		private void RegisterAction_Click(object sender, RoutedEventArgs e)
		{
			Frame.Navigate(typeof(RegisterPage));
		}
	}
}
