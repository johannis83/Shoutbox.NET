using System.Web.Mvc;
using Microsoft.Practices.Unity;
using Unity.Mvc3;
using Shoutbox.NET.Data;
using Shoutbox.NET.Controllers;
using Shoutbox.NET.Hubs;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using Shoutbox.NET.Repositories;

namespace Shoutbox.NET
{
    public static class Bootstrapper
    {
        public static void Initialise()
        {
            var container = BuildUnityContainer();

            DependencyResolver.SetResolver(new UnityDependencyResolver(container));
        }

        private static IUnityContainer BuildUnityContainer()
        {
            var container = new UnityContainer();

            // register all your components with the container here
            //To allow DI usage with SignalR
            GlobalHost.DependencyResolver.Register(typeof(IHubActivator), () => new UnityHubActivator(container));
            // it is NOT necessary to register your controllers
            container.RegisterType<IMessageRepository, MessageController>();
            container.RegisterType<IUserPrincipalRepository, UserPrincipalController>();
            container.RegisterType<IUserRepository, UserController>();
            container.RegisterType<ISOSRepository, SOSController>();
            container.RegisterType<ITeamRepository, TeamController>();
            container.RegisterType<IMasterIncidentRepository, MasterIncidentController>();

            return container;
        }
    }

    //Used for SignalR Dependency injection
    public class UnityHubActivator : IHubActivator
    {
        private readonly IUnityContainer container;

        public UnityHubActivator(IUnityContainer container)
        {
            this.container = container;
        }

        public IHub Create(HubDescriptor descriptor)
        {
            return (IHub)container.Resolve(descriptor.HubType);
        }
    }
}