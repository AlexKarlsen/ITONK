using System;
using System.Threading.Tasks;
using System.ServiceModel;
using System.Collections.Generic;
using Microsoft.ServiceFabric.Services.Remoting;

namespace Common
{
    public class StockBid
    {
        public string Username { get; set; }
        public string StockName { get; set; }
        public int Amount { get; set; }
    }

    [ServiceContract]
    public interface IStockBidService : IService
    {
        [OperationContract]
        Task AddBid(StockBid stockBid);
    }

    [ServiceContract]
    public interface IStockSaleService
    {
        [OperationContract]
        Task AddSale(StockBid stockSaleOffer);
    }

}
