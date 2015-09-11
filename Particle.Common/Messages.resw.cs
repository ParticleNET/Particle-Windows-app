using System;
using System.Collections.Generic;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Resources;

namespace Particle.Common
{
	public class Messages
	{
		public static Messages MM { get; } = new Messages();

		private ResourceLoader loader;

		public Messages()
		{
			loader = ResourceLoader.GetForCurrentView("Particle.Common/Resources");
		}

		public String GetString(String key)
		{
			return loader.GetString(key);
		}
	}
}
