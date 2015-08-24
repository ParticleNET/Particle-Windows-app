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
		static ViewModelLocator()
		{
			ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);
			if (ViewModelBase.IsInDesignModeStatic)
			{
				SimpleIoc.Default.Register<ILoginViewModel, Design.DesignLoginViewModel>();
			}
			else
			{
				SimpleIoc.Default.Register<ILoginViewModel, LoginViewModel>();
			}
			//SimpleIoc.Default.Register<MainViewModel>();
		}

		public static ILoginViewModel LoginViewModel
		{
			get
			{
				return SimpleIoc.Default.GetInstance<ILoginViewModel>();
			}
		}
	}
}
