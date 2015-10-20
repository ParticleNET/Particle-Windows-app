﻿/*
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
using Particle.Common.Models;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;
using Particle.Common.Messages;

namespace Particle.Common.ViewModel
{
	public class DevicesListViewModel : ViewModelBase, IDevicesListViewModel
	{
		private ObservableCollection<ParticleDeviceWrapper> devices;
		private bool isRefreshing;
		public DevicesListViewModel()
		{
			devices = new ObservableCollection<ParticleDeviceWrapper>();
			ViewModelLocator.Messenger.Register<Messages.RefreshDevicesMessage>(this, refreshDevices);
			ViewModelLocator.Messenger.Register<Messages.LoggedInMessage>(this, (e)=>
			{
				RefreshCommand.Execute(null);
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
					var current = devices.FirstOrDefault(i => String.Compare(i.Device.Id, d.Id) == 0);
					if (current == null)
					{
						ParticleCloud.SyncContext.InvokeIfRequired(() =>
						{
							devices.Add(new ParticleDeviceWrapper(d));
						});
						if (d.Connected)
						{
							tasks.Add(d.RefreshAsync());
						}
					}
					else
					{
						tasks.Add(current.Device.RefreshAsync());
					}
				}
				IsRefreshing = false;
				await Task.WhenAll(tasks);
			}
			else
			{
				de.SendDialogMessage();
			}
			IsRefreshing = false;
		}

		/// <summary>
		/// Gets the devices.
		/// </summary>
		/// <value>
		/// The devices.
		/// </value>
		public ObservableCollection<ParticleDeviceWrapper> Devices
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

		private void setSelectedDevice(ParticleDeviceWrapper value)
		{
			selectedDevice = value;
			RaisePropertyChanged(nameof(SelectedDevice));
			ViewModelLocator.Messenger.Send(new Messages.SelectedDeviceMessage
			{
				Device = value
			});
		}

		private ParticleDeviceWrapper selectedDevice;
		public ParticleDeviceWrapper SelectedDevice
		{
			get
			{
				return selectedDevice;
			}
			set
			{
				if (selectedDevice != value)
				{
					if (!value.Device.Connected)
					{
						DialogMessage dm = new DialogMessage();
						dm.Title = MM.M.GetString("DeviceOfflineTitle");
						dm.Description = MM.M.GetString("DeviceOffline");
						dm.Buttons = new List<MessageButtonModel>
						{
							new MessageButtonModel
							{
								Id = 1,
								Text = MM.M.GetString("DeviceOfflineButton")
							}
						};
						dm.CallBack = ((m) =>
						{
							selectedDevice = null;
							setSelectedDevice(null);
						});
						ViewModelLocator.Messenger.Send(dm);
						return;
					}
					else if(!value.HasTinker)
					{
						DialogMessage dm = new DialogMessage();
						dm.Title = MM.M.GetString("DeviceNotRunningTinkerTitle");
						dm.Description = MM.M.GetString("DeviceNotRunningTinker");
						dm.Buttons = new List<MessageButtonModel>
						{
							new MessageButtonModel
							{
								Id=1,
								Text = MM.M.GetString("DeviceReflashButton")
							},
							new MessageButtonModel
							{
								Id=2,
								Text = MM.M.GetString("DeviceCancelButton")
							},
							new MessageButtonModel
							{
								Id=3,
								Text = MM.M.GetString("DeviceTinkerAnywaysButton")
							}
						};
						dm.CallBack = (option) =>
						{
							switch (option)
							{
								case 1:
									break;
								case 2:
									setSelectedDevice(null);
									break;
								case 3:
								default:
									setSelectedDevice(value);
									break;
							}
						};
						ViewModelLocator.Messenger.Send(dm);
					}
					else
					{
						setSelectedDevice(value);
					}
				}
			}
		}

		private ICommand refreshCommand;

		public ICommand RefreshCommand
		{
			get
			{
				return refreshCommand ?? (refreshCommand = new RelayCommand(() =>
				{
					ViewModelLocator.Messenger.Send(new Messages.RefreshDevicesMessage());
				}));
			}
		}
	}
}
