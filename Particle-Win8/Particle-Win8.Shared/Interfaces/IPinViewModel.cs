/*
   Copyright 2016 ParticleNET

   Licensed under the Apache License, Version 2.0 (the "License");
   you may not use this file except in compliance with the License.
   You may obtain a copy of the License at

	   http://www.apache.org/licenses/LICENSE-2.0

   Unless required by applicable law or agreed to in writing, software
   distributed under the License is distributed on an "AS IS" BASIS,
   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
   See the License for the specific language governing permissions and
   limitations under the License.
*/
using Particle.Common.Models;
using System;
using System.ComponentModel;
using System.Windows.Input;

namespace ParticleApp.Business.Interfaces
{
	/// <summary>
	/// Represents a pin on the device
	/// </summary>
	public interface IPinViewModel : INotifyPropertyChanged
	{
		/// <summary>
		/// Gets or sets the device this pin is on
		/// </summary>
		/// <value>
		/// The device.
		/// </value>
		IDeviceWrapper Device { get; set; }

		/// <summary>
		/// Gets or sets the pin identifier.
		/// </summary>
		/// <value>
		/// The pin identifier.
		/// </value>
		String PinId { get; set; }
		/// <summary>
		/// Gets or sets the display name of the pin.
		/// </summary>
		/// <value>
		/// The display name of the pin.
		/// </value>
		String PinDisplayName { get; set; }

		/// <summary>
		/// Gets the pin mode.
		/// </summary>
		/// <value>
		/// The mode.
		/// </value>
		PinMode Mode { get; }
		/// <summary>
		/// Gets or sets the supported modes.
		/// </summary>
		/// <value>
		/// The supported modes.
		/// </value>
		PinMode SupportedModes { get; set; }

		/// <summary>
		/// Gets the analog read command
		/// </summary>
		/// <value>
		/// The analog read.
		/// </value>
		ICommand AnalogRead { get; }

		/// <summary>
		/// Gets the analog write command
		/// </summary>
		/// <value>
		/// The analog write.
		/// </value>
		ICommand AnalogWrite { get; }

		/// <summary>
		/// Gets the digital read command
		/// </summary>
		/// <value>
		/// The digital read.
		/// </value>
		ICommand DigitalRead { get; }

		/// <summary>
		/// Gets the digital write command
		/// </summary>
		/// <value>
		/// The digital write.
		/// </value>
		ICommand DigitalWrite { get; }

		/// <summary>
		/// Gets the analog manipulation complete command.
		/// </summary>
		/// <value>
		/// The analog manipulation complete command.
		/// </value>
		ICommand AnalogManipulationComplete { get; }

		/// <summary>
		/// Gets or sets the value.
		/// </summary>
		/// <value>
		/// The value.
		/// </value>
		short Value { get; set; }
		/// <summary>
		/// Gets the pin value.
		/// </summary>
		/// <value>
		/// The pin value.
		/// </value>
		IPinValue PinValue { get; }
		/// <summary>
		/// Gets or sets a value indicating whether this instance is on the right.
		/// </summary>
		/// <value>
		///   <c>true</c> if this instance is on the right; otherwise, <c>false</c>.
		/// </value>
		bool IsRight { get; set; }
		/// <summary>
		/// true if we should show the select mode dialog.
		/// </summary>
		bool ShowSelect { get; set; }

		/// <summary>
		/// Gets the refresh command
		/// Use this command to send the data to the device or grab the value from the device
		/// </summary>
		/// <value>
		/// The refresh.
		/// </value>
		ICommand Refresh { get; }

		ICommand Holding { get; }
	}
}
