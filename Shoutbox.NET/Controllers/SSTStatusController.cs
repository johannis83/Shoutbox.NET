using Shoutbox.NET.SSTMonitorService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Shoutbox.NET.Controllers
{
    public class SSTStatusController : Controller
    {
        [HttpGet]
        public ActionResult Get(string Service)
        {
            using (SSTMonitorServiceClient serviceClient = new SSTMonitorServiceClient())
            {
                return Content(serviceClient.GetStatus(Service).ToString());
            }
        }
    }
}