using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using System.Web;
using System.Web.Mvc;
using System.Net;
using System.Threading.Tasks;

namespace Shoutbox.NET.Hubs
{
    public class ChatHub : Hub
    {
        public Task BroadcastChatMessage(string tag, string text)
        {
            Message msg = new Message();
            msg.Username = Context.User.Identity.Name;
            msg.Tag = HttpContext.Current.Server.HtmlEncode(tag);
            msg.Text = HttpContext.Current.Server.HtmlEncode(text);
            msg.Timestamp = DateTime.Now;


            return Clients.All.ReceiveChatMessage(msg.Username, "WRR", msg.Timestamp, msg.Tag, msg.Text);
        }
    }

    class Message
    {
        public string Username { get; set; }
        public string Tag { get; set; }
        public string Text { get; set; }
        public DateTime Timestamp { get; set; }
    }
}