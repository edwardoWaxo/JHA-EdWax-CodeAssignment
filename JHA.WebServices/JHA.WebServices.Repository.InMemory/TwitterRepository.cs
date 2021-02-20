using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JHA.WebServices.Repository.Interface;
using JHA.WebServices.Contract.Twitter;
using Tweetinvi.Models.V2;

namespace JHA.WebServices.Repository.InMemory
{
	/// <summary>
	/// The InMemory implementation of our TwitterRepository.
	/// In order to keep the tweet data memory resident, we need to add another class to follow the Singleton pattern.
	/// </summary>
	/// <seealso cref="JHA.WebServices.Repository.Interface.ITwitterRepository" />
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
			try
			{
				return this.TwitterResults.GetTwitterStatistics();
			}
			catch (Exception e)
			{
			}
			return new TwitterStatistics();
		}

		public bool PersistTweet(TweetV2 tweet)
		{
			try
			{
				return this.TwitterResults.PersistTweet(tweet);
			}
			catch (Exception e)
			{ 
			}
			return false;
		}

		#endregion
	}
}