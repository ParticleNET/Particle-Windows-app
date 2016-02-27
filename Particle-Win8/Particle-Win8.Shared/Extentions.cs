using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Particle.Common
{
	public static class Extentions
	{
		public static void SendDialogMessage(this Result result)
		{
			ViewModel.ViewModelLocator.Messenger.Send(new Messages.DialogMessage
			{
				Title = result.Error ?? "",
				Description = result.ErrorDescription ?? ""
			});
		}
	}
}
