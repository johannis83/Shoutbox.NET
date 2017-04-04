using Shoutbox.NET.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Shoutbox.NET.Repositories
{
    public interface ITeamRepository
    {
        Team Set(Team Team);
    }
}