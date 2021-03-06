﻿using System;
using System.Text;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Nancy;
using Nancy.Hosting.Self;
using Nancy.ViewEngines.Razor;
using RazorEngine;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Core;
//This code uses Nancy micro framework for hosting and Razor for templating and Mongodb C# driver for Mongo quieries.
//Developed for viewing user statistics of the backflipt application.
//Written in August 2016 by Hithesh

namespace Backflipt_stats
{
    public class Program
    {
        //Hosts,starts and stops the nancy server using the default bootstrap
        static void Main(string[] args)
        {
            HostConfiguration hostConfigs = new HostConfiguration();
            hostConfigs.UrlReservations.CreateAutomatically = true;
            var nancyHost = new Nancy.Hosting.Self.NancyHost(new Uri("http://localhost:9664/"), new DefaultNancyBootstrapper(), hostConfigs);
            nancyHost.Start();
            Console.WriteLine("The Backflipt statistics server is running now!!" + DateTime.Now);
            Console.ReadLine();
            nancyHost.Stop();
        }
    }
    // This module is responsible for all routing and content display on the site.
    public class IndexModule : NancyModule
    {
        private static string connectionstring = "mongodb://oodlyadmin:ZHS4CHOE2JPUK9EEOJSKI6Q8UUYTG1G38URQ4XUH@candidate.5.mongolayer.com:10613,candidate.6.mongolayer.com:10495/Oodly-classic-new";
        private static MongoClient client = new MongoClient(connectionstring);
        private static MongoDB.Driver.IMongoDatabase database = client.GetDatabase("Oodly-classic-new");
        
        public IndexModule()
        {
            //The get and post methods for the main page.
            Get["/"] = parameters =>
            {
                var collection1 = database.GetCollection<BsonDocument>("users");
                var filter = Builders<BsonDocument>.Filter.Ne("emailaccounts", "");
                var result = collection1.Find(filter).ToList();
                var result2 = collection1.Distinct<BsonArray>("emailaccounts", filter);
                List<BsonDocument> l = new List<BsonDocument>();
                var p = "<html>";
                foreach(var x in result)
                {
                    //Console.WriteLine(x);
                    p = p + "<p>" + x.ToString() + "</p>";
                    l.Add(x);
                }
                p = p + "</html>";
                Console.WriteLine(p + "!!!!");
                return p;
            };

            Post["/"] = parameters =>
            {
                var body = Request.Body.ToString();
                Console.WriteLine(body);
                return HttpStatusCode.OK;
            };
            //Get["/hello/{name}"] = parameters => { return "Hello " + parameters.name; };
        }
    }
}
