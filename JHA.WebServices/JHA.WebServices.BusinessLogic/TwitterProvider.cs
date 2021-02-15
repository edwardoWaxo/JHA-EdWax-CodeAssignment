using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using Tweetinvi;
using Tweetinvi.Models;
using JHA.WebServices.BusinessLogic.Interface;
using JHA.WebServices.Contract;
using JHA.WebServices.Contract.Twitter;
using JHA.WebServices.Repository.Interface;
using Tweetinvi.Models.V2;

namespace JHA.WebServices.BusinessLogic
{
	/// <summary>
	/// The TwitterProvider.  This class contains core business logic for the TwitterController (aka service).
	/// </summary>
	/// <seealso cref="JHA.WebServices.BusinessLogic.Interface.ITwitterProvider" />
	public class TwitterProvider : ITwitterProvider
    {
		#region Constructor

		/// <summary>
		/// Initializes a new instance of the <see cref="TwitterProvider"/> class.
		/// </summary>
		public TwitterProvider(ITwitterRepository repo)
		{
			this.Repository = repo;
		}

		#endregion

		#region Public Properties

		public ITwitterRepository Repository { get; set; }
		public DateTime StreamStartedAt { get; set; }

		#endregion

		#region Public Methods

		/// <summary>
		/// Creates the and consume stream.
		/// </summary>
		/// <returns></returns>
		/// <exception cref="NotImplementedException"></exception>
		public async Task CreateAndConsumeStream()
		{
			var config = this.GetConfiguration();
			//var appCredentials = new Tweetinvi.Models.ConsumerOnlyCredentials(config.ApiKey, config.ApiKeySecret)
			//{
			//	BearerToken = config.BearerToken
			//};
			var appCredentials = this.CreateCredentials(config);

			//var appClient = new TwitterClient(appCredentials);
			var appClient = this.CreateTwitterClient(appCredentials);

			var sampleStreamV2 = appClient.StreamsV2.CreateSampleStream();
			this.StreamStartedAt = DateTime.UtcNow;

			// TODO: robust error handling ...
			{
				sampleStreamV2.TweetReceived += (sender, args) =>
				{
					var tweet = args.Tweet;
					if (tweet != null)
					{
						this.Repository.PersistTweet(tweet);
					}
				};

				await sampleStreamV2.StartAsync();
			}
		}

		public TwitterClient CreateTwitterClient(Tweetinvi.Models.ConsumerOnlyCredentials creds)
		{
			var appClient = new TwitterClient(creds);
			return appClient;
		}

		public Tweetinvi.Models.ConsumerOnlyCredentials CreateCredentials(TwitterConfiguration config)
		{
			var appCredentials = new Tweetinvi.Models.ConsumerOnlyCredentials(config.ApiKey, config.ApiKeySecret)
			{
				BearerToken = config.BearerToken
			};

			return appCredentials;
		}

		/// <summary>
		/// Gets the configuration.
		/// </summary>
		/// <returns></returns>
		public TwitterConfiguration GetConfiguration()
		{
			var config = new TwitterConfiguration();
			config.Url = ConfigurationManager.AppSettings[JHA.WebServices.Contract.Constants.TwitterConstants.TwitterUrlSetting];
			config.ApiKey = ConfigurationManager.AppSettings[JHA.WebServices.Contract.Constants.TwitterConstants.TwitterApiKeySetting];
			config.ApiKeySecret = ConfigurationManager.AppSettings[JHA.WebServices.Contract.Constants.TwitterConstants.TwitterApiKeySecretSetting];
			config.AccessToken = ConfigurationManager.AppSettings[JHA.WebServices.Contract.Constants.TwitterConstants.TwitterAccessTokenSetting];
			config.AccessTokenSecret = ConfigurationManager.AppSettings[JHA.WebServices.Contract.Constants.TwitterConstants.TwitterAccessTokenSecretSetting];
			config.BearerToken = ConfigurationManager.AppSettings[JHA.WebServices.Contract.Constants.TwitterConstants.TwitterBearerTokenSetting];

			return config;
		}

		public TwitterStatistics GetStatistics()
		{
			var stats = this.Repository.GetTwitterStatistics();
			
			return stats;
		}

		public bool PersistTweet(TweetV2 tweet)
		{
			var isPersisted = this.Repository.PersistTweet(tweet);
			return isPersisted;
		}

		#endregion
	}
}