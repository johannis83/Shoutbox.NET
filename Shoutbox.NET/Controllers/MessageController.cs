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
using Shoutbox.NET.Repositories;
using Microsoft.Security.Application;

namespace Shoutbox.NET.Controllers
{
    [ValidateInput(true)]
    public class MessageController : Controller, IMessageRepository
    {
        private IUserRepository _UserService;

        public MessageController(IUserRepository service)
        {
            _UserService = service;
        }

        public MessageController()
        {
        }

        public IEnumerable<Message> GetByDay(DateTime datetime)
        {
            using (ShoutboxContext db = new ShoutboxContext())
            {
                //Disable dynamic proxy objects. Database is disposed in the view so we want these to be available 'offline'
                db.Configuration.ProxyCreationEnabled = false;

                return db.Messages.Include(f => f.User).Where(f => f.Timestamp.Value.Day == datetime.Day).ToList();
            }
        }

        public Dictionary<string, int> GetTagPopularityByDay(DateTime datetime)
        {
            using (ShoutboxContext db = new ShoutboxContext())
            {
                Dictionary<string, int> tags = new Dictionary<string, int>();
                //Get all of today's messages, select them distinct by the tags. Add those tags to the dictionary with the amount of each particular tag
                db.Messages.Where(f => f.Tag != "" && f.Timestamp.Value.Day == datetime.Day).GroupBy(t => t.Tag).Select(g => g.FirstOrDefault()).ToList().
                    ForEach(i => tags.Add(i.Tag, db.Messages.Count(x => x.Tag == i.Tag)));

                return tags;
            }
        }

        public Message Create(Message message)
        {
            using (ShoutboxContext db = new ShoutboxContext())
            {                
                //Html encode all user submitted input to prevent XSS
                //Let admins cross site script, if they know what they're doing :)
                if(message.User.Role < Roles.Administrator) message.Text = Encoder.HtmlEncode(message.Text);
                message.Tag = Encoder.HtmlEncode(message.Tag);
                message.Type = Encoder.HtmlEncode(message.Type);

                /*Attach the user to help EF understand the context. 
                 This basically avoids it from re-creating the user along with the message.*/
                db.Users.Attach(message.User);
                db.Messages.Add(message);
                db.SaveChanges();
                return message;
            }
        }
    }
}
