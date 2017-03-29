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
        private IUserControllerService _userService;
        private IMessageControllerService _messageService;


        public ChatHub(IUserControllerService userService, IMessageControllerService messageService)
        {
            _userService = userService;
            _messageService = messageService;
        }

        //Allow only alphanumeric characters in hashtags
        

        public Task BroadcastChatMessage(string tag, string text)
        {
            User user = _userService.GetByUsername(Context.User.Identity.Name.Split('\\')[1]);

            Message message = new Message
            {
                User = user,
                Timestamp = DateTime.Now,
                Tag = new Regex("[^a-zA-Z0-9 -]").Replace(tag.ToUpper(), ""), //Upper cased & alphanumeric tags only
                Text = text,
            };

            message.User.UserID = user.UserID;

            _messageService.Create(message);

            return Clients.All.ReceiveChatMessage(
                message.User.Name, message.User.Division, message.Timestamp, message.Tag, message.Text);
        }
    }

}