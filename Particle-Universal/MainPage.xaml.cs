using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Devices.Enumeration;
using Windows.Devices.SerialCommunication;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage.Streams;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace Particle_Universal
{
	/// <summary>
	/// An empty page that can be used on its own or navigated to within a Frame.
	/// </summary>
	public sealed partial class MainPage : Page
	{
		public MainPage()
		{
			this.InitializeComponent();
		}

		protected override async void OnNavigatedTo(NavigationEventArgs e)
		{
			base.OnNavigatedTo(e);

			/*ushort vid = 0x2B04;
			ushort pid = 0xC006;
			var devices = await DeviceInformation.FindAllAsync(Windows.Devices.SerialCommunication.SerialDevice.GetDeviceSelectorFromUsbVidPid(vid, pid));
			foreach(var d in devices)
			{
				SerialDevice sd = await SerialDevice.FromIdAsync(d.Id);
				sd.BaudRate = 9600;
				DataWriter writer = new DataWriter(sd.OutputStream);
				writer.WriteString("hi\n");

				DataReader reader = new DataReader(sd.InputStream);
					while (true)
					{
						var vv = await reader.LoadAsync(1);
						if (vv > 0)
						{
							var v = reader.ReadString(sd.BytesReceived);
							Debug.Write(v);
						}
					}
			}*/
        }
	}
}
