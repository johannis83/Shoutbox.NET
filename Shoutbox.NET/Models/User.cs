using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace Shoutbox.NET.Models
{
    public class User
    {
        public int UserID { get; set; }
        public string Username { get; set; }
        public string Name { get; set; }
        public string Domain { get; set; }
        public string Settings { get; set; }
        public string Division { get; set; }
        public Roles Role { get; set; }

        public virtual ICollection<Message> Messages { get; set; }
    }

    public enum Roles
    {
        User = 0,
        Moderator = 1,
        Administrator = 2
    }
}