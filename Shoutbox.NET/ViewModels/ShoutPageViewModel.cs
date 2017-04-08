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
    public class ShoutPageViewModel
    {
        public User CurrentUser { get; set; }
        public IEnumerable<MasterIncident> MasterIncidents { get; set; }
        public IEnumerable<Message> Messages { get; set; }
        public IEnumerable<Team> Teams { get; set; }
        public IEnumerable<SOS> SOSList { get; set; }
        public Dictionary<string, int> Tags { get; set; }
        public string SerializedMasterIncidents
        {
            get
            {
                return JsonConvert.SerializeObject(MasterIncidents, Formatting.Indented,
                new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                });
            }
        }
        public string SerializedTeams
        {
            get
            {
                return JsonConvert.SerializeObject(Teams, Formatting.Indented,
                        new JsonSerializerSettings
                        {
                            ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                        });
            }
        }
        public string SerializedMessages
        {
            get
            {
                return JsonConvert.SerializeObject(Messages, Formatting.Indented,
                new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                });
            }
        }
        public string SerializedSOS
        {
            get
            {
                return JsonConvert.SerializeObject(SOSList);
            }
        }
    }
}