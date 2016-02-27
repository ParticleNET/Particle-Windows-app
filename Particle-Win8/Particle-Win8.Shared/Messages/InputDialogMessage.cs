using Particle.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Particle.Common.Messages
{
	public class InputDialogMessage
	{
		public String Title { get; set; }
		public String Description { get; set; }
		public String[] Buttons { get; set; }
		public Action<String, String> CallBack { get; set; }
	}
}
