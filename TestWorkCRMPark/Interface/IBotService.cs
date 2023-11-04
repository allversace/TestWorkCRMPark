using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace TestWorkCRMPark.Interface
{
    public interface IBotService
    {
        Task StartBot();
        Task<string> ProcessMessage(Message message);
    }
}
