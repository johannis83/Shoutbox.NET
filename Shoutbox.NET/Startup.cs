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
using Microsoft.AspNet.SignalR.Hubs;
using System.Web.Mvc;

[assembly: OwinStartup(typeof(Shoutbox.NET.Startup))]
namespace Shoutbox.NET
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            //Build custom constructor for the chathub because Unity doesn't play nice with signalR
            var unityHubActivator = new MvcHubActivator();

            GlobalHost.DependencyResolver.Register(
                typeof(IHubActivator),
                () => unityHubActivator);

            // Any connection or hub wire up and configuration should go here
            app.MapSignalR();
        }
    }

    public class MvcHubActivator : IHubActivator
    {
        public IHub Create(HubDescriptor descriptor)
        {
            return (IHub)DependencyResolver.Current
                .GetService(descriptor.HubType);
        }
    }
}