using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
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
			}
			else
			{
				SimpleIoc.Default.Register<ILoginViewModel, LoginViewModel>();
				SimpleIoc.Default.Register<IRegisterViewModel, RegisterViewModel>();
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

		public static ParticleCloud Cloud
		{
			get
			{
				return cloud;
			}
		}
	}
}
