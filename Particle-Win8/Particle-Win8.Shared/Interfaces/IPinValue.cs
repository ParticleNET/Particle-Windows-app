﻿/*
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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Particle.Common.Interfaces
{
	/// <summary>
	/// Used to pass both Value and Mode to the PinValueConverter
	/// </summary>
	public interface IPinValue
	{
		/// <summary>
		/// Gets the value.
		/// </summary>
		/// <value>
		/// The value.
		/// </value>
		short Value { get; }
		/// <summary>
		/// Gets the pin mode.
		/// </summary>
		/// <value>
		/// The mode.
		/// </value>
		PinMode Mode { get; }
	}
}
