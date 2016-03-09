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
using GalaSoft.MvvmLight.Command;
using Particle.Common.Interfaces;
using Particle.Common.Messages;
using System;
using System.Windows.Input;

namespace Particle.Common.ViewModel
{
	public class CommandsViewModel : ICommandsViewModel
	{
		private ICommand licenseCommand;

		public ICommand LicenseCommand
		{
			get
			{
				return licenseCommand ?? (licenseCommand = new RelayCommand(async () =>
				{
					await Windows.System.Launcher.LaunchUriAsync(new Uri("https://raw.githubusercontent.com/ParticleNET/Particle-Windows-app/master/LICENSE"));
				}));
			}
		}

		private ICommand particleStoreCommand;

		public ICommand ParticleStoreCommand
		{
			get
			{
				return particleStoreCommand ?? (particleStoreCommand = new RelayCommand(async () =>
				{
					await Windows.System.Launcher.LaunchUriAsync(new Uri("https://store.particle.io/"));
				}));
			}
		}

		private ICommand reportBugCommand;
		public ICommand ReportBugCommand
		{
			get
			{
				return reportBugCommand ?? (reportBugCommand = new RelayCommand(() =>
				{
					DialogMessage message = new DialogMessage();
					message.Title = MM.M.GetString("RAB_Dialog_Title");
					message.Description = MM.M.GetString("RAB_Dialog_Description");
					message.Buttons = new System.Collections.Generic.List<Particle.Common.Models.MessageButtonModel>() {
					new Particle.Common.Models.MessageButtonModel()
					{
						Id = 1,
						Text = MM.M.GetString("RAB_Dialog_GoToGithub")
					},
					new Particle.Common.Models.MessageButtonModel()
					{
						Id = 2,
						Text = MM.M.GetString("Cancel_Button")
					}
				};
					message.CallBack = async (id) =>
					{
						if (id == 1)
						{
							await Windows.System.Launcher.LaunchUriAsync(new Uri("https://github.com/ParticleNET/Particle-Windows-app/issues"));
						}
					};
					ViewModelLocator.Messenger.Send(message);
				}));
			}
		}
	}
}
