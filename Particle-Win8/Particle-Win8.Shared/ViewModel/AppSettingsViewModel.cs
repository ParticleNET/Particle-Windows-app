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
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Windows.Security.Credentials;
using Windows.Storage;

namespace ParticleApp.Business.ViewModel
{
	public class AppSettings : INotifyPropertyChanged
	{
		private const String RESOURCE = "particle.io";
		private ApplicationDataContainer settings;

		public static AppSettings Current { get; } = new AppSettings();

		private AppSettings()
		{
			settings = ApplicationData.Current.LocalSettings;
		}

		public event PropertyChangedEventHandler PropertyChanged;

		private void firePropertyChanged(String property)
		{
			if (PropertyChanged != null)
			{
				lock (PropertyChanged)
				{
					PropertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(property));
				}
			}
		}

		private void setBoolean(bool value, [CallerMemberName]String key = null)
		{
			settings.Values[key] = value;
			firePropertyChanged(key);
		}

		private bool getBoolean([CallerMemberName]String key = null, bool def = false)
		{
			bool? b = settings.Values[key] as bool?;
			if (b.HasValue)
			{
				return b.Value;
			}

			settings.Values[key] = def;

			return def;
		}

		private int getInt([CallerMemberName]String key = null, int def = 0)
		{
			int? b = settings.Values[key] as int?;
			if (b.HasValue)
			{
				return b.Value;
			}

			settings.Values[key] = def;

			return def;
		}

		private void setInt(int value, [CallerMemberName]String key = null)
		{
			settings.Values[key] = value;
			firePropertyChanged(key);
		}

		private long getLong([CallerMemberName]String key = null, long def = 0)
		{
			long? b = settings.Values[key] as long?;
			if (b.HasValue)
			{
				return b.Value;
			}

			settings.Values[key] = def;

			return def;
		}

		private void setLong(long value, [CallerMemberName]String key = null)
		{
			settings.Values[key] = value;
			firePropertyChanged(key);
		}

		private String getString([CallerMemberName]String key = null, String def = null)
		{
			String s = settings.Values[key] as String;
			if (s != null)
			{
				return s;
			}

			return def;
		}

		private void setString(String value, [CallerMemberName]String key = null)
		{
			if (value == null)
			{
				settings.Values[key] = null;
			}
			else
			{
				settings.Values[key] = value;
			}
		}

		public String Username
		{
			get
			{
				return getString();
			}
			set
			{
				setString(value);
			}
		}

		public bool RememberPassword
		{
			get
			{
				return getBoolean();
			}
			set
			{
				setBoolean(value);
			}
		}

		/// <summary>
		/// Gets or sets the stored device identifier.
		/// </summary>
		/// <value>
		/// The stored device identifier.
		/// </value>
		public String StoredDeviceId
		{
			get
			{
				return getString();
			}
			set
			{
				setString(value);
			}
		}

		public void StorePassword(String password)
		{
			try
			{
				var passwordVault = new Windows.Security.Credentials.PasswordVault();
				passwordVault.Add(new Windows.Security.Credentials.PasswordCredential(RESOURCE, Username, password));
			}
			catch { }
		}

		public void DeleteStoredPassword()
		{
			try
			{
				var passwordVault = new PasswordVault();
				var list = passwordVault.FindAllByResource(RESOURCE);
				foreach (var item in list)
				{
					if (String.Compare(item.UserName, Username, StringComparison.CurrentCultureIgnoreCase) == 0)
					{
						passwordVault.Remove(item);
					}
				}
			}
			catch { }
		}

		public string GetStoredPassword()
		{
			try
			{
				var passwordValut = new Windows.Security.Credentials.PasswordVault();
				var creds = passwordValut.Retrieve(RESOURCE, Username);
				return creds.Password;
			}
			catch
			{
				return null;
			}
		}

		public bool AutoLogin
		{
			get
			{
				return getBoolean();
			}
			set
			{
				setBoolean(value);
			}
		}
	}
}