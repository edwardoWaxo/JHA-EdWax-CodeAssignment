using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using JHA.WebServices.BusinessLogic.Interface;
using JHA.WebServices.Contract.Twitter;
using JHA.WebServices.Repository.Interface;
using Newtonsoft.Json;
using NUnit.Framework;
using Tweetinvi;

namespace JHA.WebServices.BusinessLogic.UnitTest
{
	[TestFixture]
	public class UnitTest
	{
		#region Public Properties

		public ITwitterProvider TwitterProvider { get; set; }
		public ITwitterRepository Repository { get; set; }
		public TwitterConfiguration Config { get; set; }
		public  TwitterClient AppClient { get; set; }

		#endregion

		#region SetUp / TearDown

		/// <summary>
		/// Sets up the runtime for these unit tests.
		/// 
		/// This would be a good place to run database scripts to setup test data.
		/// </summary>
		[OneTimeSetUp]
		public void SetUp()
		{
			// Unit test Dependency Injection
			this.Repository = new JHA.WebServices.Repository.InMemory.UnitTest.TwitterRepository();
			this.TwitterProvider = new JHA.WebServices.BusinessLogic.UnitTest.TwitterProvider(this.Repository);

			Console.WriteLine("SetUp task/s are complete!");
		}

		/// <summary>
		/// Tears down.  
		/// 
		/// This would be a good place to run a corresponding "undo" database script to undo what was run in SetUp.
		/// </summary>
		[OneTimeTearDown]
		public void TearDown()
		{
		}

		#endregion

		#region Unit Tests
		#endregion

		#region TwitterFeed Unit Tests

		[Test]
		public void ConsumeTwitterSampleStream_Test()
		{
			// Arrange
			// Act
			var consumeTask = Task.Run(() => ReadFromStream_Async());
			// Assert
			Assert.IsNotNull(consumeTask);

			/***
			var config = this.TwitterProvider.GetConfiguration();
			var appCredentials = new Tweetinvi.Models.ConsumerOnlyCredentials(config.ApiKey, config.ApiKeySecret)
			{
				BearerToken = config.BearerToken
			};
			var appClient = new TwitterClient(appCredentials);
			var tweetList = new List<Tweetinvi.Models.V2.TweetV2>();
			var sampleStreamV2 = appClient.StreamsV2.CreateSampleStream();

			{
				Console.WriteLine($"reading from stream ... ");
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
			***/

			// Assert
		}

		private async Task<List<Tweetinvi.Models.V2.TweetV2>>  ReadFromStream_Async()
		{
			var config = this.TwitterProvider.GetConfiguration();
			var appCredentials = new Tweetinvi.Models.ConsumerOnlyCredentials(config.ApiKey, config.ApiKeySecret)
			{
				BearerToken = config.BearerToken
			};
			var appClient = new TwitterClient(appCredentials);
			var tweetList = new List<Tweetinvi.Models.V2.TweetV2>();
			var sampleStreamV2 = appClient.StreamsV2.CreateSampleStream();

			{
				Console.WriteLine($"reading from stream ... ");
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
				
				return tweetList;
			}
		}

		[Test]
		public void CreateTwitterCredentials_Test()
		{
			// Arrange
			// Act
			var config = this.TwitterProvider.GetConfiguration();
			var appCredentials = new Tweetinvi.Models.ConsumerOnlyCredentials(config.ApiKey, config.ApiKeySecret)
			{
				BearerToken = config.BearerToken
			};

			// Assert
			Assert.IsNotNull(config);
			Assert.IsNotNull(appCredentials);
			Assert.AreNotEqual(string.Empty, config.BearerToken);
		}

		[Test]
		public void CreateTwitterClient_Test()
		{
			// Arrange
			TwitterClient appClient;

			// Act
			var config = this.TwitterProvider.GetConfiguration();
			var appCredentials = new Tweetinvi.Models.ConsumerOnlyCredentials(config.ApiKey, config.ApiKeySecret)
			{
				BearerToken = config.BearerToken
			};
			appClient = new TwitterClient(appCredentials);

			// Assert
			Assert.IsNotNull(config);
			Assert.IsNotNull(appClient);
		}

		[Test]
		public void CreateTwitterSampleStream_Test()
		{
			// Arrange
			TwitterClient appClient;
			
			// Act
			var config = this.TwitterProvider.GetConfiguration();
			var appCredentials = new Tweetinvi.Models.ConsumerOnlyCredentials(config.ApiKey, config.ApiKeySecret)
			{
				BearerToken = config.BearerToken
			};
			appClient = new TwitterClient(appCredentials);
			var sampleStreamV2 = appClient.StreamsV2.CreateSampleStream();
			sampleStreamV2.TweetReceived += (sender, args) =>
			{ 
				System.Console.WriteLine(args.Tweet.Text);
			};


			//await sampleStreamV2.StartAsync();

			// Assert
			Assert.IsNotNull(config);
			Assert.IsNotNull(appClient);
			Assert.IsNotNull(sampleStreamV2);
		}

		[Test]
		public void GetConfig_Test()
		{
			// Arrange

			// Act
			var config = this.TwitterProvider.GetConfiguration();

			// Assert
			Assert.IsNotNull(config);
			Assert.IsNotNull(config.Url);
			Assert.IsNotNull(config.ApiKey);
			Assert.IsNotNull(config.ApiKeySecret);
			Assert.IsNotNull(config.AccessToken);
			Assert.IsNotNull(config.AccessTokenSecret);
			Assert.IsNotNull(config.BearerToken);
		}

		[Test]
		public void GetTwitterStatistics_Test()
		{
			// Arrange
			TwitterStatistics stats;

			// Act
			stats = new TwitterStatistics();

			// Assert
			Assert.IsNotNull(stats.EmojiList);
			Assert.IsEmpty(stats.EmojiList);
			Assert.IsNotNull(stats.HashTags);
			Assert.IsEmpty(stats.HashTags);
			Assert.IsNotNull(stats.UrlList);
			Assert.IsEmpty(stats.UrlList);
			Assert.AreEqual(0, stats.TweetCount);

			Console.WriteLine($"Twitter statistics: {stats.ToString()}");
		}

		[Test]
		public void SerializeConfig_Test()
		{
			// Arrange

			// Act
			var config = this.TwitterProvider.GetConfiguration();
			var jsonString = JsonConvert.SerializeObject(config);

			// Assert
			Assert.IsNotNull(config);
			Assert.IsNotNull(config.Url);
			Assert.IsNotNull(config.ApiKey);
			Assert.IsNotNull(config.ApiKeySecret);
			Assert.IsNotNull(config.AccessToken);
			Assert.IsNotNull(config.AccessTokenSecret);
			Assert.IsNotNull(config.BearerToken);
			Assert.IsNotNull(jsonString);
		}

		#endregion
	}
}