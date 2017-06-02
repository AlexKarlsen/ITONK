using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace Matcher.Controllers
{
    class MatcherController : ApiController
    {
        private Common.IStockBidService _service = null;

        public MatcherController(Common.IStockBidService service)
        {
            _service = service;
        }

        // GET api/values 
        [HttpGet]
        [Route("api/stocks")]
        public List<KeyValuePair<string, int>> Get()
        {
            //return new string[] { "value1", "value2" };

            ServiceEventSource.Current.ServiceRequestStart("Get");
            //Interlocked.Increment(ref _requestCount);

            List<KeyValuePair<string, int>> stocks = new List<KeyValuePair<string, int>>();
            stocks.Add(new KeyValuePair<string, int>("hej", 1));

            ServiceEventSource.Current.ServiceRequestStop("Get");
            return stocks;
        }

        //[HttpPost]
        //[Route("api/{key}")]
        //public async Task<HttpResponseMessage> PostAsync(string key)
        //{
        //    //string activityId = GetHeaderValueOrDefault(Request, activityHeader, () => { return Guid.NewGuid().ToString(); }); ServiceEventSource.Current.ServiceRequestStart("VotesController.Post", activityId);

        //    // Update or add the item.   
        //    var stock = new Common.StockBid() { Username = "test", StockName = "test", Amount = 10 };
        //    await _service.AddBid(stock);

        //    //ServiceEventSource.Current.ServiceRequestStop("VotesController.Post", activityId); 
        //    //_service.RequestCount = 1; 

        //    return Request.CreateResponse(HttpStatusCode.NoContent);
        //}

        /// <summary>         
        /// Gets a value from a header collection or returns the default value from the function.         
        /// </summary>         
        //public static string GetHeaderValueOrDefault(HttpRequestMessage request, string headerName, Func<string> getDefault)
        //{
        //    // If headers are not specified, return the default string.             
        //    if ((null == request) || (null == request.Headers))
        //        return getDefault();

        //    // Search for the header name in the list of headers.             
        //    IEnumerable<string> values;
        //    if (true == request.Headers.TryGetValues(headerName, out values))
        //    {
        //        // Return the first value from the list.                 
        //        foreach (string value in values)
        //            return value;
        //    }

        //    // return an empty string as default.             
        //    return getDefault();
        //}
    }
}
