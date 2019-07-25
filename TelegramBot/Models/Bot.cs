using TelegramBot.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using TelegramBot.Models.Commands;

namespace TelegramBot.Models
{
    public class Bot : IMessaneger
    {
        #region Properties
        private static Bot Instance { get; set; }
        private static WebProxy proxy;
        private static List<WebProxy> proxyList = new List<WebProxy>();
        private static List<Command> commandsList = new List<Command>();

        public static IReadOnlyList<Command> Commands { get => commandsList.AsReadOnly(); }
        public static bool Online { get; set; }
        public TelegramBotClient Client { get; set; }

        public event BotMessageGetHandler OnMessageCome;
        public event BotMessageSendHandler OnMessageSend;
        public event BotConnectedHandler Connected;
        #endregion

        #region Constructors
        static Bot()
        {
            commandsList.Add(new HelloCommand());
            GetProxyList();
            proxy = proxyList[0];
        }
        private Bot()
        {
            InitializeAsync(proxy);
        }

        public static Bot GetBot()
        {
            if (Instance == null)
                Instance = new Bot();
            return Instance;
        }
        #endregion

        #region Methods
        private async Task InitializeAsync(WebProxy proxy)
        {
            Client = new TelegramBotClient(BotSettings.Key, proxy);
            await Client.DeleteWebhookAsync();
        }
        public async Task RunAsync()
        {
            try
            {
                var me = await Client.GetMeAsync();
                Online = true;

                Connected?.Invoke(Online);

                var offset = 0;
                while (true)
                {
                    var updates = await Client.GetUpdatesAsync(offset);

                    foreach (var update in updates)
                    {
                        var message = update.Message;
                        if (message.Type == MessageType.Text)
                        {
                            await GetMessageAsync(message);
                        }

                        offset = update.Id + 1;
                    }
                }
            }
            catch (Exception ex)
            {
                await ReconnectAsync(ex);
                await RunAsync();
            }
        }
        public async Task GetMessageAsync(object message)
        {
            var mes = new
            {
                Text = ((Message)message).Text,
                IdChat = ((Message)message).Chat.Id,
                IdUser = ((Message)message).From.Id
            };

            var user = new
            {
                Id = ((Message)message).From.Id,
                Name = ((Message)message).From.Username ?? "",
                LastName = ((Message)message).From.LastName ?? "",
                FirstName = ((Message)message).From.FirstName ?? ""
            };

            var chat = new
            {
                Id = ((Message)message).Chat.Id,
                Name = ((Message)message).Chat.Title ?? user.LastName + " " + user.FirstName
            };

            OnMessageCome?.Invoke(chat,mes,user);

            foreach (var command in Commands)
            {
                if (command.Contains((Message)message))
                {
                    await command.Execute((Message)message, Client);
                    break;
                }
            }

        }
        public async Task SendMessageAsync(object Chat, object message, object user = null)
        {
            try
            {

                var ch = new
                {
                    Id = Convert.ToInt32(Chat.GetType().GetProperty("Id").GetValue(Chat).ToString()),
                    Name = Chat.GetType().GetProperty("Name").GetValue(Chat).ToString()
                };
                var mes = new
                {
                    Text = (string)message.GetType().GetProperty("Text").GetValue(message),
                    IdChat = ch.Id
                };
                var bot = new
                {
                    Id = Client.BotId,
                    Name = BotSettings.Name,
                    LastName = "",
                    FirstName = ""
                };

                await Client.SendChatActionAsync(ch.Id, ChatAction.Typing);
                Task.Delay(1000).Wait();
                await Client.SendTextMessageAsync(ch.Id, mes.Text);

                OnMessageSend?.Invoke(ch, mes, bot);
            }
            catch (Exception ex)
            {
                await ReconnectAsync(ex);
                await SendMessageAsync(Chat, message, user);
            }

        }
        private async Task ReconnectAsync(Exception ex)
        {
            Online = false;
            Connected?.Invoke(Online);
            proxyList.RemoveAt(0);

            if (proxyList.Count == 0)
            {
                GetProxyList();
            }

            proxy = proxyList[0];
            await InitializeAsync(proxy);
            Console.WriteLine(ex);
        }
        private static void GetProxyList()
        {
            proxyList.Clear();
            try
            {
                using (WebClient wc = new WebClient())
                {
                    string response = wc.DownloadString("https://www.sslproxies.org");
                    Regex regex = new Regex(@"\d+(.)\d+(.)\d+(.)\d+(</td><td>)\d+");
                    MatchCollection mc = regex.Matches(response);

                    if (mc.Count > 0)
                    {
                        foreach (Match item in mc)
                        {
                            string[] split = item.Value.Split(new string[] { "</td><td>" }, StringSplitOptions.None);
                            proxyList.Add(new WebProxy(split[0], int.Parse(split[1])));
                        }
                    }
                }
            }
            catch
            {
                GetProxyList();
            }
        }
        #endregion
    }
}
