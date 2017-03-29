using Shoutbox.NET.Data;
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
        public ActionResult Tag(string tag)
        {
            IndexViewModel tagViewModel = new IndexViewModel();
            using (ShoutboxContext db = new ShoutboxContext())
            {
                //To avoid an infinite self referencing loops we store the values into an anonymous type first and then serialize it
                tagViewModel.SerializedMessages = Newtonsoft.Json.JsonConvert.SerializeObject(db.Messages.Select(f => new
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
                }).Where(f => f.Timestamp.Value.Day == DateTime.Now.Day).Where(x => x.Tag == tag)); //Only get TODAY's messages for the home page

                return View(tagViewModel);
            }
        }
    }
}