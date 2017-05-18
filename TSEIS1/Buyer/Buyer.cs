using System;
using System.Collections.Generic;
using System.Fabric;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.ServiceFabric.Services.Communication.Runtime;
using Microsoft.ServiceFabric.Services.Runtime;
using Microsoft.ServiceFabric.Services.Remoting.Client;

namespace Buyer
{
    /// <summary>
    /// The FabricRuntime creates an instance of this class for each service type instance. 
    /// </summary>
    internal sealed class Buyer : StatelessService
    {
        public Buyer(StatelessServiceContext context)
            : base(context)
        { }

        public static void AddBidOnMatchingService(string username, string stock, int amount)
        {
            var client = ServiceProxy.Create<Common.IStockBidService>(new Uri("fabric:/TSEIS1/Matcher"));
            w
            var stockbid = new Common.StockBid() { Username = username, StockName = stock, Amount = amount};

            client.AddBid(stockbid);
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
