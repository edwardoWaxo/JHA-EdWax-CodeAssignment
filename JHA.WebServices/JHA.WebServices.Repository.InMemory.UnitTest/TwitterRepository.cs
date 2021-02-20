using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JHA.WebServices.Repository.Interface;
using JHA.WebServices.Contract.Twitter;
using Tweetinvi.Models.V2;

namespace JHA.WebServices.Repository.InMemory.UnitTest
{
	public class TwitterRepository : ITwitterRepository
	{
		#region Constructor(s)

		public TwitterRepository()
		{
			this.TwitterResults = TwitterResults.Instance;
		}

		#endregion

		#region Public Properties

		//public List<TweetV2> TweetList { get; set; }
		public TwitterResults TwitterResults { get; set; }

		#endregion

		#region Public Methods

		public TwitterStatistics GetTwitterStatistics()
		{
			throw new NotImplementedException();
		}

		public bool PersistTweet(TweetV2 tweet)
		{
			return this.TwitterResults.PersistTweet(tweet);
		}

		#endregion
	}
}