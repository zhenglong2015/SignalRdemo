using Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SignalRMVC.Controllers
{
    public class DefaultController : BaseController<MyHub>
    {
        // GET: Default
        public ActionResult Index()
        {
            return View();
        }
    }
}