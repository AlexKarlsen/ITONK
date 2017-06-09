using System;
using System.Collections.Generic;
using System.Fabric;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.ServiceFabric.Services.Communication.Runtime;
using Microsoft.ServiceFabric.Services.Runtime;
using Microsoft.ServiceFabric.Services.Remoting.Client;
using Microsoft.ServiceFabric.Services.Communication.Wcf;
using System.ServiceModel.Channels;
using Microsoft.ServiceFabric.Services.Client;
using Microsoft.ServiceFabric.Services.Communication.Wcf.Client;
using Microsoft.ServiceFabric.Services.Communication.Client;
using Common;
using System.ServiceModel;

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

        private static NetTcpBinding CreateClientConnectionBinding()
        {
            NetTcpBinding binding = new NetTcpBinding(SecurityMode.None)
            {
                SendTimeout = TimeSpan.MaxValue,
                ReceiveTimeout = TimeSpan.MaxValue,
                OpenTimeout = TimeSpan.FromSeconds(5),
                CloseTimeout = TimeSpan.FromSeconds(5),
                MaxReceivedMessageSize = 1024 * 1024
            };
            binding.MaxBufferSize = (int)binding.MaxReceivedMessageSize;
            binding.MaxBufferPoolSize = Environment.ProcessorCount * binding.MaxReceivedMessageSize;

            return binding;
        }

        public static async Task AddBidOnMatchingServiceAsync(string username, string stock, int amount)
        {
            //var client = ServiceProxy.Create<Common.IStockBidService>(new Uri("fabric:/TSEIS1/Matcher"));

            //var stockbid = new Common.Stock() { Username = username, StockName = stock, Amount = amount};

            //client.AddBid(stockbid);


            var stockbid = new Common.Stock() { Username = username, StockName = stock, Amount = amount };
            //var stockBuyer = Common.MatcherConnectionFactory.CreateBuyStock();
            //var msg = await stockBuyer.BuyStock(stockbid);
            //var something = "hej";



            var serviceUri = new Uri("fabric:/TSEIS1/Matcher2");
            var serviceResolver = new ServicePartitionResolver(() => new FabricClient());   // ?
            var binding = CreateClientConnectionBinding();
            var client = new Client(new WcfCommunicationClientFactory<Common.IBuyStock>(binding), serviceUri);

            client.BuyStock(stockbid);
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

    public class Client : ServicePartitionClient<WcfCommunicationClient<Common.IBuyStock>>, Common.IBuyStock
    {
        public Client(ICommunicationClientFactory<WcfCommunicationClient<IBuyStock>> communicationClientFactory, Uri serviceUri, ServicePartitionKey partitionKey = null, TargetReplicaSelector targetReplicaSelector = TargetReplicaSelector.Default, string listenerName = null, OperationRetrySettings retrySettings = null) : base(communicationClientFactory, serviceUri, partitionKey, targetReplicaSelector, listenerName, retrySettings)
        {            
        }

        public Task<string> BuyStock(Stock stock)
        {
            return this.InvokeWithRetryAsync(client => client.Channel.BuyStock(stock));
        }
    }

}
