using System;
using Microsoft.ServiceFabric.Services.Remoting.Client;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public static class MatcherConnectionFactory
    {
        private static readonly Uri MatcherServiceUrl = new Uri("fabric:/TSEIS1/Matcher2");

        public static IBuyStock CreateBuyStock()
        {
            return ServiceProxy.Create<IBuyStock>(MatcherServiceUrl);
        }
    }
}
