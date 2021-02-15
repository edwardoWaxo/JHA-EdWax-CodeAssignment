using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JHA.WebServices.Contract.Twitter;
using JHA.WebServices.Contract;
using Tweetinvi.Models.V2;

namespace JHA.WebServices.Repository.InMemory
{

	public class TwitterResults
	{
		private static readonly TwitterResults instance = new TwitterResults();

		#region Constructor(s)


		// Explicit static constructor to tell C# compiler not to mark as beforefieldinit
		static TwitterResults()
		{
		}

		private TwitterResults()
		{
		}

		public static TwitterResults Instance
		{
			get
			{
				return instance;
			}
		}

		#endregion

		#region Public Properties

		public List<TweetV2> TweetList { get; set; }

		public List<TweetV2> SnapshotTweetList { get; set; }

		public DateTime StreamStartTime { get; set; }

		#endregion

		#region  Costants

		public const int HoursAsSeconds = (60 * 60);
		public const int MinutesAsSeconds = 60;

		#endregion

		#region Public Methods

		public void GetHashTags(ref TwitterStatistics stats)
		{
			var hashtags = this.SnapshotTweetList.Where(p => p.Entities != null && p.Entities.Hashtags != null).Select(p => p.Entities.Hashtags);
			stats.HashTags = hashtags.Take(10);
			return;
		}

		public void GetTweetCount(ref TwitterStatistics stats, JHA.WebServices.Contract.Enums.TwitterCountType countType)
		{
			switch (countType)
			{
				case JHA.WebServices.Contract.Enums.TwitterCountType.PreReportingCount:
					stats.TweetCount = this.SnapshotTweetList.Count;
					break;
				case JHA.WebServices.Contract.Enums.TwitterCountType.PostReportingCount:
					var tweets = this.TweetList.Count;
					stats.TeetsSinceReporting = tweets - stats.TweetCount;
					break;
				default:
					break;
			}
		}

		public void GetTweetRates(ref TwitterStatistics stats)
		{
			float perSecond;
			float perMinute;
			float perHour;

			// Normalize the run time into seconds.
			stats.FromDateTime = this.StreamStartTime;
			stats.ToDateTime = DateTime.UtcNow;
			var runTime = stats.ToDateTime - stats.FromDateTime;
			stats.SampleTimeInSeconds =
				runTime.Seconds +
				(runTime.Minutes * MinutesAsSeconds) +
				(runTime.Hours * HoursAsSeconds);

			int sampleTimeInSeconds = stats.SampleTimeInSeconds;

			float minuteDenominator = (float)(sampleTimeInSeconds / (float)MinutesAsSeconds);
			float hourDenominator = (float)(sampleTimeInSeconds / (float)HoursAsSeconds);

			perSecond = sampleTimeInSeconds > 0 ? (float)stats.TweetCount / sampleTimeInSeconds : 0;
			perMinute = (float)(stats.TweetCount / (float)minuteDenominator);
			perHour = (float)(stats.TweetCount / (float)hourDenominator);

			stats.TweetsPerSecond = perSecond;
			stats.TweetsPerMinute = perMinute;
			stats.TweetsPerHour = perHour;
		}

		/// <summary>
		/// Gets the twitter statistics.
		/// </summary>
		/// <returns></returns>
		public TwitterStatistics GetTwitterStatistics()
		{
			var stats = new TwitterStatistics();
			if (this.TweetList == null)
			{
				return stats;
			}
			
			// Take a snapshot of the current TweetList and report against those.
			this.SnapshotTweetList = this.TweetList.Take(this.TweetList.Count).ToList();

			// Tweets - total and rates ...
			this.GetTweetCount(ref stats, Contract.Enums.TwitterCountType.PreReportingCount);
			this.GetTweetRates(ref stats);  

			// URL info ...
			this.GetUrlCount(ref stats);

			// HashTag info ...
			this.GetHashTags(ref stats);

			//var entities = this.TweetList.Where(p => p.Entities != null).Select(p => p.Entities);
			//var urls = entities.Where(p => p.Urls != null).Select(p => p.Urls);
			//var hashTags = entities.Where(p => p.Hashtags != null).Select(p => p.Hashtags); // moved

			// Get the number of tweets that have arrived since we took our snapshot ...
			this.GetTweetCount(ref stats, Contract.Enums.TwitterCountType.PostReportingCount);

			this.SnapshotTweetList.Clear();
			return stats;
		}

		public void GetUrlCount(ref TwitterStatistics stats)
		{
			
			var entities = this.SnapshotTweetList.Where(p => p.Entities != null).Select(p => p.Entities);
			var urls = entities.Where(p => p.Urls != null).Select(p => p.Urls);
			stats.TweetsWithUrlCount = urls.Count();
		}

		/// <summary>
		/// Persists the tweet to our in-memory mechanism.
		/// </summary>
		/// <param name="tweet">The tweet.</param>
		/// <returns></returns>
		public bool PersistTweet(TweetV2 tweet)
		{
			var isPersisted = false;
			if (this.TweetList == null)
			{
				this.TweetList = new List<TweetV2>();
				this.StreamStartTime = DateTime.UtcNow;
			}

			try
			{
				this.TweetList.Add(tweet);
				if (this.TweetList.Count % 1000 == 0)
				{
					Console.WriteLine($"We have received {this.TweetList.Count} tweets");
				}
				isPersisted = true;
			}
			catch (Exception e)
			{
			}

			return isPersisted;
		}

		#endregion
	}
}