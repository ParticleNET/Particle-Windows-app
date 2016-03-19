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
using GalaSoft.MvvmLight.Command;
using ParticleApp.Business.Interfaces;
using System.Windows.Input;

namespace ParticleApp.Business.Design
{
	public class DesignCommandsViewModel : ICommandsViewModel
	{
		/// <summary>
		/// Gets the license command.
		/// </summary>
		/// <value>
		/// The license command.
		/// </value>
		public ICommand LicenseCommand
		{
			get
			{
				return new RelayCommand(() => { });
			}
		}

		/// <summary>
		/// Gets the particle store command.
		/// </summary>
		/// <value>
		/// The particle store command.
		/// </value>
		public ICommand ParticleStoreCommand
		{
			get
			{
				return new RelayCommand(() => { });
			}
		}

		/// <summary>
		/// Gets the report bug command.
		/// </summary>
		/// <value>
		/// The report bug command.
		/// </value>
		public ICommand ReportBugCommand
		{
			get
			{
				return new RelayCommand(() => { });
			}
		}
	}
}
