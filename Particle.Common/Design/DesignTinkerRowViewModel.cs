using Particle.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace Particle.Common.Design
{
	public class DesignTinkerRowViewModel : ITinkerRowViewModel
	{
		public IPinViewModel Left
		{
			get;
			set;
		}

		public IPinViewModel Right
		{
			get;
			set;
		}

		public event PropertyChangedEventHandler PropertyChanged;
	}
}
