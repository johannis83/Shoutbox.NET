using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Shoutbox.NET.Models
{
    public class ShoutboxStatistics
    {
        public Tuple<int, int, int> UsagePerTypeToday { get; set; }
        //Stores average usage statistics per week day
        public Tuple<int, int, int, int, int> AverageMasterIncidentUsagePerWeekday { get; set; }
        public Tuple<int, int, int, int, int> AverageChatMessagesPerWeekday { get; set; }
        public Tuple<int, int, int, int, int> AverageAnnouncementsPerWeekday { get; set; }
        public int OnlineUsers { get; set; }
    }
}