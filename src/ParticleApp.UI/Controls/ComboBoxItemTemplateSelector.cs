/*
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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace ParticleApp.UI.Controls
{
	public class ComboBoxItemTemplateSelector : DataTemplateSelector
	{
		public DataTemplate SelectedTemplate { get; set; }
		public DataTemplate DropDownTemplate { get; set; }

		protected override DataTemplate SelectTemplateCore(object item, DependencyObject container)
		{
			var comboBoxItem = container.GetVisualParent<ComboBoxItem>();
			if (comboBoxItem == null)
			{
				return SelectedTemplate;
			}
			return DropDownTemplate;
		}

	}

	public static class DependencyObjectExtensions
	{
		public static T GetVisualParent<T>(this DependencyObject child) where T : FrameworkElement
		{
			while ((child != null) && !(child is T))
			{
				child = VisualTreeHelper.GetParent(child);
			}
			return child as T;
		}
	}
}
