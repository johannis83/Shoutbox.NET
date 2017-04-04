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
        public Team Set(Team Team)
        {
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
    }
}