﻿using System;
using System.Threading.Tasks;
using System.ServiceModel;
using System.Collections.Generic;

namespace Common
{
    public class Stock
    {
        public string StockName { get; set; }
        public string Amount { get; set; }
    }

    [ServiceContract]
    public interface IStockService
    {
        [OperationContract]
        Task AddStockBid(Stock stock, string offerType);
        [OperationContract]
        Task CompleteTransaction(Stock boughtStock, List<Stock> soldStocks);
    }

}
