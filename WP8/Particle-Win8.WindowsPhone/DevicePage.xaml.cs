using ParticleApp.Business.Interfaces;
using ParticleApp.Business.Messages;
using ParticleApp.Business.ViewModel;
using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace Particle_Win8
{
	/// <summary>
	/// An empty page that can be used on its own or navigated to within a Frame.
	/// </summary>
	public sealed partial class DevicePage : Page
	{
		public DevicePage()
		{
			this.InitializeComponent();
		}

		protected override void OnNavigatedTo(NavigationEventArgs e)
		{
			base.OnNavigatedTo(e);
			ViewModelLocator.DevicesListViewModel.PropertyChanged += DevicesListViewModel_PropertyChanged;
			ViewModelLocator.Messenger.Register<DialogMessage>(this, (mes) => { Dialog.ShowMessageDialog(mes); });
			ViewModelLocator.Messenger.Register<InputDialogMessage>(this, (mes) => { InputDialog.ShowInputDialog(mes); });
			App.CheckInternetAccess();
		}

		private void DevicesListViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
		{
			if(String.Compare(e.PropertyName, nameof(IDevicesListViewModel.SelectedDevice)) == 0)
			{
				if(ViewModelLocator.DevicesListViewModel.SelectedDevice == null)
				{
					Frame rootFrame = Window.Current.Content as Frame;

					if (rootFrame != null && rootFrame.CanGoBack)
					{
						rootFrame.GoBack();
					}
				}
			}
		}

		protected override void OnNavigatedFrom(NavigationEventArgs e)
		{
			base.OnNavigatedFrom(e);
			ViewModelLocator.DevicesListViewModel.PropertyChanged -= DevicesListViewModel_PropertyChanged;
			ViewModelLocator.Messenger.Unregister<DialogMessage>(this);
			ViewModelLocator.Messenger.Unregister<InputDialogMessage>(this);
		}
	}
}
