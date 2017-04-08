using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using System.Web.Mvc;
using System.Net;
using System.Threading.Tasks;
using Shoutbox.NET.Models;
using Shoutbox.NET.Data;
using Shoutbox.NET.Controllers;
using Microsoft.Practices.Unity;
using System.Text.RegularExpressions;
using Shoutbox.NET.Repositories;
using System.Configuration;
using System.Timers;

namespace Shoutbox.NET.Hubs
{
    public class ShoutHub : Hub
    {
        private IUserRepository _userRepository;
        private IMessageRepository _messageRepository;
        private IUserPrincipalRepository _userPrincipalRepository;
        private ITeamRepository _TeamRepository;
        private IMasterIncidentRepository _MasterIncidentRepository;
        private ISOSRepository _sosRepository;

        //SOS timer to send periodic updates to all clients
        private Timer timer;

        public ShoutHub(IUserRepository userService, IMessageRepository messageService, IUserPrincipalRepository userPrincipalRepository, 
            ITeamRepository TeamRepository, IMasterIncidentRepository masterIncidentRepository, ISOSRepository sosRepository)
        {
            _userRepository = userService;
            _messageRepository = messageService;
            _userPrincipalRepository = userPrincipalRepository;
            _TeamRepository = TeamRepository;
            _MasterIncidentRepository = masterIncidentRepository;
            _sosRepository = sosRepository;

            //SOS Timer
            timer = new Timer(int.Parse(ConfigurationManager.AppSettings["SOSUpdateInerval"]));
            timer.Elapsed += new ElapsedEventHandler(UpdateSOS);
            timer.Start();
        }

        private void UpdateSOS(object sender, ElapsedEventArgs e)
        {
            //Store old state
            List<SOS> oldList = _sosRepository.GetList();
            //Update the previous list
            _sosRepository.UpdateSOSList();
            //Store new state
            List<SOS> newList = _sosRepository.GetList();

            //Compare the time of the latest updated issues, or the amount of SOS's in the list, if they're uneven, this means
            //the list has changed, so broadcast the new list to the clients
            if (!(oldList.Where(f => newList.Any(x => x.Time == f.Time)).Count() == newList.Count())|| 
                newList.Count != oldList.Count)
            {
                Clients.All.UpdateSOS(Newtonsoft.Json.JsonConvert.SerializeObject(newList));
            }
        }

        public void RegisterIfNotRegistered()
        {
            User user = _userRepository.GetByLogonUser(Context.User.Identity.Name);
            if (user == null) _userRepository.Create(Context.User.Identity.Name);
        }

        public Task SetTeam(string functie, string naam)
        {
            User user = _userRepository.GetByLogonUser(Context.User.Identity.Name);

            if (user.Role < Roles.User)
            {
                return null;
            }

            Team Team = new Team
            {
                Functie = functie,
                Naam = naam,
                ModifiedAt = DateTime.Now,
                //We wanna know who modified it to prevent abuse
                ModifiedBy = _userRepository.GetByLogonUser(Context.User.Identity.Name).Name
            };

            //Validate that the functie is an existing and allowed functie
            if (TeamFuncties.Functies.Contains(Team.Functie))
            {
                _TeamRepository.SetMember(Team);

                return Clients.All.UpdateTeam(Team.Functie, Team.Naam, Team.ModifiedBy);
            }

            return null;
        }

        public Task BroadcastChatMessage(string tag, string text, string type)
        {
            User user = _userRepository.GetByLogonUser(Context.User.Identity.Name);

            Message message = new Message
            {
                User = user,
                Timestamp = DateTime.Now,
                Tag = new Regex("[^a-zA-Z0-9-]").Replace(tag.ToUpper(), ""), //Upper cased & alphanumeric tags only
                Text = text,
                Type = MessageType.Types.FirstOrDefault(f => f == type) //Only allow message types that are defined by us
            };

            message.User.UserID = user.UserID;

            //Add the message to the database
            _messageRepository.Create(message);


            return Clients.All.ReceiveChatMessage(
                message.User.Name, message.User.Division, message.Timestamp, message.Tag, message.Text, message.Type);
        }

        public Task CreateMasterIncident(string description, string km, string im)
        {
            User user = _userRepository.GetByLogonUser(Context.User.Identity.Name);

            if(user.Role < Roles.Moderator)
            {
                return null;
            }

            MasterIncident masterincident = new MasterIncident
            {
                Description = description,
                KM = km,
                IM = im,
                Timestamp = DateTime.Now,
                User = user
            };

            _MasterIncidentRepository.Create(masterincident);

            return Clients.All.AddMasterIncident(masterincident.Description, masterincident.KM, masterincident.IM, masterincident.Timestamp);
        }
    }

}