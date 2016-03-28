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
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Messaging;
using Microsoft.Practices.ServiceLocation;
using Particle;
using ParticleApp.Business.Interfaces;
using ParticleApp.Business.Messages;
using ParticleApp.Business.Models;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace ParticleApp.Business.ViewModel
{
	public class ViewModelLocator
	{
		private static ParticleCloud cloud;

		static ViewModelLocator()
		{
			ServiceLocator.SetLocatorProvider(() => Ioc.Default);
			if (ViewModelBase.IsInDesignModeStatic)
			{
				Ioc.Default.Register<ILoginViewModel, Design.DesignLoginViewModel>();
				Ioc.Default.Register<IRegisterViewModel, Design.DesignRegisterViewModel>();
				Ioc.Default.Register<IDevicesListViewModel, Design.DesignDevicesListViewModel>();
				Ioc.Default.Register<ITinkerViewModel, Design.DesignTinkerViewModel>();
				Ioc.Default.Register<ILogoutViewModel, Design.DesignLogoutViewModel>();
				Ioc.Default.Register<ICommandsViewModel, Design.DesignCommandsViewModel>();
				Ioc.Default.Register<IDeviceWrapper, Design.DesignDeviceWrapper>();
			}
			else
			{
				Ioc.Default.Register<ILoginViewModel, LoginViewModel>();
				Ioc.Default.Register<IRegisterViewModel, RegisterViewModel>();
				Ioc.Default.Register<IDevicesListViewModel, DevicesListViewModel>();
				Ioc.Default.Register<ITinkerViewModel, TinkerViewModel>();
				Ioc.Default.Register<ILogoutViewModel, LogoutViewModel>();
				Ioc.Default.Register<ICommandsViewModel, CommandsViewModel>();
				Ioc.Default.Register<IDeviceWrapper, DeviceWrapper>();
			}
			cloud = new ParticleCloud();
			Messenger.Register<LoggedInMessage>(cloud, loggedIn);
			Messenger.Register<LoggedOutMessage>(cloud, loggedOut);
		}

		private static ParticleEventManager yourEvents;

		private static void loggedIn(LoggedInMessage mes)
		{
			if(yourEvents != null)
			{
				loggedOut(new LoggedOutMessage()); // loggedOut will clean up the previous instance
			}
			if(cloud.IsAuthenticated)
			{
				var accessToken = cloud.AccessToken;
				UriBuilder builder = new UriBuilder(cloud.YourEventUri);
				builder.Path = $"{builder.Path}/spark";
				yourEvents = new ParticleEventManager(builder.Uri, accessToken);
				yourEvents.Events += YourEvents_Events;
#if DEBUG
				yourEvents.Error += (a) =>
				{
					Debug.WriteLine($"Error: {a}");
				};
#endif
				yourEvents.Start();
			}
		}

		private static void YourEvents_Events(object sender, WebEventArgs e)
		{
			Messenger.Send(new YourWebEventMessage(e));
#if DEBUG
			if (e.Data != null && e.Data.Length > 0)
			{
				Debug.WriteLine($"{e.Event} at {DateTime.Now} data {e.Data[0]}");
			}
#endif
		}

		private static void loggedOut(LoggedOutMessage mes)
		{
			var y = yourEvents;
			yourEvents = null;
			if (y != null)
			{
				y.Events -= YourEvents_Events; // clean up the listener
				y.Stop();
				y = null;
			}
		}

		public static IDeviceWrapper CrateDeviceWrapper(ParticleDevice device)
		{
			return Ioc.Default.GetInstanceWithoutCaching<IDeviceWrapper>(device);
		}

		public static ICommandsViewModel CommandViewModel
		{
			get
			{
				return Ioc.Default.GetInstance<ICommandsViewModel>();
			}
		}
		

		public static ILoginViewModel LoginViewModel
		{
			get
			{
				return Ioc.Default.GetInstance<ILoginViewModel>();
			}
		}

		public static IRegisterViewModel RegisterViewModel
		{
			get
			{
				return Ioc.Default.GetInstance<IRegisterViewModel>();
			}
		}

		public static IDevicesListViewModel DevicesListViewModel
		{
			get
			{
				return Ioc.Default.GetInstance<IDevicesListViewModel>();
			}
		}

		public static ILogoutViewModel LogoutViewModel
		{
			get
			{
				return Ioc.Default.GetInstance<ILogoutViewModel>();
			}
		}

		public static ITinkerViewModel TinkerViewModel
		{
			get
			{
				return Ioc.Default.GetInstance<ITinkerViewModel>();
			}
		}

		public static ParticleCloud Cloud
		{
			get
			{
				return cloud;
			}
		}

		public static IMessenger Messenger
		{
			get
			{
				return GalaSoft.MvvmLight.Messaging.Messenger.Default;
			}
		}

		public static void Suspending()
		{
			AppSettings.Current.StoredDeviceId = DevicesListViewModel.SelectedDevice?.Device?.Id;
		}

		public static void Resuming()
		{
			DevicesListViewModel.ResumeDeviceId = AppSettings.Current.StoredDeviceId;
			AppSettings.Current.StoredDeviceId = null;
		}

		public static bool SupportsClipboard { get; set; }
	}
}
