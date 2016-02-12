using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using OAuth;

namespace AssociatePublishingNetwork {

	public class ApnClient {

		private const string ACCESS_ID = "";
		private const string ACCESS_SECRET = "";
		public const string HOSTNAME = "";

		public async Task<string> SendRequest(string endpointUrl, Dictionary<string, string> queryStringParams, HttpMethod httpMethod) {
			// create oauth client to generate our auth header
			OAuthRequest client = new OAuthRequest {
				Method = httpMethod.ToString().ToUpper(),
				Type = OAuthRequestType.ProtectedResource,
				SignatureMethod = OAuthSignatureMethod.HmacSha256,
				ConsumerKey = ACCESS_ID,
				ConsumerSecret = ACCESS_SECRET,
				RequestUrl = string.Concat(HOSTNAME, endpointUrl),
				Version = "1.0",
			};

			using (var http = new HttpClient()) {
				// add our auth header
				string authHeader;
				// for some reason, GET requests should not have the query string params in the auth header. this might be a bug on the server side.
				if (httpMethod != HttpMethod.Get && queryStringParams != null) {
					authHeader = client.GetAuthorizationHeader(queryStringParams);
				} else {
					authHeader = client.GetAuthorizationHeader();
				}
				http.DefaultRequestHeaders.Add("Authorization", authHeader);

				var url = client.RequestUrl;
				HttpResponseMessage response;

				if (httpMethod == HttpMethod.Get) {
					// GET
					// add our query string back into the url if its set
					if (queryStringParams != null) {
						url = string.Format("{0}?{1}", url, GetQueryString(queryStringParams));
					}
					response = await http.GetAsync(url);
				} else if (httpMethod == HttpMethod.Post) {
					// POST
					response = await http.PostAsync(url, queryStringParams == null ? null : new FormUrlEncodedContent(queryStringParams));
				} else if (httpMethod == HttpMethod.Put) {
					// PUT
					response = await http.PutAsync(url, queryStringParams == null ? null : new FormUrlEncodedContent(queryStringParams));
				} else {
					throw new Exception("http method not supported: " + httpMethod);
				}

				// handle response
				var content = await response.Content.ReadAsStringAsync();
				if (response.IsSuccessStatusCode) {
					return content;
				} else {
					return string.Format("{0}: {1}", response.StatusCode, content);
				}
			}

			return null;
		}

		private string GetQueryString(Dictionary<string, string> dict) {
			if (dict == null) { return ""; }
			List<string> sb = new List<string>();
			foreach (var kvp in dict) {
				sb.Add(string.Format("{0}={1}", OAuthTools.UrlEncodeStrict(kvp.Key), OAuthTools.UrlEncodeStrict(kvp.Value)));
			}
			return string.Join("&", sb);
		}
	}

}