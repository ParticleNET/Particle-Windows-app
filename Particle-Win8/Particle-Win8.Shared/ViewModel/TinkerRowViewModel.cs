using Particle.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using GalaSoft.MvvmLight;

namespace Particle.Common.ViewModel
{
	public class TinkerRowViewModel : ViewModelBase, ITinkerRowViewModel
	{
		private IPinViewModel left;
		public IPinViewModel Left
		{
			get
			{
				return left;
			}
			set
			{
				Set(nameof(Left), ref left, value);
			}
		}

		private IPinViewModel right;
		public IPinViewModel Right
		{
			get
			{
				return right;
			}
			set
			{
				Set(nameof(Right), ref right, value);
			}
		}
	}
}
