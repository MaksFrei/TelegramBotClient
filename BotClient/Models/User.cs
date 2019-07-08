using NHibernate;
using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BotClient.Models
{
    public class User : Entity
    {
        #region Properties
        private string _lastName;
        private string _firstName;
        private string _nickName;
        private ISet<Message> _messages;

        public virtual string LastName
        {
            get
            {
                return _lastName;
            }
            set
            {
                _lastName = value;
                OnPropertyChanged("LastName");
            }
        }
        public virtual string FirstName
        {
            get
            {
                return _firstName;
            }
            set
            {
                _firstName = value;
                OnPropertyChanged("FirstName");
            }
        }
        public virtual string NickName
        {
            get
            {
                return _nickName;
            }
            set
            {
                _nickName = value;
                OnPropertyChanged("NickName");
            }
        }
        public virtual ISet<Message> Messages
        {
            get
            {
                return _messages ?? (_messages = new HashSet<Message>());
            }
            set
            {
                _messages = value;
                OnPropertyChanged("Messages");
            }
        }
        #endregion

        #region Constructors
        public User()
        {

        }
        public User(int id, string nickName, string lastName, string firstName, ISession session)
        {
            using (ITransaction transaction = session.BeginTransaction())
            {
                Id = id;
                NickName = nickName;
                LastName = lastName;
                FirstName = firstName;
                session.SaveOrUpdate(this);
                transaction.Commit();
            }
        }
        #endregion

        #region Methods

        #endregion
    }

    public class UserMap : ClassMapping<User>
    {
        #region Map
        public UserMap()
        {
            Table("Users");

            Id(x => x.Id, map => map.Column("IdUser"));
            Property(x => x.LastName);
            Property(x => x.FirstName);
            Property(x => x.NickName);
            Set(m => m.Messages,
                map =>
                {
                    map.Key(k => k.Column("IdUser"));
                    map.Inverse(true);
                    map.Table("Messages");
                },
                r => r.OneToMany());
        }
        #endregion
    }
}
