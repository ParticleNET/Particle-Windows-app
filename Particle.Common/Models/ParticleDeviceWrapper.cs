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
		Flashing
	}
	public class ParticleDeviceWrapper : Particle.ParticleBase
	{
		private ParticleDevice device;

		public ParticleDeviceWrapper(ParticleDevice device)
		{
			Device = device;
			device.PropertyChanged += Device_PropertyChanged;
		}

		private void Device_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
		{
			if(String.Compare(e.PropertyName, nameof(ParticleDevice.IsRefreshing)) == 0)
			{
				FirePropertyChanged(nameof(IsRefreshing));
				FirePropertyChanged(nameof(HasTinker));
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
			var result = await Device.FlashKnownAppAsync("tinker");
			if (result.Success)
			{
				DispatcherTimer timer = new DispatcherTimer();
				timer.Interval = TimeSpan.FromMilliseconds(500);
				timer.Tick += async (s, a) =>
				{
					if (HasTinker)
					{
						Status = DeviceStatus.Tinker;
						timer.Stop();
					}
					else
					{
						if (!IsRefreshing)
						{
							var r = await Device.RefreshAsync();
							if (r.Success)
							{
								if (HasTinker)
								{
									Status = DeviceStatus.Tinker;
									timer.Stop();
								}
							}
						}
					}
				};
				timer.Start();
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
