using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BotClient.Models
{
    public class Message :Entity
    {
        #region Properties
        private string _text;
        private Chat _chat;
        private User _user;

        public virtual string Text
        {
            get
            {
                return _text;
            }
            set
            {
                _text = value;
                OnPropertyChanged("Text");
            }
        }
        public virtual Chat Chat
        {
            get
            {
                return _chat;
            }
            set
            {
                _chat = value;
                OnPropertyChanged("Chat");
            }
        }
        public virtual User User
        {
            get
            {
                return _user;
            }
            set
            {
                _user = value;
                OnPropertyChanged("User");
            }
        }
        #endregion

        #region Constructors
        public Message()
        {

        }
        public Message(string text, Chat chat, User user)
        {
            Text = text;
            Chat = chat;
            User = user;
        }
        #endregion
    }
    public class MessageMap : ClassMapping<Message>
    {
        #region Map
        public MessageMap()
        {
            Table("Messages");

            Id(x => x.Id, map =>
            {
                map.Generator(Generators.Native);
                map.Column("IdMessage");
            });
            Property(x => x.Text);
            ManyToOne(x => x.Chat,
            map =>
            {
                map.Cascade(Cascade.Persist);
                map.Column("IdChat");
                map.Fetch(FetchKind.Join);
            });
            ManyToOne(x => x.User,
            map =>
            {
                map.Cascade(Cascade.Persist);
                map.Column("IdUser");
            });
        }
        #endregion
    }
}
