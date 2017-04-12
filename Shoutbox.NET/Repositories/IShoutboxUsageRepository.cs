using Shoutbox.NET.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Shoutbox.NET.Repositories
{
    public interface IShoutboxUsageRepository
    {
        Tuple<int, int, int> GetDataDistributionByDay(DateTime day);
        Tuple<int, int, int, int, int> GetAverageChatMessagesPerWeekday();
        Tuple<int, int, int, int, int> GetAverageMasterIncidentsPerWeekday();
        Tuple<int, int, int, int, int> GetAverageAnnouncementsPerWeekDay();
        int GetOnlineUserCount();
    }
}