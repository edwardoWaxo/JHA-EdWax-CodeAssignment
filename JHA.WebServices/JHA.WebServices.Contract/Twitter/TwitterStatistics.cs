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

		public int TweetCount { get; set; }
		public int TeetsSinceReporting { get; set; }

		public int TweetsWithEmojiCount { get; set; }

		public int TweetsWithUrlCount { get; set; }

		public int TweetsWithPhotoUrlCount { get; set; }

		public float TweetsPerHour { get; set; }

		public float TweetsPerMinute { get; set; }

		public float TweetsPerSecond { get; set; }

		/// <summary>
		/// This is the date/time stamp of the first (From) tweet.
		/// </summmary>
		public DateTime FromDateTime { get; set; }

		/// <summary>
		/// This is the date/time stamp of the last (To) tweet.
		/// </summary>ary>
		public DateTime ToDateTime { get; set; }

		public int SampleTimeInSeconds { get; set; }

		public ICollection<string> EmojiList { get; set; }

		public IEnumerable<Tweetinvi.Models.V2.HashtagV2[]> HashTags { get; set; }
		public ICollection<string> UrlList { get; set; }

		#endregion

		public override string ToString()
		{
			var sb = new StringBuilder();
			sb.AppendLine("");
			sb.AppendLine($"         Total tweet count: {this.TweetCount}");
			sb.AppendLine($"   Tweets with emoji count: {this.TweetsWithEmojiCount}");
			sb.AppendLine($"     Tweets with url count: {this.TweetsWithUrlCount}");
			sb.AppendLine($"Tweet with photo url count: {this.TweetsWithPhotoUrlCount}");
			sb.AppendLine($"            From datetimet: {this.FromDateTime}");
			sb.AppendLine($"               To datetime: {this.ToDateTime}");

			return sb.ToString();
		}
	}
}