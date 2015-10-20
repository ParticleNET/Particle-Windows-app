using Particle.Common.Messages;
using System;
using System.Collections.Generic;
using System.Text;
using Windows.UI.Popups;

namespace Particle_Win8
{
	public class AppMessagesHandler
	{
		public void SetupMessageHandlers()
		{
			GalaSoft.MvvmLight.Messaging.Messenger.Default.Register<DialogMessage>(this, dialogMessageReceiver);
		}

		private async void dialogMessageReceiver(DialogMessage dm)
		{
			MessageDialog dialog = null;
			if (dm.Description != null && dm.Title != null)
			{
				dialog = new MessageDialog(dm.Description, dm.Title);
			}
			else
			{
				dialog = new MessageDialog(dm.Description ?? "");
			}

			if(dm.Buttons != null && dm.Buttons.Count > 0)
			{
				foreach(var b in dm.Buttons)
				{
					dialog.Commands.Add(new UICommand(b.Text) { Id = b.Id });
				}
			}

			var result = await dialog.ShowAsync();
			if(dm.CallBack != null)
			{
				dm.CallBack(Convert.ToInt32(result.Id));
			}
		}
	}
}
