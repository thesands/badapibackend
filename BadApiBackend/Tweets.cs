using System;
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
            List<Tweet> tweetList = tweetMap.Values.ToList();
            tweetList.OrderByDescending(x => DateTime.Parse(x.Stamp));
            return tweetList;
        }
    }
}