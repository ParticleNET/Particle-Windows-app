using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Messaging;
using Microsoft.Practices.ServiceLocation;
using Particle.Common.Interfaces;
using System;
using System.Collections.Generic;
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
			}
			else
			{
				SimpleIoc.Default.Register<ILoginViewModel, LoginViewModel>();
				SimpleIoc.Default.Register<IRegisterViewModel, RegisterViewModel>();
				SimpleIoc.Default.Register<IDevicesListViewModel, DevicesListViewModel>();
			}
			cloud = new ParticleCloud();
			//SimpleIoc.Default.Register<MainViewModel>();
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
	}
}
