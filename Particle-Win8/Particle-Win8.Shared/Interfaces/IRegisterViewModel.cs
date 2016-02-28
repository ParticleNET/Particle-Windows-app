using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Particle.Common.Interfaces
{
	public interface IRegisterViewModel : ILoginViewModel
	{
		/// <summary>
		/// The verify password for checking against
		/// </summary>
		/// <value>
		/// The verify password.
		/// </value>
		String VerifyPassword { get; set; }
	}
}
