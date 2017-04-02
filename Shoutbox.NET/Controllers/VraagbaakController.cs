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
    public class VraagbaakController : Controller, IVraagbaakRepository
    {

        // GET: Vraagbaak
        public Vraagbaak Set(Vraagbaak vraagbaak)
        {
            using (ShoutboxContext db = new ShoutboxContext())
            {
                //Check if the vraagbaak for that role already exists, if it does, change the name to the updated value
                if (db.Vraagbaken.FirstOrDefault(f => f.Functie == vraagbaak.Functie) != null)
                    db.Vraagbaken.FirstOrDefault(f => f.Functie == vraagbaak.Functie).Naam = vraagbaak.Naam;
                //If it doesn't exist yet, add it
                else
                    db.Vraagbaken.Add(vraagbaak);

                db.SaveChanges();

                return vraagbaak;
            }
        }
    }
}