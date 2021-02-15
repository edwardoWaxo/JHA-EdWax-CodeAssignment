using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JHA.WebServices.Contract.Twitter
{
    public class TwitterConfiguration
    {
		public string Url { get; set; }
		public string ApiKey { get; set; }
		public string ApiKeySecret { get; set; }
		public string AccessToken { get; set; }
		public string AccessTokenSecret { get; set; }
		public string BearerToken { get; set; }
	}
}