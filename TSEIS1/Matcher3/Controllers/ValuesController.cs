using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Matcher3.Controllers
{
    [Route("api/[controller]")]
    public class ValuesController : Controller
    {
        // GET api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            var forSale = Matcher3.GetStocksForSale();
            var bids = Matcher3.GetStocksBids();

            var stockStrings = new List<String>();

            foreach (Common.Stock stock in forSale)
                stockStrings.Add(Newtonsoft.Json.JsonConvert.SerializeObject(stock));
                   
            foreach (Common.Stock stock in bids)
                stockStrings.Add(Newtonsoft.Json.JsonConvert.SerializeObject(stock));
            
            return stockStrings; 

            //return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody]Common.Stock stock)
        {
            var newstock = stock;

            if (newstock.StockType == Common.Stock.SaleOrPurchase.Purchase)
                Matcher3.BuyStock(newstock);
            else if (newstock.StockType == Common.Stock.SaleOrPurchase.Sale)
                Matcher3.SellStock(newstock);
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
