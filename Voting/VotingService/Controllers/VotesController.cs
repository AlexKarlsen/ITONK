﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Net.Http.Headers;
using System.Web.Http;
using VotingService;
using System.Fabric;
using System.Threading.Tasks;
using Newtonsoft.Json;

public class VotesController : ApiController
{
    // Used for health checks.
    public static long _requestCount = 0L;
    // Holds the votes and counts. NOTE: THIS IS NOT THREAD SAFE FOR THE PURPOSES OF THE LAB.
    //static Dictionary<string, int> _counts = new Dictionary<string, int>();
    HttpClient _client = new HttpClient();

    // GET api/votes 
    [HttpGet]
    [Route("api/votes")]
    public async Task<HttpResponseMessage> Get()
    {
        string activityId = Guid.NewGuid().ToString(); ServiceEventSource.Current.ServiceRequestStart("VotesController.Get", activityId);
        Interlocked.Increment(ref _requestCount);
        string url = $"http://localhost:19081/Voting/VotingState/api/votes?PartitionKey=0&PartitionKind=Int64Range"; HttpResponseMessage msg = await _client.GetAsync(url).ConfigureAwait(false);
        string json = await msg.Content.ReadAsStringAsync().ConfigureAwait(false);
        List<KeyValuePair<string, int>> votes = JsonConvert.DeserializeObject<List<KeyValuePair<string, int>>>(json);
        var response = Request.CreateResponse(HttpStatusCode.OK, votes);
        response.Headers.CacheControl = new CacheControlHeaderValue() { NoCache = true, MustRevalidate = true };
        ServiceEventSource.Current.ServiceRequestStop("VotesController.Get", activityId);
        return response;
    }

    [HttpPost]
    [Route("api/{key}")]
    public async Task<HttpResponseMessage> Post(string key)
    {
        string activityId = Guid.NewGuid().ToString(); ServiceEventSource.Current.ServiceRequestStart("VotesController.Post", activityId);
        Interlocked.Increment(ref _requestCount);
        string url =
        $"http://localhost:19081/Voting/VotingState/api/{key}?PartitionKey=0&PartitionKind=Int64Range";

        HttpResponseMessage msg = await _client.PostAsync(url, null).ConfigureAwait(false);
        ServiceEventSource.Current.ServiceRequestStop("VotesController.Post", activityId);
        return Request.CreateResponse(msg.StatusCode);
    }


    [HttpDelete]
    [Route("api/{key}")]
    public HttpResponseMessage Delete(string key)
    {
        string activityId = Guid.NewGuid().ToString(); ServiceEventSource.Current.ServiceRequestStart("VotesController.Delete", activityId);
        Interlocked.Increment(ref _requestCount); // Ignoring delete for this lab.
        ServiceEventSource.Current.ServiceRequestStop("VotesController.Delete", activityId);
        return Request.CreateResponse(HttpStatusCode.NotFound);
    }

    [HttpGet]
    [Route("api/{file}")]
    public HttpResponseMessage GetFile(string file)
    {
        string activityId = Guid.NewGuid().ToString();
        ServiceEventSource.Current.ServiceRequestStart("VotesController.GetFile", activityId);

        string response = null;
        string responseType = "text/html";
        Interlocked.Increment(ref _requestCount);
        // Validate file name.
        if ("index.html" == file)
        {
            //string path = string.Format(@"..\VotingServicePkg.Code.1.0.0\{0}", file);
            // Getting index.html dynamicly from fabrics runtime
            string path = Path.Combine(FabricRuntime.GetActivationContext().GetCodePackageObject("Code").Path, "index.html");
            response = File.ReadAllText(path);
        }

        ServiceEventSource.Current.ServiceRequestStop("VotesController.GetFile", activityId);

        if (null != response)
            return Request.CreateResponse(HttpStatusCode.OK, response, responseType);
        else
            return Request.CreateErrorResponse(HttpStatusCode.NotFound, "File");
    }
}