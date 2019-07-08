using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BotClient.ViewModel.Interfaces
{
    public delegate void SendMessageHandler(object chat, object message);
    interface IChat:IChildVM
    {
        void SendMessage(object chat, object message);
    }
}
