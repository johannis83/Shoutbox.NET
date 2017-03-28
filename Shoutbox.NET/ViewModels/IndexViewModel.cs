using Shoutbox.NET.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Shoutbox.NET.ViewModels
{
    public class IndexViewModel
    {
        public ICollection<Message> Messages { get; set; }

        public string msgs { get; set; }

        public Dictionary<string, int> Tags
        {
            get
            {
                Dictionary<string, int> tags = new Dictionary<string, int>();

                //Get all of today's messages, select them distinct by the tags. Add those tags to the dictionary with the amount of each particular tag
                Messages.Where(f => f.Timestamp.Value.Day == DateTime.Now.Day).GroupBy(t => t.Tag).Select(g => g.First()).ToList().
                    ForEach(i => tags.Add(i.Tag, Messages.Count(x => x.Tag == i.Tag)));

                return tags;
            }
        }
    }
}