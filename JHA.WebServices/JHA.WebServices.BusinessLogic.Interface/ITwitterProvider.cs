using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JHA.WebServices.Contract;
using JHA.WebServices.Contract.Twitter;

namespace JHA.WebServices.BusinessLogic.Interface
{
	/// <summary>
	/// This is the TwitterProvider interface.
	/// </summary>
	public interface ITwitterProvider
    {
		/// <summary>
		/// Gets the configuration.
		/// </summary>
		/// <returns></returns>
		TwitterConfiguration GetConfiguration();

		Task CreateAndConsumeStream();

		bool PersistTweet(Tweetinvi.Models.V2.TweetV2 tweet);

		TwitterStatistics GetStatistics();
    }
}