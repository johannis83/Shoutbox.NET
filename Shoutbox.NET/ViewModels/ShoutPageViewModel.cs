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
        public DateTime HistoryViewDate { get; set; }
        public User CurrentUser { get; set; }
        public IEnumerable<MasterIncident> MasterIncidents { get; set; }
        public IEnumerable<Message> Messages { get; set; }
        public IEnumerable<Team> Teams { get; set; }
        public IEnumerable<SOS> SOSList { get; set; }
        public IEnumerable<KM> KMList { get; set; }
        public Dictionary<string, int> Tags { get; set; }
        public ShoutboxStatistics DataDistribution { get; set; }

        public string SerializedMasterIncidents
        {
            get
            {
                return JsonConvert.SerializeObject(MasterIncidents);
            }
        }
        public string SerializedTeams
        {
            get
            {
                return JsonConvert.SerializeObject(Teams);
            }
        }
        public string SerializedMessages
        {
            get
            {
                return JsonConvert.SerializeObject(Messages);
            }
        }
        public string SerializedSOS
        {
            get
            {
                return JsonConvert.SerializeObject(SOSList);
            }
        }

        public string SerializedKMList
        {
            get
            {
                return JsonConvert.SerializeObject(KMList);
            }
        }

        public string SerializedTags
        {
            get
            {
                return JsonConvert.SerializeObject(Tags);
            }
        }
    }
}