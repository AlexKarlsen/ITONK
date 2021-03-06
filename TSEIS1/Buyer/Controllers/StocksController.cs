﻿using Microsoft.ServiceFabric.Services.Client;
using System.Collections.Generic;
using System.Fabric;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Web.Http;
using Common;
using System;

namespace Buyer.Controllers
{
    [ServiceRequestActionFilter]
    public class StocksController : ApiController
    {
        // Used for health checks.
        public static long _requestCount = 0L;

        // GET api/values 
        [HttpGet]
        [Route("api/stocks")]
        public List<KeyValuePair<string, int>> Get()
        {
            // This function is implemented as a test function, just to test the access to the controller
            // It is not required for the app, as it's only purpose is to post buy bids to the matcher.
            //return new string[] { "value1", "value2" };

            ServiceEventSource.Current.ServiceRequestStart("Get");
            Interlocked.Increment(ref _requestCount);
            
            List<KeyValuePair<string, int>> stocks = new List<KeyValuePair<string, int>>();
            stocks.Add(new KeyValuePair<string, int>("hej", 1));
            
            ServiceEventSource.Current.ServiceRequestStop("Get");
            return stocks;
        }

        // GET api/values/5 
        [HttpGet]
        [Route("api/stocks/{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values 
        [System.Web.Http.HttpPost]
        [Route("api/{value}")]
        public async System.Threading.Tasks.Task PostAsync([FromUri]string value)
        {
            // Decoding input
            var firstDelimiter = value.IndexOf(';');
            var lastDelimiter = value.LastIndexOf(';');

            var username = value.Substring(0, firstDelimiter);
            var stockname = value.Substring(firstDelimiter + 1, value.Length - (firstDelimiter + 1));
            stockname = stockname.Substring(0, stockname.IndexOf(';'));

            var amount = value.Substring(lastDelimiter + 1, value.Length - (lastDelimiter + 1));

            int amountInt;

            if (int.TryParse(amount, out amountInt))
            {
                var stock = new Common.Stock() { StockName = stockname, StockType = Common.Stock.SaleOrPurchase.Purchase, Username = username, Amount = amountInt };

                await Buyer.PlaceRequestAsync(stock);
            }
        }


        // PUT api/values/5 
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5 
        public void Delete(int id)
        {
        }

        // Returns the html file as UI to the web api, requires HtmlMediaFormatter.cs
        [HttpGet]
        [Route("api/index.html")]
        public HttpResponseMessage GetFile()
        {
            string file = "index.html";

            //string activityId = Guid.NewGuid().ToString();
            ServiceEventSource.Current.ServiceRequestStart("StocksController.GetFile");

            string response = null;
            string responseType = "text/html";
            Interlocked.Increment(ref _requestCount);
            // Validate file name.
            if ("index.html" == file)
            {
                // Getting index.html dynamicly from fabrics runtime
                string path = Path.Combine(FabricRuntime.GetActivationContext().GetCodePackageObject("Code").Path, "index.html");
                response = File.ReadAllText(path);
            }

            ServiceEventSource.Current.ServiceRequestStop("StocksController.GetFile");

            if (null != response)
                return Request.CreateResponse(HttpStatusCode.OK, response, responseType);
            else
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, "File");
        }
    }
}
