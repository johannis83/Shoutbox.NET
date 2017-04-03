using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Shoutbox.NET.Data;
using Shoutbox.NET.Models;
using Shoutbox.NET.Data.AD;
using System.DirectoryServices.AccountManagement;
using Shoutbox.NET.Services;

namespace Shoutbox.NET.Controllers
{
    public class UserController : Controller, IUserControllerService
    {
        private IActiveDirectoryService _ActiveDirectoryService;

        public UserController()
        {
        }

        public UserController(IActiveDirectoryService service)
        {
            _ActiveDirectoryService = service;
        }

        public User GetByUsername(string username)
        {
            using (ShoutboxContext db = new ShoutboxContext())
            {
                User user = db.Users.FirstOrDefault(f => f.Username == username);
                return user;
            }
        }


        public User Create(string username)
        {
            if (ModelState.IsValid)
            {
                using (ShoutboxContext db = new ShoutboxContext())
                {
                    //Get the user's information from ActiveDirectory
                    UserPrincipal activeDirectoryUser = _ActiveDirectoryService.GetUser(username);

                    //Store the information in a database for faster processing
                    User user = new User
                    {
                        Name = activeDirectoryUser.DisplayName,
                        Domain = username.Split('\\')[0],
                        Username = username.Split('\\')[1],
                        Division = Division.NL
                    };

                    db.Users.Add(user);
                    db.SaveChanges();
                    return user;
                }
            }

            return null;
        }
    }
}
