using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using NUnit;
using NUnit.Framework;
using JHA.WebServices.Repository.Interface;
using Newtonsoft.Json;
using Tweetinvi.Models.V2;

namespace JHA.WebServices.Repository.InMemory.UnitTest
{
	[TestFixture]
	public class UnitTest
	{
		#region Public Properties

		public ITwitterRepository Repository { get; set; }
		public string StreamFileName = "SampleSerializedTwitterFeed.json";
		public List<TweetV2> TweetList { get; set; }

		#endregion

		#region SetUp / TearDown

		[OneTimeSetUp]
		public void SetUp()
		{
			this.Repository = new JHA.WebServices.Repository.InMemory.UnitTest.TwitterRepository();
			Console.WriteLine($"Starting unit tests at {DateTime.Now} ...");
		}

		[OneTimeTearDown]

		public static void TearDown()
		{
			Console.WriteLine($"Unit tests are complete: {DateTime.Now} ...");
		}

		#endregion

		#region Unit Tests
		#endregion

		#region TwitterFeed Unit Tests

		[TestCase(30)]
		[TestCase(60)]
		[TestCase(90)]
		[TestCase(300)]
		[TestCase(118)]
		[TestCase(0)]
		[TestCase(467)]
		[TestCase(3600)]
		[TestCase(7200)]
		[TestCase(8800)]
		public static void CalculateTwitterRates_Test(int sampleTimeInSeconds)
		{
			// Arrange
			const int hoursAsSeconds = (60 * 60);
			const int minutesAsSeconds = 60;
			float perSecond;
			double perMinute;
			double perHour;
			const int tweetCount = 500;

			// Act
			perSecond = sampleTimeInSeconds > 0 ? (float)tweetCount / sampleTimeInSeconds : 0;
			perMinute = sampleTimeInSeconds >= minutesAsSeconds ? (float)tweetCount / (sampleTimeInSeconds / minutesAsSeconds) : 0;
			perHour = sampleTimeInSeconds >= hoursAsSeconds ? (float)tweetCount / (sampleTimeInSeconds / hoursAsSeconds) : 0;

			double minuteDenominator = (double)(sampleTimeInSeconds / minutesAsSeconds);
			double hourDenominator = sampleTimeInSeconds / hoursAsSeconds;
			perMinute = (double)(tweetCount / minuteDenominator);
			perHour = (double)(tweetCount / (double)(sampleTimeInSeconds / hourDenominator));

			Console.WriteLine("CalculateTwitterRates_Test ...");
			Console.WriteLine($"tweets: " +
				$"{tweetCount}, sampleTime seconds: {sampleTimeInSeconds}, " +
				$"perSecond: {perSecond}, perMinute: {perMinute}, perHour: {perHour}");

			// Assert
		}

		[TestCase(30)]
		[TestCase(60)]
		[TestCase(90)]
		[TestCase(300)]
		[TestCase(118)]
		[TestCase(0)]
		[TestCase(467)]
		[TestCase(3600)]
		[TestCase(7200)]
		[TestCase(8800)]
		public static void CalculateTwitterRates_Foat_Test(int sampleTimeInSeconds)
		{
			// Arrange
			const int hoursAsSeconds = (60 * 60);
			const int minutesAsSeconds = 60;
			float perSecond;
			float perMinute;
			float perHour;
			const int tweetCount = 500;

			// Act

			//perMinute = sampleTimeInSeconds >= minutesAsSeconds ? (float)tweetCount / (sampleTimeInSeconds / minutesAsSeconds) : 0;
			//perHour = sampleTimeInSeconds >= hoursAsSeconds ? (float)tweetCount / (sampleTimeInSeconds / hoursAsSeconds) : 0;

			float minuteDenominator = (float)(sampleTimeInSeconds / (float)minutesAsSeconds);
			float hourDenominator = (float)sampleTimeInSeconds / (float)hoursAsSeconds;

			perSecond = sampleTimeInSeconds > 0 ? (float)tweetCount / sampleTimeInSeconds : 0;
			perMinute = (float)(tweetCount / (float)minuteDenominator);
			perHour = (float)(tweetCount / (float)hourDenominator);

			// Assert
			Assert.AreNotEqual(0, sampleTimeInSeconds);
			Console.WriteLine("CalculateTwitterRates_Foat_Test ...");
			Console.WriteLine($"tweets: " +
				$"{tweetCount}, sampleTime seconds: {sampleTimeInSeconds}, " +
				$"perSecond: {perSecond}, perMinute: {perMinute}, perHour: {perHour}");

			// Assert
		}

		[Test]
		public void ReadStreamFromFile_Test()
		{
			// Arrange
			var filename = this.StreamFileName;
			var fullpath = TestContext.CurrentContext.TestDirectory;
			List<TweetV2> items;

			// Act
			var file = System.IO.File.OpenRead($"{fullpath}/{filename}");
			using (StreamReader r = new StreamReader($"{fullpath}/{filename}"))
			{
				string json = r.ReadToEnd();
				items = JsonConvert.DeserializeObject<List<TweetV2>>(json);
				foreach (var item in items)
				{
					var isPersisted = this.Repository.PersistTweet(item);
				}
			}

			// Assert
			Assert.AreNotEqual(0, items.Count);
			Assert.IsNotNull(items);
		}

