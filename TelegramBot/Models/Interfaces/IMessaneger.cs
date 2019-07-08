using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TelegramBot.Models.Interfaces
{
    public delegate void BotMessageGetHandler(object chat, object message, object user);

    public delegate void BotMessageSendHandler(object chat, object message, object user = null);

    public delegate void BotConnectedHandler(bool online);
    public interface IMessaneger
    {
        Task GetMessageAsync(object message);

        Task SendMessageAsync(object chat, object message, object user = null);
        Task RunAsync();

        event BotMessageGetHandler OnMessageCome;

        event BotMessageSendHandler OnMessageSend;

        event BotConnectedHandler Connected;
    }
}
