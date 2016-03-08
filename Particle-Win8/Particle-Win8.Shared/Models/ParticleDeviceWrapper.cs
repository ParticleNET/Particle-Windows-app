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
using GalaSoft.MvvmLight.Command;
using Particle.Common.Messages;
using Particle.Common.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.UI.Xaml;

namespace Particle.Common.Models
{
	public enum DeviceStatus
	{
		Offline,
		Connected,
		Tinker,
		Flashing,
		Failed
	}
	public class ParticleDeviceWrapper : Particle.ParticleBase
	{
		private ParticleDevice device;

		public ParticleDeviceWrapper(ParticleDevice device)
		{
			Device = device;
			device.PropertyChanged += Device_PropertyChanged;
			ViewModelLocator.Messenger.Register<YourWebEventMessage>(this, (a) =>
			{
				var args = a.EventArgs;
				if(args != null)
				{
					if(args.Data != null && args.Data.Length > 0)
					{
						var d1 = args.Data[0];
						if(String.Compare(d1.CoreId, Device?.Id) == 0)
						{
							deviceEvent(args.Event, d1);
						}
					}
				}
			});
		}

		~ParticleDeviceWrapper()
		{
			ViewModelLocator.Messenger.Unregister<YourWebEventMessage>(this);
		}

		private async void deviceEvent(String even, ParticleEventData data)
		{
			if(even.StartsWith("spark/"))
			{
				switch (even)
				{
					case "spark/flash/status":
						switch (data.Data?.Trim())
						{
							case "started":
								Status = DeviceStatus.Flashing;
								break;

							case "failed":
								Status = DeviceStatus.Failed;
								break;
							default:
								break;
						}
						break;

					case "spark/status":
						switch (data.Data?.Trim())
						{
							case "online":
								await Device.RefreshAsync();
								break;
							default:
								break;
						}
						break;

					default:
						break;
				}
			}
		}

		private void Device_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
		{
			if(String.Compare(e.PropertyName, nameof(ParticleDevice.IsRefreshing)) == 0)
			{
				FirePropertyChanged(nameof(IsRefreshing));
				FirePropertyChanged(nameof(HasTinker));
				if (!Device.IsRefreshing)
				{
					if (Device.Connected)
					{
						if (HasTinker)
						{
							Status = DeviceStatus.Tinker;
						}
						else
						{
							Status = DeviceStatus.Connected;
						}
					}
					else
					{
						Status = DeviceStatus.Offline;
					}
				}
				else
				{
					Status = DeviceStatus.Offline;
				}
			}
		}

		public bool IsRefreshing
		{
			get { return Device.IsRefreshing; }
		}


		private DeviceStatus status;

		public DeviceStatus Status
		{
			get { return status; }
			internal set
			{
				SetProperty(ref status, value, nameof(Status));
			}
		}


		public ParticleDevice Device
		{
			get { return device; }
			internal set
			{
				SetProperty(ref device, value, nameof(Device));
			}
		}



		public bool HasTinker
		{
			get
			{
				if(device != null)
				{
					bool hasDigitalRead = false;
					bool hasDigitalWrite = false;
					bool hasAnalogRead = false;
					bool hasAnalogWrite = false;
					foreach(var funcName in device.Functions)
					{
						switch (funcName?.ToLower())
						{
							case "digitalread":
								hasDigitalRead = true;
								break;

							case "digitalwrite":
								hasDigitalWrite = true;
								break;

							case "analogread":
								hasAnalogRead = true;
								break;

							case "analogwrite":
								hasAnalogWrite = true;
								break;
						}
					}

					return hasDigitalWrite && hasDigitalRead && hasAnalogRead && hasAnalogWrite;
				}

				return false;
			}
		}

		private async void renameSuccess(String button, String newName)
		{
			if(String.Compare(button, MM.M.GetString("Rename_Button")) == 0)
			{
				if (String.IsNullOrWhiteSpace(newName))
				{
					GalaSoft.MvvmLight.Messaging.Messenger.Default.Send(new DialogMessage()
					{
						Description = MM.M.GetString("Rename_Device_InvalidNewName")
					});
				}
				else
				{
					var result = await Device.RenameAsync(newName);
					if (result.Success)
					{
						await Device.RefreshAsync();
					}
					else
					{
						GalaSoft.MvvmLight.Messaging.Messenger.Default.Send(new DialogMessage()
						{
							Description = result.Error
						});
					}
				}
			}
		}

