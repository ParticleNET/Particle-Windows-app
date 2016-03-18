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
using Newtonsoft.Json.Linq;
using ParticleApp.Business.Interfaces;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;

namespace ParticleApp.Business.Design
{
	public class DesignDevicesListViewModel : IDevicesListViewModel
	{
		public ICommand AddDeviceCommand
		{
			get
			{
				return new RelayCommand(() => { });
			}
		}

		public ICommand AddElectronCommand
		{
			get
			{
				return new RelayCommand(() => { });
			}
		}

		public ObservableCollection<IDeviceWrapper> Devices
		{
			get;
		} = new ObservableCollection<IDeviceWrapper>()
		{
			new IDeviceWrapper(new DesignParticleDevice(JObject.Parse(
@"{
name: 'Test1',
id: 'abc123',
connected: false,
product_id: 0
}"))),
			new IDeviceWrapper(new DesignParticleDevice(JObject.Parse(
@"{
name: 'Test2',
id: 'def456',
connected: true,
product_id: 6
}"))),
			new ParticleDeviceWrapper(new DesignParticleDevice(JObject.Parse(
@"{
name: 'Test3',
id: 'ghi789',
connected: true,
product_id: 0
}")))
		};

		public bool IsRefreshing
		{
			get
			{
				return false;
			}
		}

		public ICommand RefreshCommand
		{
			get
			{
				return new RelayCommand(() => { });
			}
		}

		public string ResumeDeviceId
		{
			get;

			set;
		}

		public ParticleDeviceWrapper SelectedDevice
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
