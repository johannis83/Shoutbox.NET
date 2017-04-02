using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.DirectoryServices.AccountManagement;

namespace Shoutbox.NET.Repositories
{
    public interface IUserPrincipalRepository
    {
        UserPrincipal GetByLogonUser(string logonUser);

    }
}