		[Test]
		public void PersistTweets_Test()
		{
			// Arrange
			var filename = this.StreamFileName;
			var path = TestContext.CurrentContext.TestDirectory;

			// Act
			var serializedTweets = this._ReadStreamFromFile($"{path}/{filename}");
			var items = JsonConvert.DeserializeObject<List<TweetV2>>(serializedTweets);
			foreach (var item in items)
			{
				this.TweetList.Add(item);
			}

			// Assert
			Assert.IsNotNull(this.TweetList);
		}

		[Test]
		public void GetUrlInfo_Test()
		{
			// Arrange
			var filename = this.StreamFileName;
			var path = TestContext.CurrentContext.TestDirectory;
			int picUrlCount = 0;
			float percentUrlsWithPic;
			int urlCount = 0;

			// Act
			this.TweetList = new List<TweetV2>();
			var serializedTweets = this._ReadStreamFromFile($"{path}/{filename}");
			if (serializedTweets == null)
			{
				return;
			}
			var items = JsonConvert.DeserializeObject<List<TweetV2>>(serializedTweets);
			foreach (var item in items)
			{
				this.TweetList.Add(item);
			}

			var urls = this.TweetList.Where(p => p.Entities != null && p.Entities.Urls != null).Select(p => p.Entities.Urls).ToList();
			urlCount = urls.Count;
			foreach (var url in urls)
			{
				foreach (var u in url)
				{
					if (u.DisplayUrl.ToLower().Contains("pic.twitter") || u.DisplayUrl.ToLower().Contains("instagram"))
					{
						++picUrlCount;
					}
					Console.WriteLine($"Display: {u.DisplayUrl}, Expanded: {u.ExpandedUrl}, Unwound: {u.UnwoundUrl}, Url: {u.Url}");
				}
			}
			percentUrlsWithPic = (float)picUrlCount / urlCount;
			Console.WriteLine($"Total # of tweets with URL: {urlCount}, # with pic/instagram: {picUrlCount}, percent with pic/instagram: {percentUrlsWithPic}");

			// Assert
			Assert.IsNotNull(items);
			Assert.IsNotNull(serializedTweets);
			Assert.IsNotNull(filename);
			Assert.AreEqual(path, TestContext.CurrentContext.TestDirectory);
		}

		[Test]
		public void GetUrlCount_Test()
		{
			// Arrange
			// Act
			// Assert
		}

		[Test]
		public void GetEmojiInfo_Test()
		{
			// Arrange
			var filename = this.StreamFileName;
			var path = TestContext.CurrentContext.TestDirectory;
			int emojiCount = 0;

			// Act
			this.TweetList = new List<TweetV2>();
			var serializedTweets = this._ReadStreamFromFile($"{path}/{filename}");
			if (serializedTweets == null)
			{
				return;
			}
			var items = JsonConvert.DeserializeObject<List<TweetV2>>(serializedTweets);
			foreach (var item in items)
			{
				this.TweetList.Add(item);
			}

			// TODO: find emojis ...
			//var urls = this.TweetList.Where(p => p.Entities != null && p.Entities. != null).Select(p => p.Entities).ToList();
			//emojiCount = urls.Count;
		}

		[Test]
		public void WriteSerializedTweetsToFile_Test()
		{
			// Arrange
			var filename = this.StreamFileName;
			var path = TestContext.CurrentContext.TestDirectory;

			// Act
			this.TweetList = new List<TweetV2>();
			var serializedTweets = this._ReadStreamFromFile($"{path}/{filename}");

			this.TweetList = JsonConvert.DeserializeObject<List<TweetV2>>(serializedTweets);


			var outputFilename = $"{this.StreamFileName}.out.json";
			if (System.IO.File.Exists(outputFilename))
			{
				System.IO.File.Delete(outputFilename);
			}
			var fileStream = System.IO.File.OpenWrite(outputFilename);
			using (StreamWriter sw = new StreamWriter(fileStream))
			{
				if (this.TweetList.Count > 1)
				{
					sw.WriteLine("[");
				}
				foreach (var tweet in this.TweetList)
				{
					var serializedTweet = JsonConvert.SerializeObject(tweet);
					sw.WriteLine($"{serializedTweet},");
				}
				if (this.TweetList.Count > 1)
				{
					sw.WriteLine("]");
				}
			}

			// Assert
		}

		public string _ReadStreamFromFile(string fullPath)
		{
			var filename = this.StreamFileName;
			string json = string.Empty;
			var file = System.IO.File.OpenRead($"{fullPath}");
			using (StreamReader r = new StreamReader($"{fullPath}"))
			{
				json = r.ReadToEnd();
			}

			return json;
		}

		#endregion
	}
}