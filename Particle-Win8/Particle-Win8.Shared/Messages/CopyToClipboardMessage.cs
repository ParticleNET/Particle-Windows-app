﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Particle.Common.Messages
{
	public class CopyToClipboardMessage
	{
		public String Content { get; set; }

		public String SuccessMessage { get; set; }
	}
}
