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
            IndexViewModel indexViewModel = new IndexViewModel();
            using (ShoutboxContext db = new ShoutboxContext())
            {
                #region Set IndexViewModel's serialized messages
                //To avoid an infinite self referencing loops we store the values into an anonymous type first and then serialize it
                indexViewModel.SerializedMessages = Newtonsoft.Json.JsonConvert.SerializeObject(db.Messages.Select(f => new
                {
                    //Select the properties we want..
                    f.MessageID,
                    f.Tag,
                    f.Text,
                    f.Timestamp,

                    User = new
                    {
                        f.User.Division,
                        f.User.Name
                    }
                }).Where(f => f.Timestamp.Value.Day == DateTime.Now.Day)); //Only get TODAY's messages for the home page
                #endregion



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