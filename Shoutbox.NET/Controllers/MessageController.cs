using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Shoutbox.NET.Data;
using Shoutbox.NET.Models;
using Shoutbox.NET.Services;

namespace Shoutbox.NET.Controllers
{
    public class MessageController : Controller, IMessageControllerService
    {
        private IUserControllerService _UserService;

        public MessageController(IUserControllerService service)
        {
            _UserService = service;
        }

        public MessageController()
        {
        }

        public Message Create(Message message)
        {
            if (ModelState.IsValid)
            {
                using (ShoutboxContext db = new ShoutboxContext())
                {
                    db.Messages.Add(message);
                    db.SaveChanges();
                    return message;
                }
            }

            return null;
        }

    }
}
