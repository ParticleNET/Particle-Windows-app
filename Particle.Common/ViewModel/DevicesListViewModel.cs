/*
   Copyright 2015 ParticleNET

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
using GalaSoft.MvvmLight;
using Particle.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using Particle;

namespace Particle.Common.ViewModel
{
	public class DevicesListViewModel : ViewModelBase, IDevicesListViewModel
	{
		private ObservableCollection<ParticleDevice> devices;
		private bool isRefreshing;
		public DevicesListViewModel()
		{
			devices = new ObservableCollection<ParticleDevice>();
			ViewModelLocator.Messenger.Register<Messages.RefreshDevicesMessage>(this, refreshDevices);
			ViewModelLocator.Messenger.Register<Messages.LoggedInMessage>(this, (e)=>
			{
				ViewModelLocator.Messenger.Send(new Messages.RefreshDevicesMessage());
			});
		}

		private async void refreshDevices(Messages.RefreshDevicesMessage message)
		{
			IsRefreshing = true;
			var de = await ViewModelLocator.Cloud.GetDevicesAsync();
			if (de.Success)
			{
				List<Task> tasks = new List<Task>();
				foreach (var d in de.Data)
				{
					ParticleCloud.SyncContext.InvokeIfRequired(() =>
					{
						devices.Add(d);
					});
					if (d.Connected)
					{
						tasks.Add(d.RefreshAsync());
					}
				}
				IsRefreshing = false;
				await Task.WhenAll(tasks);
			}
			IsRefreshing = false;
		}

		/// <summary>
		/// Gets the devices.
		/// </summary>
		/// <value>
		/// The devices.
		/// </value>
		public ObservableCollection<ParticleDevice> Devices
		{
			get
			{
				return devices;
			}
		}
		/// <summary>
		/// Gets a value indicating whether the devices are refreshing
		/// </summary>
		/// <value>
		/// <c>true</c> if the devices are refreshing; otherwise, <c>false</c>.
		/// </value>
		public bool IsRefreshing
		{
			get
			{
				return isRefreshing;
			}
			private set
			{
				Set(nameof(IsRefreshing), ref isRefreshing, value);
			}
		}

		private ParticleDevice selectedDevice;
		public ParticleDevice SelectedDevice
		{
			get
			{
				return selectedDevice;
			}
			set
			{
				if (Set(nameof(SelectedDevice), ref selectedDevice, value))
				{
					ViewModelLocator.Messenger.Send(new Messages.SelectedDeviceMessage
					{
						Device = value
					});
				}
			}
		}
	}
}
