using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Particle.Common.Interfaces
{
	/// <summary>
	/// Represents a row 
	/// </summary>
	public interface ITinkerRowViewModel : INotifyPropertyChanged
	{
		/// <summary>
		/// Gets the left pin
		/// </summary>
		/// <value>
		/// The left pin
		/// </value>
		IPinViewModel Left { get; }
		/// <summary>
		/// Gets the right pin
		/// </summary>
		/// <value>
		/// The right pin
		/// </value>
		IPinViewModel Right { get; }
	}
}
