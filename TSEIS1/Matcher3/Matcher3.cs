using System;
using System.Collections.Generic;
using System.Fabric;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.ServiceFabric.Services.Communication.AspNetCore;
using Microsoft.ServiceFabric.Services.Communication.Runtime;
using Microsoft.ServiceFabric.Services.Runtime;

namespace Matcher3
{
    /// <summary>
    /// The FabricRuntime creates an instance of this class for each service type instance. 
    /// </summary>
    internal sealed class Matcher3 : StatelessService
    {
        public Matcher3(StatelessServiceContext context)
            : base(context)
        { }

        // Leet stateful hack
        private static List<Common.Stock> _stockBidList = new List<Common.Stock>();
        private static List<Common.Stock> _stockSaleList = new List<Common.Stock>();

        // Method is not thread safe
        public static void BuyStock(Common.Stock stock)
        {
            // Flag to check if the matching was successful
            var foundMatch = false;

            foreach(Common.Stock stkBeingSold in _stockSaleList)
            {
                if (stkBeingSold.StockName == stock.StockName)
                    if (stkBeingSold.Amount == stock.Amount)
                    {
                        //ServiceEventSource.Current.ServiceMessage(context, "Buyer got a full match; " + stock.Username + " bought " + stock.Amount + " of " + stock.StockName + " from " + stkBeingSold.Username);
                        _stockSaleList.Remove(stkBeingSold);

                        foundMatch = true;
                        break;
                    }
                    else if (stkBeingSold.Amount > stock.Amount)
                    {
                        //ServiceEventSource.Current.ServiceMessage(context ,"Buyer got a match in a larger sale; " + stock.Username + " bought " + stock.Amount + " of " + stock.StockName + " from " + stkBeingSold.Username);
                        _stockSaleList.Remove(stkBeingSold);
                        stkBeingSold.Amount -= stock.Amount;
                        _stockSaleList.Add(stkBeingSold);

                        foundMatch = true;
                        break;
                    }
                    else if (stkBeingSold.Amount < stock.Amount)
                    {
                        //ServiceEventSource.Current.ServiceMessage(context, "Buyer got a partial match in a smaller sale; " + stock.Username + " bought " + stkBeingSold.Amount + " of " + stock.StockName + " from " + stkBeingSold.Username);
                        _stockSaleList.Remove(stkBeingSold);
                        stock.Amount -= stkBeingSold.Amount;
                        // Recursively call same method to check if further matches can be found
                        BuyStock(stock);

                        foundMatch = true;
                        break;
                    }
            }

            // Add unfulfilled bids to list to be matched later
            if (foundMatch == false)
                _stockBidList.Add(stock);
        }

        public static void SellStock(Common.Stock stock)
        {
            // Flag to check if the matching was successful
            var foundMatch = false;

            foreach (Common.Stock stkBid in _stockBidList)
            {
                if (stkBid.StockName == stock.StockName)
                    if (stkBid.Amount == stock.Amount)
                    {
                        //ServiceEventSource.Current.ServiceMessage(context, "Seller got a full match; " + stock.Username + " sold " + stock.Amount + " of " + stock.StockName + " from " + stkBid.Username);
                        _stockBidList.Remove(stkBid);

                        foundMatch = true;
                        break;
                    }
                    else if (stkBid.Amount > stock.Amount)
                    {
                        //ServiceEventSource.Current.ServiceMessage(context, "Seller got a match in a larger bid; " + stock.Username + " sold " + stock.Amount + " of " + stock.StockName + " from " + stkBid.Username);
                        _stockBidList.Remove(stkBid);
                        stkBid.Amount -= stock.Amount;
                        _stockBidList.Add(stkBid);

                        foundMatch = true;
                        break;
                    }
                    else if (stkBid.Amount < stock.Amount)
                    {
                        //ServiceEventSource.Current.ServiceMessage(context, "Seller got a partial match in a smaller bid; " + stock.Username + " sold " + stkBid.Amount + " of " + stock.StockName + " from " + stkBid.Username);
                        _stockBidList.Remove(stkBid);
                        stock.Amount -= stkBid.Amount;
                        // Recursively call same method to check if further matches can be found
                        SellStock(stock);

                        foundMatch = true;
                        break;
                    }
            }

            // Add unfulfilled bids to list to be matched later
            if (foundMatch == false)
                _stockSaleList.Add(stock);
        }

        public static List<Common.Stock> GetStocksForSale()
        {
            List<Common.Stock> copy = new List<Common.Stock>(_stockSaleList);
            return copy;
        }

        public static List<Common.Stock> GetStocksBids()
        {
            List<Common.Stock> copy = new List<Common.Stock>(_stockBidList);
            return copy;
        }

        /// <summary>
        /// Optional override to create listeners (like tcp, http) for this service instance.
        /// </summary>
        /// <returns>The collection of listeners.</returns>
        protected override IEnumerable<ServiceInstanceListener> CreateServiceInstanceListeners()
        {
            return new ServiceInstanceListener[]
            {
                new ServiceInstanceListener(serviceContext =>
                    new WebListenerCommunicationListener(serviceContext, "ServiceEndpoint", (url, listener) =>
                    {
                        ServiceEventSource.Current.ServiceMessage(serviceContext, $"Starting WebListener on {url}");

                        return new WebHostBuilder().UseWebListener()
                                    .ConfigureServices(
                                        services => services
                                            .AddSingleton<StatelessServiceContext>(serviceContext))
                                    .UseContentRoot(Directory.GetCurrentDirectory())
                                    .UseStartup<Startup>()
                                    .UseApplicationInsights()
                                    .UseServiceFabricIntegration(listener, ServiceFabricIntegrationOptions.None)
                                    .UseUrls(url)
                                    .Build();
                    }))
            };
        }
    }
}
