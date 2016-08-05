using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;
using Nancy.Hosting.Self;
using Nancy;
using System; 

namespace Backflipt_stats
{
    public class Program
    {
        static void Main(string[] args)
        {
            HostConfiguration hostConfigs = new HostConfiguration();
            hostConfigs.UrlReservations.CreateAutomatically = true;
            var nancyHost = new Nancy.Hosting.Self.NancyHost(new Uri("http://localhost:9664"), new DefaultNancyBootstrapper(), hostConfigs);
            nancyHost.Start();
            Console.WriteLine("Web server running !!!");
            Console.ReadLine();
            nancyHost.Stop();
        }
    }
    public class HelloModule : NancyModule
    {
        public HelloModule()
        {
            //KeyValuePair<dynamic, dynamic> kvp = new KeyValuePair<dynamic, dynamic>();
            Get["/"] = parameters => "Hello World Testing get";
            Post["/"] = parameters => "Hello World testing Post!!!";
        }
    }
}
