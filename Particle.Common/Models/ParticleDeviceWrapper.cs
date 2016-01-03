using GalaSoft.MvvmLight.Command;
using Particle.Common.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

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

	}
}
