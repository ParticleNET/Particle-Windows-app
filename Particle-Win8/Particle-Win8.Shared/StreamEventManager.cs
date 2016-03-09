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
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Windows.Web.Http;

namespace Particle_Win8
{
	/// <summary>
	/// 
	/// </summary>
	public class StreamEventManager : Particle.ParticleEventManager
	{
		private Uri streamUri;
		private String accessToken;

		/// <summary>
		/// Initializes a new instance of the <see cref="StreamEventManager"/> class.
		/// </summary>
		/// <param name="streamUri">The stream URI.</param>
		/// <param name="accessToken">The access token.</param>
		public StreamEventManager(Uri streamUri, String accessToken) : base(streamUri, accessToken)
		{
			this.streamUri = streamUri;
			this.accessToken = accessToken;
		}

		/// <summary>
		/// Connects to client.
		/// </summary>
		/// <returns></returns>
		protected override async Task ConnectToClient()
		{
			using (HttpClient client = new HttpClient())
			{
				client.DefaultRequestHeaders.Authorization = new Windows.Web.Http.Headers.HttpCredentialsHeaderValue("Bearer", accessToken);
				using (var result = await client.GetInputStreamAsync(streamUri))
				{
					using (var s = result.AsStreamForRead())
					{
						await ListensToStreamAsync(s);
					}
				}
			}
		}
	}
}
