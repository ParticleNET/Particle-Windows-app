/*
   Copyright 2015 Sannel Software, L.L.C.

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
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.Security.Cryptography;
using Windows.Security.Cryptography.Core;
using Windows.Storage;
using Windows.Storage.Streams;

namespace Particle.Common.ViewModel
{
	public class AppSettings : INotifyPropertyChanged
	{
		private ApplicationDataContainer settings;

		public static AppSettings Current { get; } = new AppSettings();

		private AppSettings()
		{
			settings = ApplicationData.Current.LocalSettings;
			var packageId = Windows.System.Profile.HardwareIdentification.GetPackageSpecificToken(null);
			var hid = packageId.Id;
			String id;
			using (var reader = DataReader.FromBuffer(hid))
			{
				var b = new byte[hid.Length];
				reader.ReadBytes(b);
				id = Encoding.UTF8.GetString(b, 0, b.Length);
			}
			generateKey(id, Package.Current.Id.PublisherId, 1000);
		}

		#region encryption/decryption
		/* Special thanks to Frank at http://www.franksworld.com/2013/06/12/how-to-encrypt-settings-in-windows-8-apps/ for the encryption and decription portion of this */

		private IBuffer keyMaterial;
		private IBuffer iv;

		private void generateKey(string password, string salt, uint iterationCount)
		{

			// Setup KDF parameters for the desired salt and iteration count
			IBuffer saltBuffer = CryptographicBuffer.ConvertStringToBinary(salt, BinaryStringEncoding.Utf8);
			KeyDerivationParameters kdfParameters = KeyDerivationParameters.BuildForPbkdf2(saltBuffer, iterationCount);

			// Get a KDF provider for PBKDF2, and store the source password in a Cryptographic Key
			KeyDerivationAlgorithmProvider kdf = KeyDerivationAlgorithmProvider.OpenAlgorithm(KeyDerivationAlgorithmNames.Pbkdf2Sha256);
			IBuffer passwordBuffer = CryptographicBuffer.ConvertStringToBinary(password, BinaryStringEncoding.Utf8);
			CryptographicKey passwordSourceKey = kdf.CreateKey(passwordBuffer);

			// Generate key material from the source password, salt, and iteration count.  Only call DeriveKeyMaterial once,
			// since calling it twice will generate the same data for the key and IV.
			int keySize = 256 / 8;
			int ivSize = 128 / 8;
			uint totalDataNeeded = (uint)(keySize + ivSize);
			IBuffer keyAndIv = CryptographicEngine.DeriveKeyMaterial(passwordSourceKey, kdfParameters, totalDataNeeded);

			// Split the derived bytes into a seperate key and IV
			byte[] keyMaterialBytes = keyAndIv.ToArray();
			keyMaterial = WindowsRuntimeBuffer.Create(keyMaterialBytes, 0, keySize, keySize);
			iv = WindowsRuntimeBuffer.Create(keyMaterialBytes, keySize, ivSize, ivSize);

		}

		private String encrypt(String text)
		{
			IBuffer clearTextBuffer = CryptographicBuffer.ConvertStringToBinary(text, BinaryStringEncoding.Utf8);

			// Setup an AES key, using AES in CBC mode and applying PKCS#7 padding on the input
			SymmetricKeyAlgorithmProvider aesProvider = SymmetricKeyAlgorithmProvider.OpenAlgorithm(SymmetricAlgorithmNames.AesCbcPkcs7);
			CryptographicKey aesKey = aesProvider.CreateSymmetricKey(keyMaterial);

			// Encrypt the data and convert it to a Base64 string
			IBuffer encrypted = CryptographicEngine.Encrypt(aesKey, clearTextBuffer, iv);
			string ciphertextString = CryptographicBuffer.EncodeToBase64String(encrypted);

			return ciphertextString;
		}

		private String decrypt(String text)
		{
			try
			{
				// Setup an AES key, using AES in CBC mode and applying PKCS#7 padding on the input
				SymmetricKeyAlgorithmProvider aesProvider = SymmetricKeyAlgorithmProvider.OpenAlgorithm(SymmetricAlgorithmNames.AesCbcPkcs7);
				CryptographicKey aesKey = aesProvider.CreateSymmetricKey(keyMaterial);

				// Convert the base64 input to an IBuffer for decryption
				IBuffer ciphertextBuffer = CryptographicBuffer.DecodeFromBase64String(text);

				// Decrypt the data and convert it back to a string
				IBuffer decryptedBuffer = CryptographicEngine.Decrypt(aesKey, ciphertextBuffer, iv);
				byte[] decryptedArray = decryptedBuffer.ToArray();
				string clearText = Encoding.UTF8.GetString(decryptedArray, 0, decryptedArray.Length);

				return clearText;
			}
			catch { }
			return String.Empty;
		}
		#endregion

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
				return decrypt(s);
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
				settings.Values[key] = encrypt(value);
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

		public String Password
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