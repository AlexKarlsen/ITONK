using System;
using System.Collections.Generic;
using System.Fabric;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.ServiceFabric.Services.Communication.Runtime;
using Microsoft.ServiceFabric.Services.Runtime;
using System.Net.Http;
using Common;

namespace Seller
{
    /// <summary>
    /// The FabricRuntime creates an instance of this class for each service type instance. 
    /// </summary>
    internal sealed class Seller : StatelessService
    {
        public Seller(StatelessServiceContext context)
            : base(context)
        { }

        public static async Task PlaceRequestAsync(Stock stock)
        {
            using (var _client = new HttpClient())
            {
                _client.BaseAddress = new System.Uri("http://localhost:19081");

                var jsonstring = Newtonsoft.Json.JsonConvert.SerializeObject(stock);

                var content = new StringContent(jsonstring, System.Text.Encoding.UTF8, "application/json");

                string urlReverseProxy = "/TSEIS1/Matcher3/api/values/";
                var response = await _client.PostAsync(urlReverseProxy, content);

                // We successfully communicated with votingstate, however, this can't be done without all the extra stuff...
                //string urlReverseProxy1 = $"http://localhost:19081/TSEIS1/VotingState/api/{value}?PartitionKey=0&PartitionKind=Int64Range";
                //HttpResponseMessage msg1 = await _client.PostAsync(urlReverseProxy1, null).ConfigureAwait(false);
            }
        }

        /// <summary>
        /// Optional override to create listeners (like tcp, http) for this service instance.
        /// </summary>
        /// <returns>The collection of listeners.</returns>
        protected override IEnumerable<ServiceInstanceListener> CreateServiceInstanceListeners()
        {
            return new ServiceInstanceListener[]
            {
                new ServiceInstanceListener(serviceContext => new OwinCommunicationListener(Startup.ConfigureApp, serviceContext, ServiceEventSource.Current, "ServiceEndpoint"))
            };
        }
    }
}
