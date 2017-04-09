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
using System.DirectoryServices.AccountManagement;
using System.Web.Configuration;
using Shoutbox.NET.Repositories;

namespace Shoutbox.NET.Controllers
{
    public class UserController : Controller, IUserRepository
    {
        private IUserPrincipalRepository _userPrincipalRepository;

        public UserController(IUserPrincipalRepository userPrincipalRepository)
        {
            _userPrincipalRepository = userPrincipalRepository;
        }

        public UserController()
        {
        }

        public User GetByLogonUser(string logonUser)
        {
            using (ShoutboxContext db = new ShoutboxContext())
            {
                string username = logonUser.Split('\\')[1];
                User user = db.Users.FirstOrDefault(f => f.Username == username);

                //If user doesn't exist yet, register them
                if (user == null) user = Create(logonUser);

                return user;
            }
        }

        public string GetGridLayout(string logonUser)
        {
            using (ShoutboxContext db = new ShoutboxContext())
            {
                string domain = logonUser.Split('\\')[0];
                string username = logonUser.Split('\\')[1];

                return db.Users.FirstOrDefault(f => f.Username == username).GridLayout;
            }
        }

        public void SaveGridLayout(string logonUser, string serializedLayout)
        {
            using (ShoutboxContext db = new ShoutboxContext())
            {
                string domain = logonUser.Split('\\')[0];
                string username = logonUser.Split('\\')[1];

                db.Users.FirstOrDefault(f => f.Username == username).GridLayout = serializedLayout;
                db.SaveChanges();
            }
        }

        //The domain of the logonUser has to be defined in the web.config under DomainName->LDAP mappings
        public User Create(string logonUser)
        {
            if (ModelState.IsValid)
            {
                using (ShoutboxContext db = new ShoutboxContext())
                {
                    string domain = logonUser.Split('\\')[0];
                    string username = logonUser.Split('\\')[1];

                    //Get the user's information from ActiveDirectory
                    UserPrincipal activeDirectoryUser = _userPrincipalRepository.GetByLogonUser(logonUser);

                    //Store the information in a database for faster processing
                    User user = new User
                    {
                        Name = activeDirectoryUser.GivenName + " " + activeDirectoryUser.Surname,
                        Domain = WebConfigurationManager.AppSettings[domain].Split(',')[0],
                        Username = username,
                        Division = WebConfigurationManager.AppSettings[domain].Split(',')[1],
                        Role = Roles.User //Make a new user a normal user by default
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
