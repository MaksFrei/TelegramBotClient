using NHibernate;
using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace BotClient.Models
{
    public class Chat : Entity
    {
        #region Properties
        private static ObservableCollection<Chat> _chats = new ObservableCollection<Chat>();
        private  ObservableCollection<Message> _messagesCollection = new ObservableCollection<Message>();
        private string _name;
        private ISet<Message> _messages = new HashSet<Message>();      

        public virtual string Name
        {
            get
            {
                return _name;
            }
            set
            {
                _name = value;
                OnPropertyChanged("Name");
            }
        }
        public virtual ISet<Message> Messages
        {
            get
            {
                return _messages;
            }
            set
            {
                _messagesCollection.Clear();
                foreach(Message mes in value)
                {
                    _messagesCollection.Add(mes);
                }

                _messages = value;
                OnPropertyChanged("Messages");
            }
        }
        public virtual ObservableCollection<Message> MessagesCollection
        {
            get
            {
                return _messagesCollection;
            }
            set
            {
                _messagesCollection = value;
                OnPropertyChanged("Messages");
            }
        }
        #endregion

        #region Constructors
        public Chat()
        {

        }
        public Chat(int id, string name, ISession session)
        {
            using (ITransaction transaction = session.BeginTransaction())
            {
                Id = id;
                Name = name;
                session.SaveOrUpdate(this);
                transaction.Commit();
            }

        }
        #endregion

        #region Methods
        public virtual void AddMessage(Message message, ISession session)
        {
                if (message == null)
                    throw new ArgumentNullException("message");
                if (!_messages.Contains(message))
                {
                    _messagesCollection.Add(message);
                    _messages.Add(message);

                    session.SaveOrUpdate(this);
                    session.Save(message);
                }
        }
        public static ObservableCollection<Chat> GetChats()
        {
            IList<Chat> temp;
            _chats.Clear();
            using (ISession session = NHibernateHelper.OpenSession())
            {
               temp = session.CreateCriteria<Chat>().List<Chat>();
            }
            foreach (Chat chat in temp)
            {
                _chats.Add(chat);
            }
            return _chats;
        }
        #endregion
    }
    public class ChatMap : ClassMapping<Chat>
    {
        #region Map
        public ChatMap()
        {
            Table("Chats");

            Id(x => x.Id, map => map.Column("IdChat"));
            Property(x => x.Name, map => map.Column("Name"));
            Set(m => m.Messages,
                map =>
                {
                    map.Key(k => k.Column("IdChat"));
                    map.Inverse(true);
                    map.Table("Messages");
                },
                r => r.OneToMany());
        }
        #endregion
    }
}
