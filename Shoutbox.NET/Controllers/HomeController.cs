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

                //Eager load all the necessary properties since data is deferred when the view is loaded
                indexViewModel.Messages = db.Messages
                    .Include(f => f.User).ToList();

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