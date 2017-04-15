using Microsoft.Ajax.Utilities;
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

        public List<Tag> MostPopularTags()
        {
            var popularTags = new List<Tag>();

            using (ShoutboxContext db = new ShoutboxContext())
            {
                db.Messages.Where(f => f.Type == "Announcement").GroupBy(t => t.Tag).Select(g => g.FirstOrDefault()).ToList().
                    ForEach(i => popularTags.Add(new Tag { Name = i.Tag, Count = db.Messages.Count(x => x.Tag == i.Tag) }));

                return popularTags.OrderByDescending(f => f.Count).ToList();
            }
        }


        public WeeklyStatistic AverageWeeklyMessages(string type)
        {
            using (ShoutboxContext db = new ShoutboxContext())
            {
                DateTime startDate = db.Messages.OrderBy(f => f.Timestamp).FirstOrDefault().Timestamp;
                int dayDifferential = Convert.ToInt32((DateTime.Now - startDate).TotalDays);

                if (type == "MasterIncident")
                {
                    List<MasterIncident> masterIncidents = db.MasterIncidents.ToList();

                    WeeklyStatistic weeklyMasterIncidentStatistic = new WeeklyStatistic
                    {
                        Monday = masterIncidents.Where(f => f.Timestamp.DayOfWeek == DayOfWeek.Monday).Count() / (dayDifferential / 7),
                        Tuesday = masterIncidents.Where(f => f.Timestamp.DayOfWeek == DayOfWeek.Tuesday).Count() / (dayDifferential / 7),
                        Wednesday = masterIncidents.Where(f => f.Timestamp.DayOfWeek == DayOfWeek.Wednesday).Count() / (dayDifferential / 7),
                        Thursday = masterIncidents.Where(f => f.Timestamp.DayOfWeek == DayOfWeek.Thursday).Count() / (dayDifferential / 7),
                        Friday = masterIncidents.Where(f => f.Timestamp.DayOfWeek == DayOfWeek.Friday).Count() / (dayDifferential / 7),
                        Saturday = masterIncidents.Where(f => f.Timestamp.DayOfWeek == DayOfWeek.Saturday).Count() / (dayDifferential / 7),
                        Sunday = masterIncidents.Where(f => f.Timestamp.DayOfWeek == DayOfWeek.Sunday).Count() / (dayDifferential / 7)
                    };

                    return weeklyMasterIncidentStatistic;
                }

                List<Message> messages = db.Messages.Where(f => f.Type == type).ToList();
                

                WeeklyStatistic weeklyStatistic = new WeeklyStatistic
                {
                    Monday = messages.Where(f => f.Timestamp.DayOfWeek == DayOfWeek.Monday).Count() / (dayDifferential / 7),
                    Tuesday = messages.Where(f => f.Timestamp.DayOfWeek == DayOfWeek.Tuesday).Count() / (dayDifferential / 7),
                    Wednesday = messages.Where(f => f.Timestamp.DayOfWeek == DayOfWeek.Wednesday).Count() / (dayDifferential / 7),
                    Thursday = messages.Where(f => f.Timestamp.DayOfWeek == DayOfWeek.Thursday).Count() / (dayDifferential / 7),
                    Friday = messages.Where(f => f.Timestamp.DayOfWeek == DayOfWeek.Friday).Count() / (dayDifferential / 7),
                    Saturday = messages.Where(f => f.Timestamp.DayOfWeek == DayOfWeek.Saturday).Count() / (dayDifferential / 7),
                    Sunday = messages.Where(f => f.Timestamp.DayOfWeek == DayOfWeek.Sunday).Count() / (dayDifferential / 7)
                };

                return weeklyStatistic;
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