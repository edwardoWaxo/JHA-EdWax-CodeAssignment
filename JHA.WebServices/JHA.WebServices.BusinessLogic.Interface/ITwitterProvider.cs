using System.Threading.Tasks;
using JHA.WebServices.Contract.Twitter;
using Tweetinvi;

namespace JHA.WebServices.BusinessLogic.Interface
{
	/// <summary>
	/// This is the TwitterProvider interface.
	/// </summary>
	public interface ITwitterProvider
    {
		/// <summary>
		/// Consumes the (sample) stream.
		/// </summary>
		/// <param name="sampeStream">The sampe stream.</param>
		/// <returns></returns>
		Task ConsumeStream(Tweetinvi.Streaming.V2.ISampleStreamV2 sampeStream);

		/// <summary>
		/// Creates the and consume stream.
		/// </summary>
		/// <param name="config">The configuration.</param>
		/// <returns></returns>
		Task CreateAndConsumeStream(TwitterConfiguration config);

		/// <summary>
		/// Creates the stream.
		/// </summary>
		/// <param name="client">The client.</param>
		/// <returns></returns>
		Tweetinvi.Streaming.V2.ISampleStreamV2 CreateStream(TwitterClient client);

		/// <summary>
		/// Gets the configuration.
		/// </summary>
		/// <returns></returns>
		TwitterConfiguration GetConfiguration();

		/// <summary>
		/// Gets the configuration.
		/// </summary>
		/// <param name="config">The configuration.</param>
		/// <returns></returns>
		TwitterConfiguration GetConfiguration(TwitterConfiguration config);

		/// <summary>
		/// Gets the statistics.
		/// </summary>
		/// <returns></returns>
		TwitterStatistics GetStatistics();

		/// <summary>
		/// Persists the tweet.
		/// </summary>
		/// <param name="tweet">The tweet.</param>
		/// <returns></returns>
		bool PersistTweet(Tweetinvi.Models.V2.TweetV2 tweet);
    }
}