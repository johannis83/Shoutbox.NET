using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Shoutbox.NET.Models
{
    public class ShoutboxStatistics
    {
        public Tuple<int, int, int> UsagePerTypeToday { get; set; }
        public WeeklyStatistic AverageChatMessages { get; set; }
        public WeeklyStatistic AverageAnnouncementMessages { get; set; }
        public WeeklyStatistic AverageMasterIncident { get; set; }
        public List<Tag> MostPopularTags { get; set; }
        public int OnlineUsers { get; set; }
    }
}