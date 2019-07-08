using BotClient.Models;
using BotClient.ViewModel.Interfaces;
using DevExpress.Mvvm;
using System.ComponentModel;
using System.Linq;

namespace BotClient.ViewModel
{
    public class ChatViewModel : BaseViewModel, INotifyPropertyChanged, IChat
    {
        #region Properties
        private Chat _activeChat;
        private string _text = "";
        private Message _lastMessage;

        public event VMClosedHandler Closed;
        public event SendMessageHandler MessageSended;

        public MainViewModel Owner { get; set; }
        public string Text
        {
            get
            {
                return _text;
            }
            set
            {
                _text = value;
                OnPropertyChanged(Text);
            }
        }
        public Chat ActiveChat
        {
            get
            {
                return _activeChat;
            }
            set
            {
                _activeChat = value;
                OnPropertyChanged("ActiveChat");
            }
        }
        public Message LastMessage
        {
            get
            {
                return _lastMessage;
            }
            set
            {
                _lastMessage = value;
                OnPropertyChanged("LastMessage");
            }
        }
        public int Id { get; set; }
        #endregion

        #region Constructors
        public ChatViewModel(Chat chat, MainViewModel owner)
        {
            Id = chat.Id;
            ActiveChat = chat;
            Owner = owner;
            LastMessage = chat.MessagesCollection.LastOrDefault();
        }
        #endregion

        #region Methods
        public void Close(ChatViewModel viewModel)
        {
            Closed?.Invoke(viewModel);
        }
        public void SendMessage(object chat, object message)
        {
            MessageSended?.Invoke(chat, message);
        }
        #endregion

        #region Commands
        public DelegateCommand<Chat> CloseView => new DelegateCommand<Chat>(chat =>
        {
            Close(this);
        });
        public DelegateCommand Send => new DelegateCommand(() =>
        {
            var message = new
            {
                Text
            };
            SendMessage(ActiveChat, message);
            Text = "";
        });

        #endregion
    }
}
