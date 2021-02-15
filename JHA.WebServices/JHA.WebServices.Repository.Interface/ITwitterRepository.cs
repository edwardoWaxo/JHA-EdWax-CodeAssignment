using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JHA.WebServices.Contract.Twitter;

namespace JHA.WebServices.Repository.Interface
{
    public interface ITwitterRepository
    {
        TwitterStatistics GetTwitterStatistics();
        bool PersistTweet(Tweetinvi.Models.V2.TweetV2 tweet);

    }
}