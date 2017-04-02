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
        // GET: SSTStatus
        public static Status Get()
        {
            using (SSTMonitorServiceClient serviceClient = new SSTMonitorServiceClient())
            {
                return serviceClient.GetStatus();
            }
        }
    }
}