		private ICommand renameCommand;
		/// <summary>
		/// Gets the rename command.
		/// </summary>
		/// <value>
		/// The rename command.
		/// </value>
		public ICommand RenameCommand
		{
			get
			{
				return renameCommand ?? (renameCommand = new RelayCommand(() =>
				{
					GalaSoft.MvvmLight.Messaging.Messenger.Default.Send(new InputDialogMessage()
					{
						Title = MM.M.GetString("Rename_Device_Title"),
						Description = MM.M.GetString("Rename_Device_Description"),
						Buttons = new String[]
						{
							MM.M.GetString("Rename_Button"),
							MM.M.GetString("Cancel_Button")
						},
						CallBack = renameSuccess
					});
				}));
			}
		}

		private ICommand copyDeviceIdCommand;
		/// <summary>
		/// Gets the copy device identifier command.
		/// </summary>
		/// <value>
		/// The copy device identifier command.
		/// </value>
		public ICommand CopyDeviceIdCommand
		{
			get
			{
				return copyDeviceIdCommand ?? (copyDeviceIdCommand = new RelayCommand(() =>
				{
					GalaSoft.MvvmLight.Messaging.Messenger.Default.Send(new CopyToClipboardMessage
					{
						Content = Device.Id,
						SuccessMessage = MM.M.GetString("Copied_Device_To_Clipboard")
					});
				}));
			}
		}

		private async void flashTinker()
		{
			Status = DeviceStatus.Flashing;
			switch (Device?.DeviceType)
			{
				case ParticleDeviceType.Electron:
				case ParticleDeviceType.Photon: // From what i see in the android app its better to flash the binary
					var pr = await Device.FlashExampleAppAsync("56214d636666d9ece3000006");
					if(!pr.Success)
					{

					}
					break;

				case ParticleDeviceType.Core: // From what i see in the android app this works for Core only
				default:
					var result = await Device.FlashKnownAppAsync("tinker");
					if (!result.Success)
					{
					}
					break;
			}
			
		}

		private ICommand flashTinkerCommand;
		public ICommand FlashTinkerCommand
		{
			get
			{
				return flashTinkerCommand ?? (flashTinkerCommand = new RelayCommand(() =>
				{
					GalaSoft.MvvmLight.Messaging.Messenger.Default.Send(new DialogMessage()
					{
						Description = MM.M.GetString("Flash_Tinker"),
						Buttons = new List<MessageButtonModel>()
						{
							new MessageButtonModel
							{
								Id = 1,
								Text = MM.M.GetString("Flash")
							},
							new MessageButtonModel
							{
								Id = 2,
								Text = MM.M.GetString("Cancel_Button")
							}
						},
						CallBack = (i) =>
						{
							if(i == 1)
							{
								flashTinker();
							}
						}
					});
				}));
			}
		}

		private ICommand unclaimDeviceCommand;
		public ICommand UnclaimDeviceCommand
		{
			get
			{
				return unclaimDeviceCommand ?? (unclaimDeviceCommand = new RelayCommand(() =>
				{
					ViewModelLocator.Messenger.Send(new DialogMessage
					{
						Title = MM.M.GetString("Unclaim_Device_Title"),
						Description = String.Format(MM.M.GetString("Unclaim_Device_Description"), Device.Name, Device.Id),
						Buttons = new List<MessageButtonModel>()
						{
							new MessageButtonModel
							{
								Id = 1,
								Text = MM.M.GetString("Unclaim_Device_Button")
							},
							new MessageButtonModel
							{
								Id=2,
								Text = MM.M.GetString("Cancel_Button")
							}
						},
						CallBack = async (m) =>
						{
							if(m == 1)
							{
								var result = await Device.UnclaimAsync();
								if(result.Success)
								{
									ViewModelLocator.DevicesListViewModel.SelectedDevice = null;
									var refresh = ViewModelLocator.DevicesListViewModel.RefreshCommand;
									if (refresh.CanExecute(null))
									{
										refresh.Execute(null);
									}
								}
								else
								{
									ViewModelLocator.Messenger.Send(new DialogMessage
									{
										Title = MM.M.GetString("Error_Unclaiming_Device"),
										Description = result.Error
									});
								}
							}
						}
					});
				}));
			}
		}

	}
}
