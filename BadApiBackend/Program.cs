using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;

namespace BadApiBackend
{
    public class Program
    {
        public static void Main(string[] args)
        {
            GetTweets();
            BuildWebHost(args).Run();
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .Build();

        public static string CallApi(string baseUrl, DateTime start, DateTime end)
        {
            string url = baseUrl + start + "&endDate=" + end;
            HttpWebRequest myReq = 
                (HttpWebRequest)WebRequest.Create(url);
            var response = (HttpWebResponse)myReq.GetResponse();
            var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();
            
            return responseString;
        }

        public static JArray GetChunk(DateTime startDate, DateTime endDate)
        {
            var baseUrl = "https://badapi.iqvia.io/api/v1/Tweets?startDate=";
            var tweets = JArray.Parse(CallApi(baseUrl, startDate, endDate));
//            Console.WriteLine("Count: " + tweets.Count);

            if (tweets.Count == 100) Console.WriteLine("Count === 100, start: " + startDate + " end: " + endDate);
//            
//            var duration = (endDate.Ticks - startDate.Ticks) / 2;
//            endDate = endDate.Subtract(new TimeSpan(duration));
//            Console.WriteLine("new endDate: " + endDate);
//            GetChunk(startDate, endDate);

            return tweets;
        }
        
        public static void GetTweets()
        {
            DateTime startDate = new DateTime(2016, 1, 1, 0, 0, 0);
            DateTime endDate = startDate.AddDays(4);
            DateTime finishDate = new DateTime(2017, 12, 31, 11, 59, 59);

            while (endDate < finishDate)
            {
                var tweets = GetChunk(startDate, endDate);
            
                foreach (var tweet in tweets.Children())
                {
                    var tweetProperties = tweet.Children<JProperty>();
                    string tweetId = tweetProperties.FirstOrDefault(x => x.Name == "id").Value.ToString();
                    string tweetStamp = tweetProperties.FirstOrDefault(x => x.Name == "stamp").Value.ToString();
                    string tweetText = tweetProperties.FirstOrDefault(x => x.Name == "text").Value.ToString();
                
                    Tweet t = new Tweet(tweetId, tweetStamp, tweetText);
                
                    Tweets.AddTweet(t);
                }

                endDate = endDate.AddDays(4);
                startDate = startDate.AddDays(4);
            }            
        }
    }
}