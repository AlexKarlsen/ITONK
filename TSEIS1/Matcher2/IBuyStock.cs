using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Matcher2
{
    public interface IBuyStock
    {
        Task BuyStock(Common.Stock stock);
    }
}
