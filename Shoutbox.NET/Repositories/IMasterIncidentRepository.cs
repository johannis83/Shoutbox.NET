using Shoutbox.NET.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoutbox.NET.Repositories
{
    interface IMasterIncidentRepository
    {
        MasterIncident Create(MasterIncident masterIncident);

        MasterIncident Delete(MasterIncident masterIncident);
    }
}
