using Shoutbox.NET.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoutbox.NET.Repositories
{
    public interface IMessageRepository
    {
        Message Create(Message message);
        IEnumerable<Message> GetByDay(DateTime date);
        List<Tag> GetTagPopularityByDay(DateTime datetime);
        Message ToggleMessageRelevance(int id);
    }
}
