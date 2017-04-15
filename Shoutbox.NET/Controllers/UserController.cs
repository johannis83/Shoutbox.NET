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
using Newtonsoft.Json.Linq;
using Shoutbox.NET.Misc;

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

                return user;
            }
        }

        public string GetGridLayout(string logonUser)
        {
            using (ShoutboxContext db = new ShoutboxContext())
            {
                string username = logonUser.Split('\\')[1];

                return db.Users.FirstOrDefault(f => f.Username == username).GridLayout;
            }
        }

        public void SaveGridLayout(string logonUser, string serializedLayout)
        {
            using (ShoutboxContext db = new ShoutboxContext())
            {
                string username = logonUser.Split('\\')[1];

                db.Users.FirstOrDefault(f => f.Username == username).GridLayout = serializedLayout;
                db.SaveChanges();
            }
        }

        public void SaveUserPreferences(string logonUser, string serializedSettings)
        {
            using (ShoutboxContext db = new ShoutboxContext())
            {
                string username = logonUser.Split('\\')[1];

                //Validate the entered json
                if (!HelperFunctions.IsValidJson(serializedSettings)) return;

                db.Users.FirstOrDefault(f => f.Username == username).UserPreferences = serializedSettings;
                db.SaveChanges();
            }
        }

        //The domain of the logonUser has to be defined in the web.config under DomainName->LDAP mappings
        public User CreateFromUserPrincipal(ActiveDirectoryUser activeDirectoryUser)
        {
            using (ShoutboxContext db = new ShoutboxContext())
            {
                //Store the information in a database for faster processing
                User user = new User
                {
                    Name = activeDirectoryUser.UserPrincipal.GivenName + " " + activeDirectoryUser.UserPrincipal.Surname,
                    Domain = WebConfigurationManager.AppSettings[activeDirectoryUser.DomainName].Split(',')[0], 
                    Username = activeDirectoryUser.UserPrincipal.SamAccountName,
                    Division = WebConfigurationManager.AppSettings[activeDirectoryUser.DomainName].Split(',')[1], //Set Division based on the mapping found in web.config
                    UserPreferences = "{\"Team\":true,\"Masterincidenten\":true,\"Sos\":true,\"Meldingen\":true,\"Chat\":true}", //Enable notifications by default
                    Role = Roles.Moderator //Make a new user a normal user by default
                };

                db.Users.Add(user);
                db.SaveChanges();
                return user;
            }
        }
    }
}
