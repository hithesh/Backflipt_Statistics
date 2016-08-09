using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Core;
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
            Console.WriteLine("Query results end here!!!");
            Console.ReadLine();
            nancyHost.Stop();
        }
        public static async Task testing()
        {
            //Console.WriteLine("heyyyy working !!");
            //var collection = database.GetCollection<BsonDocument>("restaurants");
            //collection.InsertOne();
            var filter = Builders<BsonDocument>.Filter.Ne("grades", "");
            var filter2 = Builders<BsonDocument>.Filter.Eq("cuisine", "Bakery");
            //var result = await collection.Find(filter).ToListAsync();
            FieldDefinition<BsonDocument> field = "cuisine";
            //var x = collection.Distinct(field, filter);
            //Console.WriteLine(collection.Distinct(""));
        }
    }
    public class HelloModule : NancyModule
    {
        private static string connectionstring = "mongodb://oodlyadmin:ZHS4CHOE2JPUK9EEOJSKI6Q8UUYTG1G38URQ4XUH@candidate.5.mongolayer.com:10613,candidate.6.mongolayer.com:10495/Oodly-classic-new";
        private static MongoClient client = new MongoClient(connectionstring);
        private static MongoDB.Driver.IMongoDatabase database = client.GetDatabase("Oodly-classic-new");

        public HelloModule()
        { 

            //KeyValuePair<dynamic, dynamic> kvp = new KeyValuePair<dynamic, dynamic>();
            Get["/"] = parameters => {
                List<BsonDocument> l = new List<BsonDocument>();
                var p = "<html>";
                foreach (var x in database.ListCollections().ToList())
                {
                    p = p + x.ToString();
                    l.Add(x);
                }
                //var p = "<html>"+l[0].ToString()+l[1].ToString()+"</html>";
                p = p + "</html>";
                return p;
            };
            Get["/hello/{name}"] = parameters => { return "Hello " + parameters.name; };
            Post["/"] = parameters => {
                var body = Request.Body.ToString();
                Console.WriteLine(body);
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

