using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StockExchange.Models
{
    public class StockShares
    {
        public Guid Id { get; set; }
        public Stock Stock { get; set; }
        public int Share { get; set; }
        public virtual ApplicationUser Users { get; set; }
    }
}