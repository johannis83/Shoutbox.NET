using System;
using System.Collections.Generic;
using System.DirectoryServices.AccountManagement;
using System.Linq;
using System.Web;

namespace Shoutbox.NET.Models
{
    public class ActiveDirectoryUser
    {
        public UserPrincipal UserPrincipal { get; set; }

        public string DomainName { get; set; }
    }
}