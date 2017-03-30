using Shoutbox.NET.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoutbox.NET.Services
{
    public interface IMessageRepository
    {
        Message Create(Message message);
    }
}
