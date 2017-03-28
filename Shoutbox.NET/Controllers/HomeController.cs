using Shoutbox.NET.Data;
using Shoutbox.NET.Models;
using Shoutbox.NET.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Shoutbox.NET.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            using (ShoutboxContext db = new ShoutboxContext())
            {
                IndexViewModel indexViewModel = new IndexViewModel();


                var messages = db.Messages.Select(f => new
                {
                    MessageID = f.MessageID,
                    Tag = f.Tag,
                    Text = f.Text,
                    Timestamp = f.Timestamp,
                    User = f.User
                });

                indexViewModel.msgs = Newtonsoft.Json.JsonConvert.SerializeObject(messages);

                return View(indexViewModel);
            }
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Historie()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}