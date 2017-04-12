using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.DirectoryServices.AccountManagement;
using Shoutbox.NET.Models;

namespace Shoutbox.NET.Repositories
{
    public interface IUserPrincipalRepository
    {
        ActiveDirectoryUser GetByLogonUser(string logonUser);

    }
}
