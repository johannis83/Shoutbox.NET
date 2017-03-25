using System;
using System.Collections.Generic;
using System.DirectoryServices.AccountManagement;
using System.Linq;
using System.Web;
using System.Configuration;
using System.DirectoryServices;

namespace Shoutbox.NET.Data.AD
{

    public class ActiveDirectory : IActiveDirectory
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
                (search.GetUnderlyingSearcher() as DirectorySearcher).PropertiesToLoad.Clear();
                (search.GetUnderlyingSearcher() as DirectorySearcher).PropertiesToLoad.Add("samaccountname");
                (search.GetUnderlyingSearcher() as DirectorySearcher).PropertiesToLoad.Add("displayname");
                (search.GetUnderlyingSearcher() as DirectorySearcher).PropertiesToLoad.Add("department");
                return (UserPrincipal)search.FindOne();
            }
        }
    }
}