using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.IO;
using System.Runtime.Serialization.Json;
using Newtonsoft.Json;

namespace JHA.WebServices.BusinessLogic
{
    public class HttpClient
    {
        #region Constructor(s)
        public HttpClient()
        {
            this.WebClient = new WebClient();
            //this.WebClient.Headers["Accept"] = "application/json";
            //this.WebClient.Headers["Content-type"] = "application/json";
            //this.WebClient.Headers["${AuthizationHeaderName}"] = $"{Bearer} ";
            this.WebClient.Encoding = Encoding.UTF8;
            System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
        }
        public HttpClient(List<KeyValuePair<string, string>> httpHeaders) : this()
        {
            foreach (var header in httpHeaders)
            {
                this.WebClient.Headers.Add(header.Key, header.Value);
            }
        }

		#endregion

		#region Public Properties

		public WebClient WebClient { get; set; }

        public string BaseUri { get; set; }
        public string Resource { get; set; }
        public string QueryString { get; set; }
        public const string AuthorizationHeaderName = "Authorization";
        public const string Bearer = "Bearer";

        #endregion

        #region Constants

        private const string HttpDeleteVerb = "DELETE";
        private const string HttpGetVerb = "GET";
        private const string HttpPatchVerb = "PATCH";
        private const string HttpPostVerb = "POST";
        private const string HttpPutVerb = "PUT";

        #endregion

        #region Public Methods

        public void HttpDelete<T>(T req, string baseUri, string resource, string queryString) where T : class
        {
            var url = baseUri;
            if (string.IsNullOrEmpty(url))
            {
                return;
            }
            if (!string.IsNullOrEmpty(resource))
            {
                url += $"/{resource}";
            }
            if (!string.IsNullOrEmpty(queryString))
            {
                url += $"?{queryString}";
            }

            try
            {
                var ser = new DataContractJsonSerializer(typeof(T));
                var mem = new MemoryStream();
                ser.WriteObject(mem, req);
                var requestData = Encoding.UTF8.GetString(mem.ToArray(), 0, (int)mem.Length);

                // Issue the DELETE request.
                var response = this.WebClient.UploadString(url, HttpDeleteVerb, requestData);

            }
            catch (Exception e)
            {

            }
        }

        public U HttpGet<U>(string baseUri, string resource, string queryString)
        {
            var url = baseUri;
            if (string.IsNullOrEmpty(url))
            {
                return default;
            }
            if (!string.IsNullOrEmpty(resource))
            {
                url += $"/{resource}";
            }
            if (!string.IsNullOrEmpty(queryString))
            {
                url += $"?{queryString}";
            }

            var response = default(U);

            try
            {
                var rsp = this.WebClient.DownloadString(url);
                response = JsonConvert.DeserializeObject<U>(rsp);
            }
            catch (Exception e)
            {
            }

            return response;
        }

        #endregion
    }
}