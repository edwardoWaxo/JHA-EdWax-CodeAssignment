using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tweetinvi;
using JHA.WebServices.BusinessLogic.Interface;
using JHA.WebServices.Repository.Interface;
using JHA.WebServices.Repository.InMemory.UnitTest;
using JHA.WebServices.Contract;
using JHA.WebServices.Contract.Twitter;

namespace JHA.WebServices.BusinessLogic.UnitTest
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
			var appCredentials = new Tweetinvi.Models.ConsumerOnlyCredentials(config.ApiKey, config.ApiKeySecret)
			{
				BearerToken = config.BearerToken
			};
			var appClient = new TwitterClient(appCredentials);

			var tweetList = new List<Tweetinvi.Models.V2.TweetV2>(); // TODO: move to repository

			var sampleStreamV2 = appClient.StreamsV2.CreateSampleStream();
			{
				sampleStreamV2.TweetReceived += (sender, args) =>
				{
					var tweet = args.Tweet;
					if (tweet != null)
					{
						tweetList.Add(tweet);
						Console.WriteLine($"got another tweet ... count is {tweetList.Count}");
						var tweetString = appClient.Json.Serialize<Tweetinvi.Models.V2.TweetV2>(tweet);
						if (tweetList.Count % 15 == 0)
						{
							Console.WriteLine($"Tweet as JSON string: {tweetString}");
						}
						//if (tweet.Text != null) Console.WriteLine($"tweet text is: {tweet.Text}");
						//if (tweet.Entities != null) Console.WriteLine($"tweet hashtag is: {tweet.Entities.Hashtags}");
					}
					//System.Console.WriteLine(args.Tweet.Text);
				};

				await sampleStreamV2.StartAsync();
			}
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
			throw new NotImplementedException();
		}

		public bool PersistTweet(Tweetinvi.Models.V2.TweetV2 tweet)
		{
			throw new NotImplementedException();
		}

		#endregion
	}
}