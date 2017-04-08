using Shoutbox.NET.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Shoutbox.NET.Models;
using Shoutbox.NET.Data;
using Microsoft.Security.Application;

namespace Shoutbox.NET.Controllers
{
    public class MasterIncidentController : Controller, IMasterIncidentRepository
    {
        public IEnumerable<MasterIncident> GetByDay(DateTime datetime)
        {
            using (ShoutboxContext db = new ShoutboxContext())
            {
                //Disable dynamic proxy objects. Database is disposed in the view so we want these to be available 'offline'
                db.Configuration.ProxyCreationEnabled = false;
                return db.MasterIncidents.Where(f => f.Timestamp.Day == datetime.Day).ToList();
            }
        }

        public MasterIncident GetById(int id)
        {
            using (ShoutboxContext db = new ShoutboxContext())
            {
                db.Configuration.ProxyCreationEnabled = false;
                return db.MasterIncidents.FirstOrDefault(f => f.MasterIncidentID == id);
            }
        }

        public MasterIncident Create(MasterIncident masterIncident)
        {
            using (ShoutboxContext db = new ShoutboxContext())
            {
                //Html encode all user submitted input to prevent XSS
                masterIncident.IM = Encoder.HtmlEncode(masterIncident.IM);
                masterIncident.KM = Encoder.HtmlEncode(masterIncident.KM);
                masterIncident.Description = Encoder.HtmlEncode(masterIncident.Description);
                masterIncident.Active = true;

                db.Users.Attach(masterIncident.User);
                db.MasterIncidents.Add(masterIncident);
                db.SaveChanges();

                return db.MasterIncidents.FirstOrDefault(f => f.MasterIncidentID == masterIncident.MasterIncidentID);
            }
        }

        public MasterIncident Disable(int masterIncidentID)
        {
            using (ShoutboxContext db = new ShoutboxContext())
            {
                db.MasterIncidents.FirstOrDefault(f => f.MasterIncidentID == masterIncidentID).Active = false;
                db.SaveChanges();
                return db.MasterIncidents.FirstOrDefault(f => f.MasterIncidentID == masterIncidentID);
            }
        }
    }
}