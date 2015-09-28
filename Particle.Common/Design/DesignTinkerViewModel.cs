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

		private List<ITinkerRowViewModel> pinRows;

		public IEnumerable<ITinkerRowViewModel> PinRows
		{
			get
			{
				return pinRows ?? (pinRows = new List<ITinkerRowViewModel>
				{
					new DesignTinkerRowViewModel
					{
						Left = new DesignPinViewModel
						{
							Device = Device,
							Mode = PinMode.AnalogWrite,
							PinDisplayName = "A7",
							Value = 122
						},
						Right = new DesignPinViewModel
						{
							Device = Device,
							PinDisplayName = "D7",
							Mode = PinMode.DigitalWrite,
							Value = 1,
							IsRight = true
						}
					},
					new DesignTinkerRowViewModel
					{
						Left = new DesignPinViewModel
						{
							Device = Device,
							Mode = PinMode.AnalogRead,
							PinDisplayName = "A6",
							Value = 1024
						},
						Right = new DesignPinViewModel
						{
							Device = device,
							Mode = PinMode.DigitalRead,
							PinDisplayName = "D6",
							Value = 1,
							IsRight = true
						}
					},
					new DesignTinkerRowViewModel
					{
						Left = new DesignPinViewModel
						{
							Device = Device,
							Mode = PinMode.Unknown,
							PinDisplayName = "A5"
						},
						Right = new DesignPinViewModel
						{
							Device = Device,
							Mode = PinMode.Unknown,
							PinDisplayName = "D5",
							IsRight = true
						}
					},
					new DesignTinkerRowViewModel
					{
						Left = new DesignPinViewModel
						{
							Device = Device,
							Mode = PinMode.Unknown,
							PinDisplayName = "A4"
						},
						Right = new DesignPinViewModel
						{
							Device = Device,
							Mode = PinMode.Unknown,
							PinDisplayName = "D4",
							IsRight = true
						}
					},
					new DesignTinkerRowViewModel
					{
						Left = new DesignPinViewModel
						{
							Device = Device,
							Mode = PinMode.Unknown,
							PinDisplayName = "A3"
						},
						Right = new DesignPinViewModel
						{
							Device = Device,
							Mode = PinMode.Unknown,
							PinDisplayName = "D3",
							IsRight = true
						}
					},
					new DesignTinkerRowViewModel
					{
						Left = new DesignPinViewModel
						{
							Device = Device,
							Mode = PinMode.Unknown,
							PinDisplayName = "A2"
						},
						Right = new DesignPinViewModel
						{
							Device = Device,
							Mode = PinMode.Unknown,
							PinDisplayName = "D2",
							IsRight = true
						}
					},
					new DesignTinkerRowViewModel
					{
						Left = new DesignPinViewModel
						{
							Device = Device,
							Mode = PinMode.Unknown,
							PinDisplayName = "A1"
						},
						Right = new DesignPinViewModel
						{
							Device = Device,
							Mode = PinMode.Unknown,
							PinDisplayName = "D1",
							IsRight = true
						}
					},
					new DesignTinkerRowViewModel
					{
						Left = new DesignPinViewModel
						{
							Device = Device,
							Mode = PinMode.Unknown,
							PinDisplayName = "A0"
						},
						Right = new DesignPinViewModel
						{
							Device = Device,
							Mode = PinMode.Unknown,
							PinDisplayName = "D0",
							IsRight = true
						}
					},
				});
			}
		}

		public event PropertyChangedEventHandler PropertyChanged;
	}
}
