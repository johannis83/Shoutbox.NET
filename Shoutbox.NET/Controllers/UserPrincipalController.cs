using Shoutbox.NET.Repositories;
using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.DirectoryServices.AccountManagement;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;

namespace Shoutbox.NET.Controllers
{
    public class UserPrincipalController : Controller, IUserPrincipalRepository
    {

        public UserPrincipalController()
        {
        }

        public UserPrincipal GetByLogonUser(string logonUser)
        {
            string domain = logonUser.Split('\\')[0];
            string username = logonUser.Split('\\')[1];

            // Enter AD settings, search in the domain the user is logged in from. Domain does not always equal the LDAP name so these are mapped in web.config
            PrincipalContext AD = new PrincipalContext(ContextType.Domain, WebConfigurationManager.AppSettings[domain].Split(',')[0]);

            // Create search user and add criteria  
            UserPrincipal u = new UserPrincipal(AD)
            {
                SamAccountName = username
            };

            // Search for user  
            using (PrincipalSearcher search = new PrincipalSearcher(u))
            {
                using (DirectorySearcher Searcher = search.GetUnderlyingSearcher() as DirectorySearcher)
                {
                    Searcher.PageSize = 1;
                    Searcher.PropertiesToLoad.Clear();
                    Searcher.PropertiesToLoad.Add("samaccountname");
                    Searcher.PropertiesToLoad.Add("displayname");
                    return (UserPrincipal)search.FindOne();
                }
            }
        }
    }
}