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
        public string Type { get; set; }
        public virtual User User { get; set; }
    }

    /* Would rather use an ENUM but those are stored as ints.
       This also makes it easier to validate that the entered Message Type is allowed
       without having to create an if statement for each type */
    public static class MessageType
    {
        public static List<string> Types = new List<string>
        {
            "Chat", "Announcement"
        };
    }
}