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
using Particle;
using ParticleApp.Business.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using ParticleApp.Business.Models;
using System.ComponentModel;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;

namespace ParticleApp.Business.Design
{
	/// <summary>
	/// 
	/// </summary>
	public class DesignDeviceWrapper : IDeviceWrapper
	{
		public DesignDeviceWrapper(ParticleDevice device)
		{
			Device = device;
		}

		/// <summary>
		/// Gets the copy device identifier command.
		/// </summary>
		/// <value>
		/// The copy device identifier command.
		/// </value>
		public ICommand CopyDeviceIdCommand
		{
			get
			{
				return new RelayCommand(() => { PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CopyDeviceIdCommand))); });
			}
		}

		/// <summary>
		/// Gets the device.
		/// </summary>
		/// <value>
		/// The device.
		/// </value>
		public ParticleDevice Device
		{
			get;
			private set;
		}

		/// <summary>
		/// Gets the flash tinker command.
		/// </summary>
		/// <value>
		/// The flash tinker command.
		/// </value>
		public ICommand FlashTinkerCommand
		{
			get
			{
				return CopyDeviceIdCommand;
			}
		}

		/// <summary>
		/// Gets a value indicating whether this instance has tinker.
		/// </summary>
		/// <value>
		/// <c>true</c> if this instance has tinker; otherwise, <c>false</c>.
		/// </value>
		public bool HasTinker
		{
			get;
			internal set;
		}

		/// <summary>
		/// Gets a value indicating whether this instance is refreshing.
		/// </summary>
		/// <value>
		/// <c>true</c> if this instance is refreshing; otherwise, <c>false</c>.
		/// </value>
		public bool IsRefreshing
		{
			get;
			internal set;
		}

		/// <summary>
		/// Gets the status.
		/// </summary>
		/// <value>
		/// The status.
		/// </value>
		public DeviceStatus Status
		{
			get;
			internal set;
		}

		/// <summary>
		/// Gets the unclaim device command.
		/// </summary>
		/// <value>
		/// The unclaim device command.
		/// </value>
		public ICommand UnclaimDeviceCommand
		{
			get
			{
				return CopyDeviceIdCommand;
			}
		}

		/// <summary>
		/// Occurs when [property changed].
		/// </summary>
		public event PropertyChangedEventHandler PropertyChanged;
	}
}
