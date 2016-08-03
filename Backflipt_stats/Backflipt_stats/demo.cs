using Nancy;
using System;
using Nancy.Hosting.Self;
using EwsXenLib;
using Microsoft.Exchange.WebServices.Data;
using System.Net;
using System.Collections.Generic;

// http://www.topwcftutorials.net/2013/09/simple-steps-for-restful-service.html
// http://www.picnet.com.au/blogs/guido/post/2011/03/22/understanding-nancy-sinatra-for-net/
// http://stackoverflow.com/questions/7185369/nancy-self-host-doesnt-call-module
// \
namespace AuthServer
{
    public class Program
    {

        static void Main(string[] args)
        {
            HostConfiguration hostConfigs = new HostConfiguration();
            hostConfigs.UrlReservations.CreateAutomatically = true;
            var nancyHost = new Nancy.Hosting.Self.NancyHost(new Uri("http://localhost:9664"), new DefaultNancyBootstrapper(), hostConfigs);
            EwsXenLib.Oodly.loggerconfig();
            nancyHost.Start();
            Console.WriteLine("Web server running...");
            Console.ReadLine();


            nancyHost.Stop();
        }
    }


    public class MainModule : Nancy.NancyModule
    {
        public MainModule()
        {
            Post["/"] = parameters =>
            {
                ServicePointManager.ServerCertificateValidationCallback = EwsXenLib.security.CertificateValidationCallBack;
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
            };
        }


        public static Tuple<string, string> login(DynamicDictionary x)
        {
            ExchangeService service = new ExchangeService(ExchangeVersion.Exchange2007_SP1);

            Console.WriteLine(x["accountname"].ToString());

            string pwd = EwsXenLib.security.decrypt(x["password"]);

            bool logged = false;

            string logintype = "";
            if ((x["method"] == "basic") || (x["method"] == "exchange_ews_basic"))
            {
                logged = EwsXenLib.Program.Authenticate(x["accountname"], pwd, null, ref service);
                logintype = "exchange_ews_basic";
            }
            else if (x["method"] == "exchange_ews_without_autodiscovery")
            {
                service.Url = new Uri(x["server"]);
                logged = EwsXenLib.Program.Authenticate(x["accountname"], pwd, null, ref service);
                logintype = "exchange_ews_without_autodiscovery";
            }
            else if (x["method"] == "exchange_ews_advanced")
            {
                service.Url = new System.Uri(x["server"]);
                logged = EwsXenLib.Program.Authenticate(x["username"], pwd, x["domain"], ref service);
                logintype = "exchange_ews_advanced";
            }
            else if (x["method"] == "advanced")
            {
                service.Url = new System.Uri(x["server"]);
                logged = EwsXenLib.Program.Authenticate(x["accountname"], pwd, null, ref service);
                logintype = "exchange_ews_without_autodiscovery";
                if (!logged)
                {
                    logged = EwsXenLib.Program.Authenticate(x["username"], pwd, x["domain"], ref service);
                    logintype = "exchange_ews_advanced";
                }


            }
            string url = "";
            string method = "";
            if (logged == true)
            {
                method = logintype;
                url = service.Url.ToString();
            }
            return new Tuple<string, string>(method, url);

        }



    }
}
`