using Shoutbox.NET.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using Shoutbox.NET.SSTMonitorService;

namespace Shoutbox.NET.ViewModels
{
    public class IndexViewModel
    {
        public Status SM9Status { get; set; }
        public Status EasyVistaStatus { get; set; }

        public ICollection<Message> Messages
        {
            get
            {
                return JsonConvert.DeserializeObject<ICollection<Message>>(SerializedMessages);
            }
        }

        public ICollection<Team> Teams
        {
            get
            {
                return JsonConvert.DeserializeObject<ICollection<Team>>(SerializedTeams);
            }
        }

        public string SerializedTeams { get; set; }
        public string SerializedMessages { get; set; }
        

        public Dictionary<string, int> Tags
        {
            get
            {
                Dictionary<string, int> tags = new Dictionary<string, int>();

                //Get all of today's messages, select them distinct by the tags. Add those tags to the dictionary with the amount of each particular tag
                Messages.Where(f => f.Tag != "" && f.Timestamp.Value.Day == DateTime.Now.Day).GroupBy(t => t.Tag).Select(g => g.First()).ToList().
                    ForEach(i => tags.Add(i.Tag, Messages.Count(x => x.Tag == i.Tag)));

                return tags;
            }
        }
    }
}