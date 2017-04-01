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
using Shoutbox.NET.Services;
using Microsoft.Practices.Unity;
using System.Text.RegularExpressions;

namespace Shoutbox.NET.Hubs
{
    public class ChatHub : Hub
    {
        private IUserRepository _userService;
        private IMessageRepository _messageService;
        private IUserPrincipalRepository _userPrincipalRepository;


        public ChatHub(IUserRepository userService, IMessageRepository messageService, IUserPrincipalRepository userPrincipalRepository)
        {
            _userService = userService;
            _messageService = messageService;
            _userPrincipalRepository = userPrincipalRepository;
        }

        public void RegisterIfNotRegistered()
        {
            User user = _userService.GetByLogonUser(Context.User.Identity.Name);
            if (user == null) _userService.Create(Context.User.Identity.Name);
        }

        public Task BroadcastChatMessage(string tag, string text, string type)
        {
            User user = _userService.GetByLogonUser(Context.User.Identity.Name);

            Message message = new Message
            {
                User = user,
                Timestamp = DateTime.Now,
                Tag = new Regex("[^a-zA-Z0-9-]").Replace(tag.ToUpper(), ""), //Upper cased & alphanumeric tags only
                Text = text,
                Type = MessageType.Types.FirstOrDefault(f => f == type) //Only allow message types that are defined by us
            };

            message.User.UserID = user.UserID;

            _messageService.Create(message);

            return Clients.All.ReceiveChatMessage(
                message.User.Name, message.User.Division, message.Timestamp, message.Tag, message.Text, message.Type);
        }
    }

}