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
        private IKMRepository _kmRepository;

        public MainController(IMessageRepository messageRepository, ITeamRepository teamRepository, 
            IMasterIncidentRepository masterIncidentRepository, ISOSRepository sosRepository, IUserRepository userRepository,
            IKMRepository kmRepository)
        {
            _messageRepository = messageRepository;
            _teamRepository = teamRepository;
            _masterIncidentRepository = masterIncidentRepository;
            _sosRepository = sosRepository;
            _userRepository = userRepository;
            _kmRepository = kmRepository;
        }

        public ActionResult Index()
        {
            //Only get todays objects for the homepage
            DateTime pageDate = DateTime.Now;
            ShoutPageViewModel indexViewModel = new ShoutPageViewModel()
            {
                Messages = _messageRepository.GetByDay(pageDate),
                SOSList = _sosRepository.GetList(),
                Tags = _messageRepository.GetTagPopularityByDay(pageDate),
                Teams = _teamRepository.GetByDay(pageDate),
                MasterIncidents = _masterIncidentRepository.GetByDay(pageDate).Where(f => f.Active),
                CurrentUser = _userRepository.GetByLogonUser(User.Identity.Name), //Also registers the user if they don't exist yet
                KMList = _kmRepository.GetList()
            };
            return View(indexViewModel);
        }

        [HttpGet]
        public ActionResult Historie(string date)
        {
            DateTime pageDate;

            if(string.IsNullOrWhiteSpace(date))
                //Show yesterday by default
                pageDate = DateTime.Now.AddDays(-1);
            else
            {
                if(!DateTime.TryParseExact(date, "d-M-yyyy", DateTimeFormatInfo.InvariantInfo, DateTimeStyles.None, out pageDate))
                {
                    pageDate = DateTime.Now;
                }
            }


            ShoutPageViewModel historyViewModel = new ShoutPageViewModel()
            {
                Messages = _messageRepository.GetByDay(pageDate),
                Tags = _messageRepository.GetTagPopularityByDay(pageDate),
                Teams = _teamRepository.GetByDay(pageDate),
                MasterIncidents = _masterIncidentRepository.GetByDay(pageDate).Where(f => f.Active),
                HistoryViewDate = pageDate
            };
            return View(historyViewModel);
        }
    }
}