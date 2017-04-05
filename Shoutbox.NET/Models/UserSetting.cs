using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Shoutbox.NET.Models
{
    public class UserSetting
    {
        public int ID { get; set; }
        public DateTime? LastChanged { get; set; }
        public string Role { get; set; }
        public string Layout { get; set; }
        public virtual User User { get; set; }
    }
}