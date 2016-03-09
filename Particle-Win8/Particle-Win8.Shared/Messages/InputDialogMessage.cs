/*
   Copyright 2016 Sannel Software, L.L.C.

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
		/// <summary>
		/// The call back to call when the dialog is closed. 
		/// The First parameter is the text of the button that was clicked.
		/// The Second parameter is the text the user inputed.
		/// </summary>
		public Action<String, String> CallBack { get; set; }
	}
}
