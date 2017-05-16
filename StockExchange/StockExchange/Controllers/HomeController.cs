using System;
using System.Collections.Generic;
using System.Linq;
<<<<<<< HEAD
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
=======
using System.Web;
using System.Web.Mvc;
>>>>>>> 50aab336b9c455806c6a3725494884ed39824dc1

namespace StockExchange.Controllers
{
    public class HomeController : Controller
    {
<<<<<<< HEAD
        public IActionResult Index()
=======
        public ActionResult Index()
>>>>>>> 50aab336b9c455806c6a3725494884ed39824dc1
        {
            return View();
        }

<<<<<<< HEAD
        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";
=======
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";
>>>>>>> 50aab336b9c455806c6a3725494884ed39824dc1

            return View();
        }

<<<<<<< HEAD
        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
=======
        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}
>>>>>>> 50aab336b9c455806c6a3725494884ed39824dc1
