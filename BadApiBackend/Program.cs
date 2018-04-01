using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http.Features;
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
            string startStr = start.ToUniversalTime().ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'fff'Z'");
            string endStr = end.ToUniversalTime().ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'fff'Z'");
            
            string url = baseUrl + startStr + "&endDate=" + endStr;
            HttpWebRequest myReq = 
                (HttpWebRequest)WebRequest.Create(url);
            var response = (HttpWebResponse)myReq.GetResponse();
            var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();
            
            return responseString;
        }

        public static void GetAllTweets(DateTime startDate, DateTime endDate)
        {
            var baseUrl = "https://badapi.iqvia.io/api/v1/Tweets?startDate=";
            var tweets = JArray.Parse(CallApi(baseUrl, startDate, endDate));
            //Console.WriteLine("start: " + startDate + " end: " + endDate + " count: " + tweets.Count);

            if (tweets.Count == 100)
            {
                TimeSpan span = (endDate - startDate) / 2;
                DateTime newEnd = startDate.Add(span);
                
                GetAllTweets(startDate, newEnd);
                GetAllTweets(newEnd, endDate);
            }
            else
            {
                foreach (var tweet in tweets.Children())
                {
                    var tweetProperties = tweet.Children<JProperty>();
                    string tweetId = tweetProperties.FirstOrDefault(x => x.Name == "id").Value.ToString();
                    string tweetStamp = tweetProperties.FirstOrDefault(x => x.Name == "stamp").Value.ToString();
                    string tweetText = tweetProperties.FirstOrDefault(x => x.Name == "text").Value.ToString();
                
                    Tweet t = new Tweet(tweetId, tweetStamp, tweetText);
                
                    Tweets.AddTweet(t);
                }
            }
        }
        
        public static void GetTweets()
        {
            DateTime startDate = new DateTime(2016, 1, 1, 0, 0, 0);
            DateTime endDate = new DateTime(2018, 1, 1, 0, 0, 0);

            GetAllTweets(startDate, endDate); 
        }
    }
}