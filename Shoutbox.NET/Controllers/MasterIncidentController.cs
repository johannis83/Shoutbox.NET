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