using Shoutbox.NET.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoutbox.NET.Services
{
    public interface IUserControllerService
    {
        User Create(string username);
        User GetByUsername(string username);
    }
}
