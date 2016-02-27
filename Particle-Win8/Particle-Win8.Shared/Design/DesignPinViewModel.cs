using Particle.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Particle.Common.Models;
using System.ComponentModel;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;
using Windows.UI.Xaml.Input;

namespace Particle.Common.Design
{
	public class DesignPinViewModel : IPinViewModel
	{
		private ICommand cmd = new RelayCommand(() =>
		{

		});
		public ICommand AnalogRead
		{
			get
			{
				return cmd;
			}
		}

		public ICommand AnalogWrite
		{
			get
			{
				return cmd;
			}
		}

		public ParticleDeviceWrapper Device
		{
			get;

			set;
		}

		public ICommand DigitalRead
		{
			get
			{
				return cmd;
			}
		}

		public ICommand DigitalWrite
		{
			get
			{
				return cmd;
			}
		}

		public ICommand AnalogManipulationComplete
		{
			get
			{
				return cmd;
			}
		}

		public ICommand Holding
		{
			get
			{
				return cmd;
			}
		}

		public bool IsRight
		{
			get;
			set;
		}

		public PinMode Mode
		{
			get;
			set;
		}

		public string PinDisplayName
		{
			get;

			set;
		}

		public string PinId
		{
			get;

			set;
		}

		public ICommand Refresh
		{
			get
			{
				return cmd;
			}
		}

		public bool ShowSelect
		{
			get;
			set;
		}

		public PinMode SupportedModes
		{
			get;

			set;
		}

		public short Value
		{
			get;

			set;
		}

		public IPinValue PinValue
		{
			get
			{
				return new PinValueModel
				{
					Value = Value,
					Mode = Mode
				};
			}
		}

		//This is a design class so ignore the Property not being used warning
#pragma warning disable CS0067
		public event PropertyChangedEventHandler PropertyChanged;
#pragma warning restore CS0067
	}
}
