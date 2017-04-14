using Microsoft.AspNet.SignalR;
using Shoutbox.NET.Data;
using Shoutbox.NET.Hubs;
using Shoutbox.NET.Models;
using Shoutbox.NET.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Shoutbox.NET.Controllers
{
    public class ShoutboxStatisticsController : IShoutboxUsageRepository
    {

        public Tuple<int, int, int> GetDataDistributionByDay(DateTime day)
        {
            ShoutboxStatistics dataDistribution = new ShoutboxStatistics();

            using (ShoutboxContext db = new ShoutboxContext())
            {
                //First day used to count the amount of days until now
                DateTime firstDay = db.Messages.OrderBy(f => f.Timestamp).FirstOrDefault().Timestamp;

                //Get the total amount of messages and divide them by the amount of days have passed since the first message
                int announcements = db.Messages.Where(f => f.Type == "Announcement" && f.Timestamp.Day == day.Day).Count();
                int chatMessages = db.Messages.Where(f => f.Type == "Chat" && f.Timestamp.Day == day.Day).Count();
                int masterIncidents = db.MasterIncidents.Where(f => f.Timestamp.Day == day.Day).Count();

                return new Tuple<int, int, int>(masterIncidents, announcements, chatMessages);
            }
        }

        public int GetOnlineUserCount()
        {
            return ShoutHub.ConnectedUsers.Count;
        }

        public Tuple<int, int, int, int, int> GetAverageChatMessagesPerWeekday()
        {
            using (ShoutboxContext db = new ShoutboxContext())
            {
                //First day used to count the amount of days until now
                DateTime firstDay = db.Messages.OrderBy(f => f.Timestamp).FirstOrDefault().Timestamp;

                int averageOnMonday = db.Messages.ToList().Where(f => f.Timestamp.DayOfWeek == DayOfWeek.Monday && f.Type == "Chat").Count() / (GetWorkingDays(firstDay, DateTime.Now) / 5);
                int averageOnTuesday = db.Messages.ToList().Where(f => f.Timestamp.DayOfWeek == DayOfWeek.Tuesday && f.Type == "Chat").Count() / (GetWorkingDays(firstDay, DateTime.Now) / 5);
                int averageOnWednesday = db.Messages.ToList().Where(f => f.Timestamp.DayOfWeek == DayOfWeek.Wednesday && f.Type == "Chat").Count() / (GetWorkingDays(firstDay, DateTime.Now) / 5);
                int averageOnThursday = db.Messages.ToList().Where(f => f.Timestamp.DayOfWeek == DayOfWeek.Thursday && f.Type == "Chat").Count() / (GetWorkingDays(firstDay, DateTime.Now) / 5);
                int averageOnFriday = db.Messages.ToList().Where(f => f.Timestamp.DayOfWeek == DayOfWeek.Friday && f.Type == "Chat").Count() / (GetWorkingDays(firstDay, DateTime.Now) / 5);

                return new Tuple<int, int, int, int, int>(averageOnMonday, averageOnTuesday, averageOnWednesday, averageOnThursday, averageOnFriday);
            }
        }


        public Tuple<int, int, int, int, int> GetAverageMasterIncidentsPerWeekday()
        {
            using (ShoutboxContext db = new ShoutboxContext())
            {
                //First day used to count the amount of days until now
                DateTime firstDay = db.Messages.OrderBy(f => f.Timestamp).FirstOrDefault().Timestamp;

                int averageOnMonday = db.MasterIncidents.ToList().Where(f => f.Timestamp.DayOfWeek == DayOfWeek.Monday).Count() / (GetWorkingDays(firstDay, DateTime.Now) / 5);
                int averageOnTuesday = db.MasterIncidents.ToList().Where(f => f.Timestamp.DayOfWeek == DayOfWeek.Tuesday).Count() / (GetWorkingDays(firstDay, DateTime.Now) / 5);
                int averageOnWednesday = db.MasterIncidents.ToList().Where(f => f.Timestamp.DayOfWeek == DayOfWeek.Wednesday).Count() / (GetWorkingDays(firstDay, DateTime.Now) / 5);
                int averageOnThursday = db.MasterIncidents.ToList().Where(f => f.Timestamp.DayOfWeek == DayOfWeek.Thursday).Count() / (GetWorkingDays(firstDay, DateTime.Now) / 5);
                int averageOnFriday = db.MasterIncidents.ToList().Where(f => f.Timestamp.DayOfWeek == DayOfWeek.Friday).Count() / (GetWorkingDays(firstDay, DateTime.Now) / 5);

                return new Tuple<int, int, int, int, int>(averageOnMonday, averageOnTuesday, averageOnWednesday, averageOnThursday, averageOnFriday);
            }
        }

        public Tuple<int, int, int, int, int> GetAverageAnnouncementsPerWeekDay()
        {
            using (ShoutboxContext db = new ShoutboxContext())
            {
                //First day used to count the amount of days until now
                DateTime firstDay = db.Messages.OrderBy(f => f.Timestamp).FirstOrDefault().Timestamp;

                int averageOnMonday = db.Messages.ToList().Where(f => f.Timestamp.DayOfWeek == DayOfWeek.Monday && f.Type == "Announcement").Count() / (GetWorkingDays(firstDay, DateTime.Now) / 5);
                int averageOnTuesday = db.Messages.ToList().Where(f => f.Timestamp.DayOfWeek == DayOfWeek.Tuesday && f.Type == "Announcement").Count() / (GetWorkingDays(firstDay, DateTime.Now) / 5);
                int averageOnWednesday = db.Messages.ToList().Where(f => f.Timestamp.DayOfWeek == DayOfWeek.Wednesday && f.Type == "Announcement").Count() / (GetWorkingDays(firstDay, DateTime.Now) / 5);
                int averageOnThursday = db.Messages.ToList().Where(f => f.Timestamp.DayOfWeek == DayOfWeek.Thursday && f.Type == "Announcement").Count() / (GetWorkingDays(firstDay, DateTime.Now) / 5);
                int averageOnFriday = db.Messages.ToList().Where(f => f.Timestamp.DayOfWeek == DayOfWeek.Friday && f.Type == "Announcement").Count() / (GetWorkingDays(firstDay, DateTime.Now) / 5);

                return new Tuple<int, int, int, int, int>(averageOnMonday, averageOnTuesday, averageOnWednesday, averageOnThursday, averageOnFriday);
            }
        }

        public int GetWorkingDays(DateTime from, DateTime to)
        {
            var dayDifference = (int)to.Subtract(from).TotalDays;
            return Enumerable
                .Range(1, dayDifference)
                .Select(x => from.AddDays(x))
                .Count(x => x.DayOfWeek != DayOfWeek.Saturday && x.DayOfWeek != DayOfWeek.Sunday);
        }
    }
}