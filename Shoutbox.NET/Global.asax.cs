﻿using Microsoft.AspNet.SignalR;
using Shoutbox.NET.Controllers;
using Shoutbox.NET.Models;
using Shoutbox.NET.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace Shoutbox.NET
{
    public class MvcApplication : System.Web.HttpApplication
    {       
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            //Keep clients alive
            GlobalHost.Configuration.DisconnectTimeout = TimeSpan.FromSeconds(90);
            GlobalHost.Configuration.KeepAlive = TimeSpan.FromSeconds(30);
            GlobalHost.Configuration.TransportConnectTimeout = TimeSpan.FromSeconds(10);

            //Configurate newton JSON
            HttpConfiguration config = GlobalConfiguration.Configuration;
            config.Formatters.JsonFormatter
                        .SerializerSettings
                        .ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;

            //Dependency (IOC) Initializer [Unity]
            Bootstrapper.Initialise();
        }
    }
}
