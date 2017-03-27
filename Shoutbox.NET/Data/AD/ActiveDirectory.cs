using System;
using System.Collections.Generic;
using System.DirectoryServices.AccountManagement;
using System.Linq;
using System.Web;
using System.Configuration;
using System.DirectoryServices;
using Shoutbox.NET.Services;

namespace Shoutbox.NET.Data.AD
{

    public class ActiveDirectory : IActiveDirectoryService
    {
        public UserPrincipal GetUser(string username)
        {
            // Enter AD settings  
            PrincipalContext AD = new PrincipalContext(ContextType.Domain, ConfigurationManager.AppSettings["ActiveDirectoryDomain"]);

            // Create search user and add criteria  
            UserPrincipal u = new UserPrincipal(AD);
            u.SamAccountName = username;

            // Search for user  
            using (PrincipalSearcher search = new PrincipalSearcher(u))
            {
                using (DirectorySearcher Searcher = search.GetUnderlyingSearcher() as DirectorySearcher)
                {
                    Searcher.PageSize = 1;
                    Searcher.PropertiesToLoad.Clear();
                    Searcher.PropertiesToLoad.Add("samaccountname");
                    Searcher.PropertiesToLoad.Add("displayname");
                    Searcher.PropertiesToLoad.Add("department");
                    return (UserPrincipal)search.FindOne();
                }
            }
        }
    }
}