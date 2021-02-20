using System;
using System.Configuration;
using System.Threading.Tasks;
using JHA.WebServices.BusinessLogic.Interface;
using JHA.WebServices.Contract.Twitter;
using JHA.WebServices.Repository.Interface;
using Tweetinvi;
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
		/// <remarks>
		/// The class property, Repository, is injected with a concrete implementation 
		/// by our DependencyInjection.InjectDependencies class.
		/// </remarks>
		public TwitterProvider(ITwitterRepository repo)
		{
			this.Repository = repo;
		}

		#endregion

		#region Public Properties

		public ITwitterRepository Repository { get; set; }
		public Tweetinvi.Streaming.V2.ISampleStreamV2 SampleStream { get; set; }
		public DateTime StreamStartedAt { get; set; }

		#endregion

		#region Public Methods

		/// <summary>
		/// Consumes the (sample) stream.
		/// </summary>
		/// <param name="sampleStream">The sample stream.</param>
		public async Task ConsumeStream(Tweetinvi.Streaming.V2.ISampleStreamV2 sampleStream)
		{
			// Consume the sample twitter stream ...
			while (true)
			{
				try
				{
					sampleStream.TweetReceived += (sender, args) =>
					{
						var tweet = args.Tweet;
						if (tweet != null)
						{
							this.PersistTweet(tweet);
						}
					};

					await sampleStream.StartAsync();
				}
				catch (Exception e)
				{
				}
			}
		}

		/// <summary>
		/// Creates and consumes the Twitter sample stream.  Here we implement the SOA Design Principle of 
		/// Composability by composing the singular piece-parts into one.
		/// </summary>
		/// <param name="configFromRequest">The configuration from request.</param>
		public async Task CreateAndConsumeStream(TwitterConfiguration configFromRequest)
		{
			// Get the configuration keys/tokens, create credentials from them and finally create our Twitter client ...
			var config = this.GetConfiguration(configFromRequest);
			var appCredentials = this.CreateCredentials(config);
			var appClient = this.CreateTwitterClient(appCredentials);

			// Create our Sample Stream ...
			this.SampleStream = this.CreateStream(appClient);
			this.StreamStartedAt = DateTime.UtcNow;

			// Consume our Sample Stream ...
			try
			{
				await this.ConsumeStream(this.SampleStream);
			}
			catch (Exception e)
			{
				// Take corrective actions, if needed.  At the least, provide some type of feedback
				// via logging, so something like that.
			}
		}

		/// <summary>
		/// Creates the credentials.
		/// </summary>
		/// <param name="config">The configuration.</param>
		/// <returns></returns>
		public Tweetinvi.Models.ConsumerOnlyCredentials CreateCredentials(TwitterConfiguration config)
		{
			var appCredentials = new Tweetinvi.Models.ConsumerOnlyCredentials(config.ApiKey, config.ApiKeySecret)
			{
				BearerToken = config.BearerToken
			};

			return appCredentials;
		}

		/// <summary>
		/// Creates the stream.
		/// </summary>
		/// <param name="configFromRequest">The configuration from request.</param>
		/// <returns></returns>
		public Tweetinvi.Streaming.V2.ISampleStreamV2 CreateStream(TwitterConfiguration configFromRequest)
		{
			var config = this.GetConfiguration(configFromRequest);
			var appCredentials = this.CreateCredentials(config);
			var appClient = this.CreateTwitterClient(appCredentials);
			var sampleStreamV2 = appClient.StreamsV2.CreateSampleStream();
			this.StreamStartedAt = DateTime.UtcNow;

			return sampleStreamV2;
		}

		/// <summary>
		/// Creates the (sample) stream.
		/// </summary>
		/// <param name="client">The client.</param>
		/// <returns></returns>
		public Tweetinvi.Streaming.V2.ISampleStreamV2 CreateStream(TwitterClient client)
		{
			var sampleStream = client.StreamsV2.CreateSampleStream();

			return sampleStream;
		}

		/// <summary>
		/// Creates the twitter client.
		/// </summary>
		/// <param name="creds">The creds.</param>
		/// <returns></returns>
		public TwitterClient CreateTwitterClient(Tweetinvi.Models.ConsumerOnlyCredentials creds)
		{
			var appClient = new TwitterClient(creds);
			return appClient;
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

		/// <summary>
		/// Gets the configuration.
		/// </summary>
		/// <param name="configFromRequest">The configuration from request.</param>
		/// <returns></returns>
		public TwitterConfiguration GetConfiguration(TwitterConfiguration configFromRequest)
		{
			// Get the configuration info.  There are 2 possible sources:
			//	1. The body of the POST request (configFromRequest)
			//	2. The app.config file
			var config = (configFromRequest == null)
				? this.GetConfiguration()
				: configFromRequest;

			return config;
		}

		/// <summary>
		/// Gets the statistics.
		/// </summary>
		/// <returns></returns>
		public TwitterStatistics GetStatistics()
		{
			var stats = this.Repository.GetTwitterStatistics();
			
			return stats;
		}

		/// <summary>
		/// Persists the tweet.
		/// </summary>
		/// <param name="tweet">The tweet.</param>
		/// <returns></returns>
		public bool PersistTweet(TweetV2 tweet)
		{
			var isPersisted = this.Repository.PersistTweet(tweet);
			return isPersisted;
		}

		#endregion
	}
}