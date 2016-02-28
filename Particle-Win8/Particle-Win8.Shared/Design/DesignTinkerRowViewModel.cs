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

		//This is a design class so ignore the Property not being used warning
#pragma warning disable CS0067
		public event PropertyChangedEventHandler PropertyChanged;
#pragma warning restore CS0067
	}
}
