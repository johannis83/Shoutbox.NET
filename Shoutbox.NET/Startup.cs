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
            //Build custom constructor for the chathub because Unity doesn't play nice with signalR
            //GlobalHost.DependencyResolver.Register(
            //typeof(ChatHub),() => new ChatHub(
            //      new UserController(), 
            //      new MessageController(), 
            //      new UserPrincipalController()));

            //GlobalHost.DependencyResolver.Register(
            //typeof(UserController), () => new UserController(
            //       new UserPrincipalController()));

            // Any connection or hub wire up and configuration should go here
            app.MapSignalR();
        }
    }
}