using Particle.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Particle.Common.Messages
{
	public class SelectedDeviceMessage
	{
		public ParticleDeviceWrapper Device { get; set; }
	}
}
