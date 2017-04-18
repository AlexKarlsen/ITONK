using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StockExchange.Models
{
    public class Stock
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string ShortName { get; set; }
        public double Rate { get; set; }
    }
}