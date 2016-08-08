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
            var nancyHost = new Nancy.Hosting.Self.NancyHost(new Uri("http://localhost:9664/"), new DefaultNancyBootstrapper(), hostConfigs);
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
            Get["/new"] = paramaeters => "Hello world new!!";
            Post["/"] = parameters => {
                return HttpStatusCode.OK;  //"Hello World testing Post!!!"
                };

            /*Post["/test"] = parameters =>
            {
                //ServicePointManager.ServerCertificateValidationCallback = EwsXenLib.security.CertificateValidationCallBack;
                Dictionary<string, string> result = new Dictionary<string, string>();
                Tuple<string, string> logindetails = new Tuple<string, string>("", "");
                try
                {
                    logindetails = login(this.Request.Form);
                }
                catch (Exception e)
                {
                    EwsXenLib.Program.logger.Info(e.Message);
                }
                EwsXenLib.Program.logger.Info("");
                if (logindetails.Item1 != "")
                {
                    result.Add("provider", logindetails.Item1);
                    result.Add("server", logindetails.Item2);
                    result.Add("status", "ok");
                }
                else
                {
                    result.Add("status", "error");
                    result.Add("error", "Authentication Failed");
                }
                return Response.AsJson(result);
            };*/
        }
    }
}

