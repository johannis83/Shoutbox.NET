using Shoutbox.NET.Data;
using Shoutbox.NET.Models;
using Shoutbox.NET.Repositories;
using Shoutbox.NET.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Shoutbox.NET.Controllers
{
    public class MainController : Controller
    {
        private IMessageRepository _messageRepository;
        private ITeamRepository _teamRepository;
        private IMasterIncidentRepository _masterIncidentRepository;
        private ISOSRepository _sosRepository;
        private IUserRepository _userRepository;
        private IUserPrincipalRepository _userPrincipalRepository;
        private IKMRepository _kmRepository;
        private IShoutboxUsageRepository _shoutboxUsageRepository;

        public MainController(IMessageRepository messageRepository, ITeamRepository teamRepository,
            IMasterIncidentRepository masterIncidentRepository, ISOSRepository sosRepository, IUserRepository userRepository,
            IKMRepository kmRepository, IUserPrincipalRepository userPrincipalRepository,
            IShoutboxUsageRepository shoutboxUsageRepository)
        {
            _messageRepository = messageRepository;
            _teamRepository = teamRepository;
            _masterIncidentRepository = masterIncidentRepository;
            _sosRepository = sosRepository;
            _userRepository = userRepository;
            _kmRepository = kmRepository;
            _userPrincipalRepository = userPrincipalRepository;
            _shoutboxUsageRepository = shoutboxUsageRepository;
        }

        public ActionResult Index()
        {
            string userLogon = User.Identity.Name;
            User currentUser = _userRepository.GetByLogonUser(userLogon);

            //If user isn't known yet in our database, get their data from ActiveDirectory and Create them in the database
            if (currentUser == null)
                currentUser = _userRepository.CreateFromUserPrincipal(_userPrincipalRepository.GetByLogonUser(userLogon));

            //Only get todays objects for the homepage
            DateTime pageDate = DateTime.Now;
            ShoutPageViewModel indexViewModel = new ShoutPageViewModel()
            {
                Messages = _messageRepository.GetByDay(DateTime.Now),
                SOSList = _sosRepository.GetList(),
                Tags = _messageRepository.GetTagPopularityByDay(pageDate),
                Teams = _teamRepository.GetByDay(pageDate),
                MasterIncidents = _masterIncidentRepository.GetByDay(pageDate).Where(f => f.Active),
                CurrentUser = currentUser,
                KMList = _kmRepository.GetList()
            };
            return View(indexViewModel);
        }

        [HttpGet]
        public ActionResult Historie(string date)
        {
            string userLogon = User.Identity.Name;
            User currentUser = _userRepository.GetByLogonUser(userLogon);
            DateTime pageDate;

            if (string.IsNullOrWhiteSpace(date))
                //Show yesterday by default
                pageDate = DateTime.Now.AddDays(-1);
            else
            {
                if (!DateTime.TryParseExact(date, "d-M-yyyy", DateTimeFormatInfo.InvariantInfo, DateTimeStyles.None, out pageDate))
                {
                    pageDate = DateTime.Now;
                }
            }


            ShoutPageViewModel historyViewModel = new ShoutPageViewModel()
            {
                CurrentUser = currentUser,
                Messages = _messageRepository.GetByDay(pageDate),
                Tags = _messageRepository.GetTagPopularityByDay(pageDate),
                Teams = _teamRepository.GetByDay(pageDate),
                MasterIncidents = _masterIncidentRepository.GetByDay(pageDate).Where(f => f.Active),
                HistoryViewDate = pageDate
            };
            return View(historyViewModel);
        }

        public ActionResult Tag(string tag)
        {
            string userLogon = User.Identity.Name;
            User currentUser = _userRepository.GetByLogonUser(userLogon);
            ShoutPageViewModel tagViewModel = new ShoutPageViewModel()
            {
                CurrentUser = currentUser,
                Messages = _messageRepository.GetByDay(DateTime.Now).Where(f => f.Tag.ToLower() == tag.ToLower())
            };
            return View(tagViewModel);

        }

        public ActionResult Monitor()
        {
            string userLogon = User.Identity.Name;
            User currentUser = _userRepository.GetByLogonUser(userLogon);

            ShoutPageViewModel monitorViewModel = new ShoutPageViewModel()
            {
                CurrentUser = currentUser
            };

            //Only users with a role higher than a normal user can access the status monitor
            if (currentUser.Role < Roles.Moderator)
            {
                return View("Unauthorized", monitorViewModel);
            }

            monitorViewModel.DataDistribution = new ShoutboxStatistics()
            {
                UsagePerTypeToday = _shoutboxUsageRepository.GetDataDistributionByDay(DateTime.Now),
                AverageMasterIncident = _shoutboxUsageRepository.AverageWeeklyMessages("MasterIncident"),
                AverageChatMessages = _shoutboxUsageRepository.AverageWeeklyMessages("Chat"),
                AverageAnnouncementMessages = _shoutboxUsageRepository.AverageWeeklyMessages("Announcement"),
                OnlineUsers = _shoutboxUsageRepository.GetOnlineUserCount(),
                MostPopularTags = _shoutboxUsageRepository.MostPopularTags(),
               
            };
            return View(monitorViewModel);
        }

        public ActionResult Demo()
        {

            ShoutPageViewModel svm = new ShoutPageViewModel()
            {
                CurrentUser = _userRepository.GetByLogonUser(User.Identity.Name)
            };

            return View(svm);
        }
    }
}
