using Microsoft.Security.Application;
using Shoutbox.NET.Data;
using Shoutbox.NET.Models;
using Shoutbox.NET.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Shoutbox.NET.Controllers
{
    public class TeamController : Controller, ITeamRepository
    {

        // GET: Team
        public Team SetMember(Team Team)
        {
            //Sanitize user input, prevent XSS.
            Team.Functie = Encoder.HtmlEncode(Team.Functie);
            Team.Naam = Encoder.HtmlEncode(Team.Naam);

            using (ShoutboxContext db = new ShoutboxContext())
            {
                //Check if the Team for that role already exists, if it does, remove it
                if (db.Teams.FirstOrDefault(f => f.Functie == Team.Functie) != null)
                    db.Teams.Remove(db.Teams.FirstOrDefault(f => f.Functie == Team.Functie));

                db.Teams.Add(Team);

                db.SaveChanges();

                return Team;
            }
        }

        // GET : Team
        public IEnumerable<Team> GetByDay(DateTime datetime)
        {
            if (ModelState.IsValid)
            {
                using (ShoutboxContext db = new ShoutboxContext())
                {
                    db.Configuration.LazyLoadingEnabled = false;
                    db.Configuration.ProxyCreationEnabled = false;

                    return db.Teams.Where(f => f.ModifiedAt.Day == DateTime.Now.Day).ToList();

                }
            }

            return null;
        }

    }
}