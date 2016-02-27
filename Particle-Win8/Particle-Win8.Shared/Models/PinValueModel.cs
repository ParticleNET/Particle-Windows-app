using Particle.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Particle.Common.Models
{
	public class PinValueModel : IPinValue
	{
		public PinMode Mode
		{
			get;
			set;
		}

		public short Value
		{
			get;
			set;
		}
	}
}
