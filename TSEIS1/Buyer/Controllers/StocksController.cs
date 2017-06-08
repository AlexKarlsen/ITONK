using Microsoft.ServiceFabric.Services.Client;
using System.Collections.Generic;
using System.Fabric;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Web.Http;

namespace Buyer.Controllers
{
    [ServiceRequestActionFilter]
    public class StocksController : ApiController
    {
        // Used for health checks.
        public static long _requestCount = 0L;

        // GET api/values 
        [HttpGet]
        [Route("api/stocks")]
        public List<KeyValuePair<string, int>> Get()
        {
            //return new string[] { "value1", "value2" };

            ServiceEventSource.Current.ServiceRequestStart("Get");
            Interlocked.Increment(ref _requestCount);
            
            List<KeyValuePair<string, int>> stocks = new List<KeyValuePair<string, int>>();
            stocks.Add(new KeyValuePair<string, int>("hej", 1));
            
            ServiceEventSource.Current.ServiceRequestStop("Get");
            return stocks;
        }

        // GET api/values/5 
        [HttpGet]
        [Route("api/stocks/{id}")]
        public string Get(int id)
        {
            return "value";
        }

        HttpClient _client = new HttpClient();

        // POST api/values 
        [System.Web.Http.HttpPost]
        [Route("api/{value}")]
        public async void Post([FromUri]string value)
        {
            var firstDelimiter = value.IndexOf(';');
            var lastDelimiter = value.LastIndexOf(';');

            var username = value.Substring(0, firstDelimiter);
            var stockname = value.Substring(firstDelimiter + 1, value.Length - (firstDelimiter + 1));
            stockname = stockname.Substring(0, stockname.IndexOf(';'));

            var amount = value.Substring(lastDelimiter + 1, value.Length - (lastDelimiter + 1));

            int amountInt;
            if (int.TryParse(amount, out amountInt))
            {
                //Buyer.AddBidOnMatchingService(username, stockname, amountInt);
                ServicePartitionResolver resolver = ServicePartitionResolver.GetDefault();

                ResolvedServicePartition partition = await resolver.ResolveAsync(new System.Uri("fabric:/TSEIS1/Matcher"), new ServicePartitionKey(0), CancellationToken.None);

                // Converts the partition from JSON and gets the url
                var endpoints = Newtonsoft.Json.Linq.JObject.Parse(partition.GetEndpoint().Address)["Endpoints"];
                var url = endpoints[""].ToString();
                var url1 = url.Substring(0,23);

                // VIrker ikke 

                string urlReverseProxy = $"http://localhost:19081/TSEIS1/Matcher1/api/{value}?PartitionKey=0&PartitionKind=Int64Range";
                HttpResponseMessage msg = await _client.PostAsync(urlReverseProxy, null).ConfigureAwait(false);

                string urlReverseProxy1 = $"http://localhost:19081/TSEIS1/VotingState/api/{value}?PartitionKey=0&PartitionKind=Int64Range";
                HttpResponseMessage msg1 = await _client.PostAsync(urlReverseProxy1, null).ConfigureAwait(false);
            }
        }

        // PUT api/values/5 
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5 
        public void Delete(int id)
        {
        }

        [HttpGet]
        [Route("api/index.html")]
        public HttpResponseMessage GetFile()
        {
            string file = "index.html";

            //string activityId = Guid.NewGuid().ToString();
            ServiceEventSource.Current.ServiceRequestStart("StocksController.GetFile");

            string response = null;
            string responseType = "text/html";
            Interlocked.Increment(ref _requestCount);
            // Validate file name.
            if ("index.html" == file)
            {
                //string path = string.Format(@"..\VotingServicePkg.Code.1.0.0\{0}", file);
                // Getting index.html dynamicly from fabrics runtime
                string path = Path.Combine(FabricRuntime.GetActivationContext().GetCodePackageObject("Code").Path, "index.html");
                response = File.ReadAllText(path);
            }

            ServiceEventSource.Current.ServiceRequestStop("StocksController.GetFile");

            if (null != response)
                return Request.CreateResponse(HttpStatusCode.OK, response, responseType);
            else
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, "File");
        }
    }
}
