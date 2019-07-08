using BotClient.Models;
using DevExpress.Mvvm;
using NHibernate;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using TelegramBot.Models.Interfaces;
using System.Linq;
using BotClient.Interfaces.ViewModel;

namespace BotClient.ViewModel
{
    public class MainViewModel : BaseViewModel
    {
        #region Properties
        private readonly IDialogManager _dialogManager;
        private readonly IMessaneger _messaneger;

        private ObservableCollection<Chat> _chats = new ObservableCollection<Chat>();
        private ChatViewModel _openChat;
        private Chat _activeChat;
        private string _connectionStatus = "Connecting...";

        public ObservableCollection<Chat> Chats
        {
            get
            {
                return _chats;
            }
            set
            {
                _chats = value;
                OnPropertyChanged("Chats");
            }
        }
        public ChatViewModel OpenedChat
        {
            get
            {
                return _openChat;
            }
            set
            {
                _openChat = value;
                OnPropertyChanged("OpenChats");
            }
        }
        public Chat ActiveChat { get
            {
                return _activeChat;
            }
            set
            {
                    _activeChat = value;
                    OnPropertyChanged("ActiveChat");
            }
        }
        public string ConnectionStatus
        {
            get
            {
                return _connectionStatus;
            }
            set
            {
                _connectionStatus = value;
                OnPropertyChanged("ConnectionStatus");
            }
        }
        #endregion

        #region Constructors
        public MainViewModel(IDialogManager dialogManager, IMessaneger messaneger)
        {

            _dialogManager = dialogManager;
            _messaneger = messaneger;

            Chats = Chat.GetChats();

            _messaneger.OnMessageCome += ReceiveMessage;
            _messaneger.OnMessageSend += ReceiveMessage;
            _messaneger.Connected += ConnectionStatusChanged;
            RunBot();

        }
        #endregion

         #region Methods
        private async void RunBot()
        {
            try
            {
                await _messaneger.RunAsync();
            }
            catch
            {
                RunBot();
            }
        }
        private void ReceiveMessage(object chat, object message, object user)
        {
            int ChatId = Convert.ToInt32(chat.GetType().GetProperty("Id").GetValue(chat).ToString());
            string ChatName = chat.GetType().GetProperty("Name").GetValue(chat).ToString();

            int UserId = Convert.ToInt32(user.GetType().GetProperty("Id").GetValue(user));
            string UserNickName = user.GetType().GetProperty("Name").GetValue(user).ToString();
            string UserLastName = user.GetType().GetProperty("LastName").GetValue(user).ToString();
            string UserFirstName = user.GetType().GetProperty("FirstName").GetValue(user).ToString();

            string MessageText = (string)message.GetType().GetProperty("Text").GetValue(message);

            try
            {
                using (ISession session = NHibernateHelper.OpenSession())
                {
                    Chat ch = new Chat(ChatId, ChatName, session);
                    User us = new User(UserId, UserNickName, UserLastName, UserFirstName, session);
                    Message ms = new Message(MessageText, ch, us);
                    using (ITransaction transaction = session.BeginTransaction())
                    {
                        ch.AddMessage(ms, session);
                        transaction.Commit();
                    }

                    if (ExistsOpenChat(ch.Id))
                    {
                        OpenedChat.ActiveChat.MessagesCollection.Add(ms);
                        OpenedChat.LastMessage = ms;
                    }
                    if (Chats.SingleOrDefault(c => c.Id == ch.Id) == null)
                        Chats.Add(ch);
                }
            }
            catch
            {
                throw;
            }
        }
        private void SendMessage(object chat, object message)
        {
            _messaneger.SendMessageAsync(chat, message);
        }
        private bool ExistsOpenChat(int id)
        {
            if (OpenedChat != null && OpenedChat.Id == id)
                return true;
            else
                return false;
        }
        public void ConnectionStatusChanged(bool status)
        {
            if (status)
                ConnectionStatus = "Online";
            else
                ConnectionStatus = "Connecting...";
        }
        public void ChatOpen(Chat chat)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            using (ITransaction transaction = session.BeginTransaction())
            {
                OpenedChat?.Close(OpenedChat);
                OpenedChat = new ChatViewModel(session.Load<Chat>(chat.Id), this);
                OpenedChat.Closed += ChatClose;
                OpenedChat.MessageSended += SendMessage;
                _dialogManager.Show(OpenedChat);
            }
        }
        public void ChatClose(ChatViewModel viewModel)
        {
            _dialogManager.Close(viewModel);
            OpenedChat = null;
        }
        #endregion

        #region Commands
        public DelegateCommand<Chat> ChatSelected => new DelegateCommand<Chat>(chat =>
        {
            ChatOpen(chat);
        });
        #endregion
    }
}