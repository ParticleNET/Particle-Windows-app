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
using Particle;
using Particle.Common;
using Particle.Common.Messages;
using Particle.Common.ViewModel;
using System;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;
#if WINDOWS_PHONE_APP
using Windows.Phone.UI.Input;
#else
using Windows.UI.ApplicationSettings;
#endif

// The Blank Application template is documented at http://go.microsoft.com/fwlink/?LinkId=234227

namespace Particle_Win8
{
	/// <summary>
	/// Provides application-specific behavior to supplement the default Application class.
	/// </summary>
	public sealed partial class App : Application
	{
#if WINDOWS_PHONE_APP
		private TransitionCollection transitions;
#endif

		/// <summary>
		/// Initializes the singleton application object.  This is the first line of authored code
		/// executed, and as such is the logical equivalent of main() or WinMain().
		/// </summary>
		public App()
		{
			this.InitializeComponent();
			this.Suspending += this.OnSuspending;
			this.Resuming += this.OnResuming;
#if WINDOWS_PHONE_APP
			HardwareButtons.BackPressed += HardwareButtons_BackPressed;
			ViewModelLocator.SupportsClipboard = false;
#else
			ViewModelLocator.SupportsClipboard = true;
#endif
		}

#if WINDOWS_PHONE_APP
		void HardwareButtons_BackPressed(object sender, BackPressedEventArgs e)
		{
			Frame rootFrame = Window.Current.Content as Frame;

			if (rootFrame != null && rootFrame.CanGoBack)
			{
				e.Handled = true;
				rootFrame.GoBack();
			}
		}
#endif

		/// <summary>
		/// Invoked when the application is launched normally by the end user.  Other entry points
		/// will be used when the application is launched to open a specific file, to display
		/// search results, and so forth.
		/// </summary>
		/// <param name="e">Details about the launch request and process.</param>
		protected override void OnLaunched(LaunchActivatedEventArgs e)
		{
#if DEBUG
			if (System.Diagnostics.Debugger.IsAttached)
			{
				this.DebugSettings.EnableFrameRateCounter = true;
			}
#endif

			Frame rootFrame = Window.Current.Content as Frame;
			
			ParticleCloud.SyncContext = System.Threading.SynchronizationContext.Current;

			// Do not repeat app initialization when the Window already has content,
			// just ensure that the window is active
			if (rootFrame == null)
			{
				// Create a Frame to act as the navigation context and navigate to the first page
				rootFrame = new Frame();

				// TODO: change this value to a cache size that is appropriate for your application
				rootFrame.CacheSize = 1;

				if (e.PreviousExecutionState == ApplicationExecutionState.Terminated)
				{
					// TODO: Load state from previously suspended application
				}

				// Place the frame in the current Window
				Window.Current.Content = rootFrame;
			}

			if (rootFrame.Content == null)
			{
#if WINDOWS_PHONE_APP
				// Removes the turnstile navigation for startup.
				if (rootFrame.ContentTransitions != null)
				{
					this.transitions = new TransitionCollection();
					foreach (var c in rootFrame.ContentTransitions)
					{
						this.transitions.Add(c);
					}
				}

				rootFrame.ContentTransitions = null;
				rootFrame.Navigated += this.RootFrame_FirstNavigated;
#endif

				// When the navigation stack isn't restored navigate to the first page,
				// configuring the new page by passing required information as a navigation
				// parameter
				if (!rootFrame.Navigate(typeof(MainPage), e.Arguments))
				{
					throw new Exception("Failed to create initial page");
				}
			}

			// Ensure the current window is active
			Window.Current.Activate();
		}

#if WINDOWS_PHONE_APP
		/// <summary>
		/// Restores the content transitions after the app has launched.
		/// </summary>
		/// <param name="sender">The object where the handler is attached.</param>
		/// <param name="e">Details about the navigation event.</param>
		private void RootFrame_FirstNavigated(object sender, NavigationEventArgs e)
		{
			var rootFrame = sender as Frame;
			rootFrame.ContentTransitions = this.transitions ?? new TransitionCollection() { new NavigationThemeTransition() };
			rootFrame.Navigated -= this.RootFrame_FirstNavigated;
		}
#endif

		protected override void OnWindowCreated(WindowCreatedEventArgs args)
		{
			base.OnWindowCreated(args);

#if !WINDOWS_PHONE_APP
			var settings = SettingsPane.GetForCurrentView();
			settings.CommandsRequested += Settings_CommandsRequested;
#endif
		}

#if !WINDOWS_PHONE_APP
		private void Settings_CommandsRequested(SettingsPane sender, SettingsPaneCommandsRequestedEventArgs args)
		{
			args.Request.ApplicationCommands.Add(new SettingsCommand("ReportBug", MM.M.GetString("Settings_ReportBug"), (a) =>
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
			args.Request.ApplicationCommands.Add(new SettingsCommand("Store", MM.M.GetString("Settings_Store"), async (a) =>
			{
				await Windows.System.Launcher.LaunchUriAsync(new Uri("https://store.particle.io/"));
			}));
			args.Request.ApplicationCommands.Add(new SettingsCommand("License", MM.M.GetString("Settings_License"), async (a) =>
			{
				await Windows.System.Launcher.LaunchUriAsync(new Uri("https://raw.githubusercontent.com/ParticleNET/Particle-Windows-app/master/LICENSE"));
			}));
		}
#endif
		/// <summary>
		/// Invoked when application execution is being suspended.  Application state is saved
		/// without knowing whether the application will be terminated or resumed with the contents
		/// of memory still intact.
		/// </summary>
		/// <param name="sender">The source of the suspend request.</param>
		/// <param name="e">Details about the suspend request.</param>
		private void OnSuspending(object sender, SuspendingEventArgs e)
		{
			var deferral = e.SuspendingOperation.GetDeferral();

			// TODO: Save application state and stop any background activity
			ViewModelLocator.Suspending();
			deferral.Complete();
		}


		/// <summary>
		/// Called when [resuming].
		/// </summary>
		/// <param name="sender">The sender.</param>
		/// <param name="e">The e.</param>
		private void OnResuming(object sender, object e)
		{
			ViewModelLocator.Resuming();
		}

	}
}