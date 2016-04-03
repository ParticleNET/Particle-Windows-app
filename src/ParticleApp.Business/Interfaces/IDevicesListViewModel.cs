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
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;

namespace ParticleApp.Business.Interfaces
{
	public interface IDevicesListViewModel : INotifyPropertyChanged
	{
		/// <summary>
		/// Gets a value indicating whether the devices are refreshing
		/// </summary>
		/// <value>
		/// <c>true</c> if the devices are refreshing; otherwise, <c>false</c>.
		/// </value>
		bool IsRefreshing { get; }
		/// <summary>
		/// Gets or sets the resume device identifier.
		/// </summary>
		/// <value>
		/// The resume device identifier.
		/// </value>
		String ResumeDeviceId { get; set; }
		/// <summary>
		/// Gets the devices.
		/// </summary>
		/// <value>
		/// The devices.
		/// </value>
		ObservableCollection<IDeviceWrapper> Devices { get; }
		/// <summary>
		/// Gets or sets the selected device.
		/// </summary>
		/// <value>
		/// The selected device.
		/// </value>
		IDeviceWrapper SelectedDevice { get; set; }
		/// <summary>
		/// The command to call to Refresh the devices
		/// </summary>
		/// <value>
		/// The command to call to Refresh the devices
		/// </value>
		ICommand RefreshCommand { get; }

		/// <summary>
		/// Gets the add device command.
		/// </summary>
		/// <value>
		/// The add device command.
		/// </value>
		ICommand AddDeviceCommand { get; }
		/// <summary>
		/// Gets the add electron command.
		/// </summary>
		/// <value>
		/// The add electron command.
		/// </value>
		ICommand AddElectronCommand { get; }
		/// <summary>
		/// Gets the logout command.
		/// </summary>
		/// <value>
		/// The logout command.
		/// </value>
		ICommand LogoutCommand { get; }
	}
}
