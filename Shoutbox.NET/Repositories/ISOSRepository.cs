using Shoutbox.NET.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoutbox.NET.Repositories
{
    public interface ISOSRepository
    {
        void UpdateSOSList();
        List<SOS> GetList();
    }
}
