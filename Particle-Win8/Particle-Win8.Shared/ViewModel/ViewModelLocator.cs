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
using Particle.Common.Interfaces;
using Particle.Common.Messages;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Particle.Common.ViewModel
{
	public class ViewModelLocator
	{
		private static ParticleCloud cloud;

		static ViewModelLocator()
		{
			ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);
			if (ViewModelBase.IsInDesignModeStatic)
			{
				SimpleIoc.Default.Register<ILoginViewModel, Design.DesignLoginViewModel>();
				SimpleIoc.Default.Register<IRegisterViewModel, Design.DesignRegisterViewModel>();
				SimpleIoc.Default.Register<IDevicesListViewModel, Design.DesignDevicesListViewModel>();
				SimpleIoc.Default.Register<ITinkerViewModel, Design.DesignTinkerViewModel>();
				SimpleIoc.Default.Register<ILogoutViewModel, Design.DesignLogoutViewModel>();
				SimpleIoc.Default.Register<ICommandsViewModel, Design.DesignCommandsViewModel>();
			}
			else
			{
				SimpleIoc.Default.Register<ILoginViewModel, LoginViewModel>();
				SimpleIoc.Default.Register<IRegisterViewModel, RegisterViewModel>();
				SimpleIoc.Default.Register<IDevicesListViewModel, DevicesListViewModel>();
				SimpleIoc.Default.Register<ITinkerViewModel, TinkerViewModel>();
				SimpleIoc.Default.Register<ILogoutViewModel, LogoutViewModel>();
				SimpleIoc.Default.Register<ICommandsViewModel, CommandsViewModel>();
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
				Task.Run(() => yourEvents.Start()); // With out the Task.Run the app freezes up not sure why
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

		public static ICommandsViewModel CommandViewModel
		{
			get
			{
				return SimpleIoc.Default.GetInstance<ICommandsViewModel>();
			}
		}
		

		public static ILoginViewModel LoginViewModel
		{
			get
			{
				return SimpleIoc.Default.GetInstance<ILoginViewModel>();
			}
		}

		public static IRegisterViewModel RegisterViewModel
		{
			get
			{
				return SimpleIoc.Default.GetInstance<IRegisterViewModel>();
			}
		}

		public static IDevicesListViewModel DevicesListViewModel
		{
			get
			{
				return SimpleIoc.Default.GetInstance<IDevicesListViewModel>();
			}
		}

		public static ILogoutViewModel LogoutViewModel
		{
			get
			{
				return SimpleIoc.Default.GetInstance<ILogoutViewModel>();
			}
		}

		public static ITinkerViewModel TinkerViewModel
		{
			get
			{
				return SimpleIoc.Default.GetInstance<ITinkerViewModel>();
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
