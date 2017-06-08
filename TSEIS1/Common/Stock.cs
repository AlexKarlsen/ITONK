using System;
using System.Threading.Tasks;
using System.ServiceModel;
using System.Collections.Generic;
using Microsoft.ServiceFabric.Services.Remoting;
using Microsoft.ServiceFabric.Services.Runtime;

namespace Common
{
    public class Stock
    {
        public enum SaleOrPurchase { Sale, Purchase };

        public string Username { get; set; }
        public string StockName { get; set; }
        public int Amount { get; set; }
        public SaleOrPurchase StockType { get; set; }
    }

    //[ServiceContract]
    //public interface IStockBidService : IService
    //{
    //    [OperationContract]
    //    Task AddBid(Stock stockBid);
    //}

    //[ServiceContract]
    //public interface IStockSaleService
    //{
    //    [OperationContract]
    //    Task AddSale(Stock stockSaleOffer);
    //}

}
