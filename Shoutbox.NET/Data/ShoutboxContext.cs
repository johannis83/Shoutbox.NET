using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using Shoutbox.NET.Models;

namespace Shoutbox.NET.Data
{
    public class ShoutboxContext : DbContext
    {
        public ShoutboxContext() : base("ShoutboxContext")
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<Team> Teams { get; set; }
        public DbSet<MasterIncident> MasterIncidents { get; set; }

    }
}