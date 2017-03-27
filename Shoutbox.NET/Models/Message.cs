using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Shoutbox.NET.Models
{
    public class Message
    {
        public int MessageID { get; set; }
        public DateTime? Timestamp { get; set; }
        public string Tag { get; set; }
        public string Text { get; set; }

        public virtual User User { get; set; }
    }
}