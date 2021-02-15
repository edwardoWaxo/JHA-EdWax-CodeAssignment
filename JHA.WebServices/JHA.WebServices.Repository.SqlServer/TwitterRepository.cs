using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JHA.WebServices.Repository.Interface;
using JHA.WebServices.Contract;
using JHA.WebServices.Contract.Twitter;
using Tweetinvi.Models.V2;

namespace JHA.WebServices.Repository.SqlServer
{
	public class TwitterRepository : ITwitterRepository
	{
		#region Constructor(s)

		public TwitterRepository()
		{
			if (this.TweetList == null)
			{
				this.TweetList = new List<TweetV2>();
			}
		}

		#endregion

		#region Public Properties

		public List<TweetV2> TweetList { get; set; }

		#endregion

		#region Public Methods

		public TwitterStatistics GetTwitterStatistics()
		{
			throw new NotImplementedException();
		}

		public bool PersistTweet(TweetV2 tweet)
		{
			throw new NotImplementedException();
		}

		#endregion
	}
}