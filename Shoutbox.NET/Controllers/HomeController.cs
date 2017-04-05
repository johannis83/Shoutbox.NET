using Shoutbox.NET.Data;
using Shoutbox.NET.Models;
using Shoutbox.NET.Repositories;
using Shoutbox.NET.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Shoutbox.NET.Controllers
{
    public class HomeController : Controller
    {
        private IMessageRepository _messageRepository;
        private ITeamRepository _teamRepository;
        private IMasterIncidentRepository _masterIncidentRepository;

        public HomeController(IMessageRepository messageRepository, ITeamRepository teamRepository, IMasterIncidentRepository masterIncidentRepository)
        {
            _messageRepository = messageRepository;
            _teamRepository = teamRepository;
            _masterIncidentRepository = masterIncidentRepository;
        }

        public ActionResult Index()
        {
            DateTime pageDate = DateTime.Now;
            //Only get todays objects for the homepage
            IndexViewModel indexViewModel = new IndexViewModel()
            {
                Messages = _messageRepository.GetByDay(pageDate),
                Tags = _messageRepository.GetTagPopularityByDay(pageDate),
                Teams = _teamRepository.GetByDay(pageDate),
                MasterIncidents = _masterIncidentRepository.GetByDay(pageDate)

            };
            return View(indexViewModel);
        }

        public ActionResult Historie()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}