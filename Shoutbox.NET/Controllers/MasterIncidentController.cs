using Shoutbox.NET.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Shoutbox.NET.Models;
using Shoutbox.NET.Data;

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

        public MasterIncident Create(MasterIncident masterIncident)
        {
            using (ShoutboxContext db = new ShoutboxContext())
            {
                db.Users.Attach(masterIncident.User);
                db.MasterIncidents.Add(masterIncident);
                db.SaveChanges();

                return db.MasterIncidents.FirstOrDefault(f => f.MasterIncidentID == masterIncident.MasterIncidentID);
            }
        }

        public MasterIncident Delete(MasterIncident masterIncident)
        {
            using (ShoutboxContext db = new ShoutboxContext())
            {
                db.MasterIncidents.Remove(masterIncident);
                db.SaveChanges();

                return db.MasterIncidents.FirstOrDefault(f => f.MasterIncidentID == masterIncident.MasterIncidentID);
            }
        }
    }
}