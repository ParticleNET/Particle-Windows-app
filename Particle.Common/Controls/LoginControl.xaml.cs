/*
   Copyright 2015 Sannel Software, L.L.C.

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

using System.ComponentModel;
using Particle.Common.ViewModel;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Particle.Common.Interfaces;
using GalaSoft.MvvmLight.Ioc;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace Particle.Common.Controls
{
	public sealed partial class LoginControl : UserControl, System.ComponentModel.INotifyPropertyChanged
	{
		private ILoginViewModel _viewModel;

		private ILoginViewModel viewModel
		{
			get
			{
				return _viewModel ?? (_viewModel = (ILoginViewModel)DataContext);
			}
		}

		public LoginControl()
		{
			this.InitializeComponent();
			viewModel.PropertyChanged += viewModel_PropertyChanged;
			viewModel.Load();
		}

		private void viewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			if (string.Compare(e.PropertyName, nameof(IsProcessing)) == 0)
			{
				if (PropertyChanged != null)
				{
					lock (PropertyChanged)
					{
						PropertyChanged(this, new PropertyChangedEventArgs(nameof(IsProcessing)));
					}
				}
			}
		}

		public event PropertyChangedEventHandler PropertyChanged;

		public bool IsProcessing
		{
			get
			{
				return viewModel.IsProcessing;
			}
		}
	}
}