﻿using System;
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

namespace Shoutbox.NET.Hubs
{
    public class ChatHub : Hub
    {
        private IUserRepository _userRepository;
        private IMessageRepository _messageRepository;
        private IUserPrincipalRepository _userPrincipalRepository;
        private IVraagbaakRepository _vraagbaakRepository;

        public ChatHub(IUserRepository userService, IMessageRepository messageService, IUserPrincipalRepository userPrincipalRepository, IVraagbaakRepository vraagbaakRepository)
        {
            _userRepository = userService;
            _messageRepository = messageService;
            _userPrincipalRepository = userPrincipalRepository;
            _vraagbaakRepository = vraagbaakRepository;
        }

        public void RegisterIfNotRegistered()
        {
            User user = _userRepository.GetByLogonUser(Context.User.Identity.Name);
            if (user == null) _userRepository.Create(Context.User.Identity.Name);
        }

        public Task SetVraagbaak(string functie, string naam)
        {
            Vraagbaak vraagbaak = new Vraagbaak
            {
                Functie = functie,
                Naam = naam,
                ModifiedAt = DateTime.Now,
                //We wanna know who modified it to prevent abuse
                ModifiedBy = _userRepository.GetByLogonUser(Context.User.Identity.Name).Name
            };

            //Validate that the functie is an existing and allowed functie
            if (VraagbaakFuncties.Functies.Contains(vraagbaak.Functie))
            {
                _vraagbaakRepository.Set(vraagbaak);

                return Clients.All.UpdateVraagbaak(vraagbaak.Functie, vraagbaak.Naam, vraagbaak.ModifiedBy);
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

            _messageRepository.Create(message);

            return Clients.All.ReceiveChatMessage(
                message.User.Name, message.User.Division, message.Timestamp, message.Tag, message.Text, message.Type);
        }
    }

}