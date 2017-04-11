using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace Shoutbox.NET.Models
{
    public class User
    {
        public string Name { get; set; }
        public string Division { get; set; }

        [JsonIgnore]
        public int UserID { get; set; }
        [JsonIgnore]
        public string Username { get; set; }
        [JsonIgnore]
        public string Domain { get; set; }
        [JsonIgnore]
        public string GridLayout { get; set; }
        [JsonIgnore]
        public string NotificationSettings { get; set; }
        [JsonIgnore]
        public Roles Role { get; set; }
        [JsonIgnore]
        public virtual ICollection<Message> Messages { get; set; }
    }

    public enum Roles
    {
        User = 0,
        Moderator = 1,
        Administrator = 2
    }
}