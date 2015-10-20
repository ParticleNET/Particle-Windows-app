using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

		private bool isRefreshing;

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

	}
}
