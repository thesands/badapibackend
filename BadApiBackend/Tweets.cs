using System.Collections.Generic;
using System.Linq;

namespace BadApiBackend
{
    public static class Tweets
    {
        private static Dictionary<string, Tweet> tweetMap;

        static Tweets()
        {
            tweetMap = new Dictionary<string, Tweet>();
        }

        public static void AddTweet(Tweet t)
        {
            tweetMap.TryAdd(t.Id, t);
        }

        public static List<Tweet> RetrieveAllTweets()
        {
            return tweetMap.Values.ToList();
        }
    }
}