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
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Particle;
using ParticleApp.Business.Interfaces;
using ParticleApp.Business.Messages;
using ParticleApp.Business.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using WinRTXamlToolkit.Async;

namespace ParticleApp.Business.ViewModel
{
	public class DevicesListViewModel : ViewModelBase, IDevicesListViewModel
	{
		private ObservableCollection<IDeviceWrapper> devices;
		private bool isRefreshing;
		public DevicesListViewModel()
		{
			devices = new ObservableCollection<IDeviceWrapper>();
			ViewModelLocator.Messenger.Register<Messages.RefreshDevicesMessage>(this, refreshDevices);
			ViewModelLocator.Messenger.Register<Messages.LoggedInMessage>(this, (e)=>
			{
				RefreshCommand.Execute(null);
			});
			ViewModelLocator.Messenger.Register<Messages.LoggedOutMessage>(this, (e) =>
			{
				ParticleCloud.SyncContext.InvokeIfRequired(() =>
				{
					Devices.Clear();
				});
			});
		}

		private String resumeDeviceId;

		public String ResumeDeviceId
		{
			get
			{
				return resumeDeviceId;
			}
			set
			{
				Set(nameof(ResumeDeviceId), ref resumeDeviceId, value);
			}
		}

		private readonly static AsyncLock refreshLocker = new AsyncLock();

		private async void refreshDevices(Messages.RefreshDevicesMessage message)
		{
			using (var loc = await refreshLocker.LockAsync())
			{
				IsRefreshing = true;
				var de = await ViewModelLocator.Cloud.GetDevicesAsync();
				if (de.Success)
				{
					List<Task> tasks = new List<Task>();
					var dict = new Dictionary<String, bool>();
					foreach (var dd in devices)
					{
						dict[dd.Device.Id] = false;
					}

					foreach (var d in de.Data)
					{
						if (d?.Id == null)
						{
							continue; // Some how we got a device without an id so go to next device.
						}
						var current = devices.FirstOrDefault(i => String.Compare(i.Device.Id, d.Id) == 0);
						if (current == null)
						{
							ParticleCloud.SyncContext.InvokeIfRequired(() =>
							{
								devices.Add(ViewModelLocator.CrateDeviceWrapper(d));
							});
							if (d.Connected)
							{
								tasks.Add(d.RefreshAsync());
							}
						}
						else
						{
							dict[current.Device.Id] = true;
							tasks.Add(current.Device.RefreshAsync());
						}
					}

					foreach (var toRemove in dict.Where(i => i.Value == false))
					{
						var item = devices.FirstOrDefault(i => String.Compare(i.Device.Id, toRemove.Key) == 0);
						if (item != null)
						{
							ParticleCloud.SyncContext.InvokeIfRequired(() =>
							{
								devices.Remove(item);
							});
						}
					}

					IsRefreshing = false;
					await Task.WhenAll(tasks);
					if (!String.IsNullOrWhiteSpace(ResumeDeviceId))
					{
						var dev = devices.FirstOrDefault(i => i.Device?.Id == ResumeDeviceId);
						if (dev != null)
						{
							setSelectedDevice(dev);
						}
					}
				}
				else
				{
					de.SendDialogMessage();
				}
				IsRefreshing = false;
			}
		}

		/// <summary>
		/// Gets the devices.
		/// </summary>
		/// <value>
		/// The devices.
		/// </value>
		public ObservableCollection<IDeviceWrapper> Devices
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

		private void setSelectedDevice(IDeviceWrapper value)
		{
			selectedDevice = value;
			RaisePropertyChanged(nameof(SelectedDevice));
			ViewModelLocator.Messenger.Send(new Messages.SelectedDeviceMessage
			{
				Device = value
			});
		}

		private IDeviceWrapper selectedDevice;
		public IDeviceWrapper SelectedDevice
		{
			get
			{
				return selectedDevice;
			}
			set
			{
				if (selectedDevice != value)
				{
					if(value == null)
					{
						setSelectedDevice(null);
						return;
					}
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
									if (value.FlashTinkerCommand.CanExecute(true))
									{
										value.FlashTinkerCommand.Execute(true);
									}
									setSelectedDevice(null);
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
		public ICommand LogoutCommand
		{
			get
			{
				return ViewModelLocator.LogoutViewModel.Logout;
			}
		}

		private async void addDeviceReceived(String button, String text)
		{
			if(String.Compare(button, MM.M.GetString("Add_Device_Button")) == 0)
			{
				if (String.IsNullOrWhiteSpace(text))
				{
					ViewModelLocator.Messenger.Send(new DialogMessage()
					{
						Description = MM.M.GetString("Invalid_Device_Id")
					});
				}
				else
				{
					var result = await ViewModelLocator.Cloud.ClaimDeviceAsync(text);
					if (result.Success)
					{
						var idm = new InputDialogMessage();
						idm.Title = MM.M.GetString("NameDevice_Dialog_Title");
						idm.Description = MM.M.GetString("NameDevice_Dialog_Description");
						idm.Buttons = new String[]
						{
							MM.M.GetString("NameDevice_Dialog_Button")
						};

						idm.CallBack = async (act, resp) =>
						{
							if (String.IsNullOrWhiteSpace(resp))
							{
								ViewModelLocator.Messenger.Send(new DialogMessage
								{
									Description = MM.M.GetString("NameDevice_Dialog_Error"),
									CallBack = (i)=>
									{
										ViewModelLocator.Messenger.Send(idm);
									}
								});
							}
							else
							{
								var devices = await ViewModelLocator.Cloud.GetDevicesAsync();
								if(devices.Success)
								{
									var d = devices?.Data.FirstOrDefault(i => String.Compare(i.Id, text) == 0);
									if(d != null)
									{
										await d.RenameAsync(resp);
									}
								}
								if (RefreshCommand.CanExecute(null))
								{
									RefreshCommand.Execute(null);
								}
							}
						};
						ViewModelLocator.Messenger.Send(idm);
						
					}
					else
					{
						ViewModelLocator.Messenger.Send(new DialogMessage()
						{
							Description = result.Error
						});
					}
				}
			}
		}

		private ICommand addDeviceCommand;
		public ICommand AddDeviceCommand
		{
			get
			{
				return addDeviceCommand ?? (addDeviceCommand = new RelayCommand(() =>
				{
					ViewModelLocator.Messenger.Send(new InputDialogMessage()
					{
						Title = MM.M.GetString("Add_Device_Title"),
						Description = MM.M.GetString("Add_Device_Description"),
						Buttons = new String[]
						{
							MM.M.GetString("Add_Device_Button"),
							MM.M.GetString("Cancel_Button")
						},
						CallBack = addDeviceReceived
					});
				}));
			}
		}

		private ICommand addElectronCommand;
		public ICommand AddElectronCommand
		{
			get
			{
				return addElectronCommand ?? (addElectronCommand = new RelayCommand(async () =>
				{
					await Windows.System.Launcher.LaunchUriAsync(new Uri("https://setup.particle.io/"));
				}));
			}
		}
	}
}
