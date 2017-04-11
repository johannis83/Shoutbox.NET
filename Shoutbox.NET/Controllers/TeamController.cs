using Microsoft.Ajax.Utilities;
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

        public Team SetMember(Team Team)
        {
            //Sanitize user input, prevent XSS.
            Team.Functie = Encoder.HtmlEncode(Team.Functie);
            Team.Naam = Encoder.HtmlEncode(Team.Naam);

            using (ShoutboxContext db = new ShoutboxContext())
            {
                db.Teams.Add(Team);
                db.SaveChanges();

                return Team;
            }
        }

        public IEnumerable<Team> GetByDay(DateTime datetime)
        {
            using (ShoutboxContext db = new ShoutboxContext())
            {
                db.Configuration.ProxyCreationEnabled = false;

                //Get the last 4 updated
                return db.Teams.Where(f => f.ModifiedAt.Day == datetime.Day)
                    .OrderByDescending(f => f.ModifiedAt)
                    .DistinctBy(f => f.Functie).ToList();
            }
        }
    }
}