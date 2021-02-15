using System;
using System.Threading.Tasks;
using System.Linq;
using System.IO;
using Tweetinvi;
using Tweetinvi.Models;
using JHA.WebServices.Contract.Twitter;
using JHA.WebServices.BusinessLogic.Interface;
using JHA.WebServices.BusinessLogic;
using JHA.WebServices.Repository.Interface;
using JHA.WebServices.Repository.InMemory;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace DotNetCoreDemoApp
{
	class Program
	{
        const string JsonFileName = @"c:\temp\serializedTweets.json";

        static async Task Main(string[] args)
		{
            // Configuration info copies from WebServices project
            //
            const string streamUrl = "https://api.twitter.com/2/tweets/sample/stream";
            const string consumerKey = "3mPNFry9ukj3w8bdIA3GjI94i";
            const string consumerKeySecret = "rQUNxAPh0ba1FnVzknYAtJvExlTbJPfFqhbgfe9THFXPFy3Qu9";
            const string accessToken = "1359179418195939329-4xtDPJYewyh6sQAHG1kPTJVXrShT0m";
            const string accessTokenSecret = "ejFE8QdUggwhe4WishQVBNiV8y3j21qlf7D00t4c666dq";
            const string bearerToken = "AAAAAAAAAAAAAAAAAAAAAMpTMgEAAAAA6Y4%2FQRX4p2recuNoCVHlUQvRRts%3Dvr7KpF3JisYSBMJfUCXzcyXfQ2GGphu3WSJAYyHa2Yrw3zKtVI";
            
			var appCredentials = new Tweetinvi.Models.ConsumerOnlyCredentials(consumerKey, consumerKeySecret)
			{
				BearerToken = bearerToken
            };
			var appClient = new TwitterClient(appCredentials);

            if (File.Exists(JsonFileName))
			{
                File.Delete(JsonFileName);
			}

            var skipCount = 0;
            var takeCount = 500;
            var tweetList = new List<Tweetinvi.Models.V2.TweetV2>();
            var tweetStringList = new List<string>();
            var sampleStreamV2 = appClient.StreamsV2.CreateSampleStream();

            try
            {
                Console.WriteLine($"reading from stream ... ");
                sampleStreamV2.TweetReceived += (sender, args) =>
                {
                    var tweet = args.Tweet;
                    if (tweet != null)
                    {
                        tweetList.Add(tweet);
                        Console.WriteLine($"got another tweet ... count is {tweetList.Count}");
                        var tweetString = appClient.Json.Serialize<Tweetinvi.Models.V2.TweetV2>(tweet);
                        tweetStringList.Add(tweetString);
                        if (tweetStringList.Count % takeCount == 0)
                        {
                            var tweetsToSave = tweetList.Skip(skipCount).Take(takeCount).ToList();
                            skipCount += takeCount;
                            var saved = WriteTweetsToFile(tweetsToSave);
                            Console.WriteLine($"Tweet as JSON string: {tweetString}");
                        }
                    }
                };

                await sampleStreamV2.StartAsync();
            }
            catch (Exception e)
			{
                Console.WriteLine($"exception: Message= {e.Message}, stacktrace={e.StackTrace}");
			}


            Console.WriteLine($"we have {tweetList.Count} tweets!");
            Console.Write("Press [ENTER] to continue ...");
            var x = Console.ReadKey();

            return;
        }

        public static bool WriteTweetsToFile(List<Tweetinvi.Models.V2.TweetV2> tweets)
		{
            var json = JsonConvert.SerializeObject(tweets);
            File.AppendAllText(JsonFileName, json);
            
          
            return true;
		}
	}
}
