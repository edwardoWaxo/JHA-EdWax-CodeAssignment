using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JHA.WebServices.Contract.Twitter;
using Tweetinvi.Models.V2;

namespace JHA.WebServices.Repository.Interface
{
	public interface ITwitterResults
	{
		void GetEmojiCount(ref TwitterStatistics stats);
		void GetEmojiInfo(ref TwitterStatistics stats);
		void GetHashTags(ref TwitterStatistics stats);
		void GetTweetCount(ref TwitterStatistics stats, JHA.WebServices.Contract.Enums.TwitterCountType countType);
		void GetTweetRates(ref TwitterStatistics stats);
		TwitterStatistics GetTwitterStatistics();
		void GetUrlCount(ref TwitterStatistics stats);
		void GetUrlInfo(ref TwitterStatistics stats);
		bool PersistTweet(TweetV2 tweet);
	}
}
