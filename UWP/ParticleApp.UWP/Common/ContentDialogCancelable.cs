﻿/*
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
using ParticleApp.Business.Interfaces;
using Windows.UI.Xaml.Controls;

namespace ParticleApp.UWP.Common
{
	public class ContentDialogCancelable : ICancelable
	{
		private ContentDialogButtonClickEventArgs args;
		public ContentDialogCancelable(ContentDialogButtonClickEventArgs args)
		{
			this.args = args;
		}
		public bool Cancel
		{
			get
			{
				return args.Cancel;
			}

			set
			{
				args.Cancel = value;
			}
		}

		public IDeferral GetDeferal()
		{
			return new ContentDialogDeferral(args.GetDeferral());
		}
	}
}
