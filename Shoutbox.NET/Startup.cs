using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Owin;
using Microsoft.Owin;
using Microsoft.AspNet.SignalR;
using Shoutbox.NET.Hubs;
using Shoutbox.NET.Controllers;
using Shoutbox.NET.Services;

[assembly: OwinStartup(typeof(Shoutbox.NET.Startup))]
namespace Shoutbox.NET
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            GlobalHost.DependencyResolver.Register(
            typeof(ChatHub),
            () => new ChatHub(new UserController(), new MessageController()));

            // Any connection or hub wire up and configuration should go here
            app.MapSignalR();
        }
    }
}