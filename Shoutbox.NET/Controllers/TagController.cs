﻿using Shoutbox.NET.Data;
using Shoutbox.NET.Repositories;
using Shoutbox.NET.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Shoutbox.NET.Controllers
{
    public class TagController : Controller
    {
        private IMessageRepository _messageRepository;

        public TagController(IMessageRepository messageRepository)
        {
            _messageRepository = messageRepository;
        }
    }
}