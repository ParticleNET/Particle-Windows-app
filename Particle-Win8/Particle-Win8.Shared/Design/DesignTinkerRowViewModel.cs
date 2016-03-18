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
using ParticleApp.Business.Interfaces;
using System.ComponentModel;

namespace ParticleApp.Business.Design
{
	/// <summary>
	/// 
	/// </summary>
	/// <seealso cref="ParticleApp.Business.Interfaces.ITinkerRowViewModel" />
	public class DesignTinkerRowViewModel : ITinkerRowViewModel
	{
		/// <summary>
		/// Gets the left pin
		/// </summary>
		/// <value>
		/// The left pin
		/// </value>
		public IPinViewModel Left
		{
			get;
			set;
		}

		/// <summary>
		/// Gets the right pin
		/// </summary>
		/// <value>
		/// The right pin
		/// </value>
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
