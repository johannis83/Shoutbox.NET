using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Owin;
using Microsoft.Owin;
using Microsoft.AspNet.SignalR;
using Shoutbox.NET.Hubs;
using Shoutbox.NET.Controllers;
using Microsoft.AspNet.SignalR.Hubs;
using System.Web.Mvc;
using Shoutbox.NET.Repositories;
using System.Timers;
using System.Configuration;

[assembly: OwinStartup(typeof(Shoutbox.NET.Startup))]
namespace Shoutbox.NET
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            // Any connection or hub wire up and configuration should go here
            var hubConfiguration = new HubConfiguration { EnableDetailedErrors = true};
            app.MapSignalR(hubConfiguration);
            //Only authorized users can invoke our hub methods
            GlobalHost.HubPipeline.RequireAuthentication();
        }
    }
}