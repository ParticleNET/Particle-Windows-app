using Particle.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Windows.Input;

namespace Particle.Common.Design
{
	public class DesignRegisterViewModel : DesignLoginViewModel, IRegisterViewModel
	{
		public string VerifyPassword
		{
			get;
			set;
		}
	}
}
