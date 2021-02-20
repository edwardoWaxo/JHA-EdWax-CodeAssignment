using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tweetinvi.Models.V2;

namespace JHA.WebServices.Contract.Twitter
{
	public class TwitterStatistics
	{
		#region Constructor(s)
		
		public TwitterStatistics()
		{
			this.EmojiList = new List<string>();
			this.UrlList = new List<string>();
		}

		#endregion

		#region Public Properties

		/// <summary>
		/// Gets or sets the tweet count.
		/// </summary
		public int TweetCount { get; set; }

		/// <summary>
		/// Gets or sets the tweets since reporting.
		/// </summary>
		public int TweetsSinceReporting { get; set; }

		/// <summary>
		/// Gets or sets the tweets with emoji count.
		/// </summary>
		public int TweetsWithEmojiCount { get; set; }

		/// <summary>
		/// Gets or sets the percent tweets with emoji.
		/// </summary>
		public float PercentTweetsWithEmoji { get; set; }

		/// <summary>
		/// Gets or sets the tweets with URL count.
		/// </summary>
		public int TweetsWithUrlCount { get; set; }

		/// <summary>
		/// Gets or sets the percent tweets with URL.
		/// </summary>
		public float PercentTweetsWithUrl { get; set; }

		/// <summary>
		/// Gets or sets the tweets with photo URL count.
		/// </summary>
		public int TweetsWithPhotoUrlCount { get; set; }

		/// <summary>
		/// Gets or sets the percent tweets with photo URL.
		/// </summary>
		public float PercentTweetsWithPhotoUrl { get; set; }

		/// <summary>
		/// Gets or sets the tweets per hour.
		/// </summary>
		public float TweetsPerHour { get; set; }

		/// <summary>
		/// Gets or sets the tweets per minute.
		/// </summary>
		/// <value>
		public float TweetsPerMinute { get; set; }

		/// <summary>
		/// Gets or sets the tweets per second.
		/// </summary>
		public float TweetsPerSecond { get; set; }

		/// <summary>
		/// This is the date/time stamp of the first (From) tweet.
		/// </summmary>
		public DateTime FromDateTime { get; set; }

		/// <summary>
		/// This is the date/time stamp of the last (To) tweet.
		/// </summary>ary>
		public DateTime ToDateTime { get; set; }

		/// <summary>
		/// Gets or sets the sample time in seconds.
		/// </summary>
		public int SampleTimeInSeconds { get; set; }

		public ICollection<string> EmojiList { get; set; }

		public IEnumerable<Tweetinvi.Models.V2.HashtagV2[]> HashTags { get; set; }
		public ICollection<string> UrlList { get; set; }

		#endregion

		public override string ToString()
		{
			var sb = new StringBuilder();
			sb.AppendLine("");
			sb.AppendLine($"     Total number of tweets: {this.TweetCount}");
			sb.AppendLine($"    Tweets with emoji count: {this.TweetsWithEmojiCount}");
			sb.AppendLine($"      Tweets with url count: {this.TweetsWithUrlCount}");
			sb.AppendLine($"Tweets with photo url count: {this.TweetsWithPhotoUrlCount}");
			sb.AppendLine($"   Percent tweets w/pic url: {this.PercentTweetsWithPhotoUrl}");
			sb.AppendLine($"             From datetimet: {this.FromDateTime}");
			sb.AppendLine($"                To datetime: {this.ToDateTime}");

			return sb.ToString();
		}
	}
}