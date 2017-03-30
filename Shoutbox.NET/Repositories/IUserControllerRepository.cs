using Shoutbox.NET.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoutbox.NET.Services
{
    public interface IUserRepository
    {
        User Create(string username);
        User GetByLogonUser(string username);
    }
}
