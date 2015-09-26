using Particle.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Particle.Common.Models;
using System.ComponentModel;
using Newtonsoft.Json.Linq;

namespace Particle.Common.Design
{
	public class DesignTinkerViewModel : ITinkerViewModel
	{
		private ParticleDeviceWrapper device;
		public ParticleDeviceWrapper Device
		{
			get
			{
				return device ?? (device = new ParticleDeviceWrapper(new DesignParticleDevice(JObject.Parse(
@"{
name: 'Test1',
id: 'abc123',
connected: false,
product_id: 0
}"))));
			}
		}

		private List<IPinViewModel> leftPins;

		public IEnumerable<IPinViewModel> LeftPins
		{
			get
			{
				return leftPins ?? (leftPins = new List<IPinViewModel>
				{
					new DesignPinViewModel
					{
						Device = Device,
						PinDisplayName = "A7",
						Mode = PinMode.AnalogRead
					},
					new DesignPinViewModel
					{
						Device = Device,
						PinDisplayName = "A6",
						Mode = PinMode.AnalogWrite
					},
					new DesignPinViewModel
					{
						Device = Device,
						PinDisplayName = "A5",
						Mode = PinMode.Unknown
					},
					new DesignPinViewModel
					{
						Device = Device,
						PinDisplayName = "A4",
						Mode = PinMode.Unknown
					},
					new DesignPinViewModel
					{
						Device = Device,
						PinDisplayName = "A3",
						Mode = PinMode.Unknown
					},
					new DesignPinViewModel
					{
						Device = Device,
						PinDisplayName = "A2",
						Mode = PinMode.Unknown
					},
					new DesignPinViewModel
					{
						Device = Device,
						PinDisplayName = "A1",
						Mode = PinMode.Unknown
					},
					new DesignPinViewModel
					{
						Device = Device,
						PinDisplayName = "A0",
						Mode = PinMode.Unknown
					}
				});
			}
		}

		private List<IPinViewModel> rightPins;
		public IEnumerable<IPinViewModel> RightPins
		{
			get
			{
				return rightPins ?? (rightPins = new List<IPinViewModel>
				{
					new DesignPinViewModel
					{
						Device = Device,
						PinDisplayName = "D7",
						Mode = PinMode.DigitalRead,
						Value = 0
					},
					new DesignPinViewModel
					{
						Device = Device,
						PinDisplayName = "D6",
						Mode = PinMode.DigitalWrite,
						Value = 1
					},
					new DesignPinViewModel
					{
						Device = Device,
						PinDisplayName = "D5",
						Mode = PinMode.Unknown
					},
					new DesignPinViewModel
					{
						Device = Device,
						PinDisplayName = "D4",
						Mode = PinMode.Unknown
					},
					new DesignPinViewModel
					{
						Device = Device,
						PinDisplayName = "D3",
						Mode = PinMode.Unknown
					},
					new DesignPinViewModel
					{
						Device = Device,
						PinDisplayName = "D2",
						Mode = PinMode.Unknown
					},
					new DesignPinViewModel
					{
						Device = Device,
						PinDisplayName = "D1",
						Mode = PinMode.Unknown
					},
					new DesignPinViewModel
					{
						Device = Device,
						PinDisplayName = "D0",
						Mode = PinMode.Unknown
					}
				});
			}
		}

		public event PropertyChangedEventHandler PropertyChanged;
	}
}
