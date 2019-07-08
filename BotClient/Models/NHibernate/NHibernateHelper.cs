using NHibernate;
using NHibernate.Cfg;
using NHibernate.Cfg.MappingSchema;
using NHibernate.Dialect;
using NHibernate.Mapping.ByCode;
using NHibernate.Tool.hbm2ddl;
using System.Data.SQLite;
using System.IO;
using System.Reflection;

namespace BotClient.Models
{
    public class NHibernateHelper
    {
        private static readonly string db = "BotDB.db";
        private static readonly string connectionString = string.Format(@"Data Source = {0}", db);
        public static ISession OpenSession()
        {
            if (!File.Exists(db))
            {
                SQLiteConnection.CreateFile(db);
                using(SQLiteConnection connection = new SQLiteConnection(connectionString))
                {
                    SQLiteCommand cmd = new SQLiteCommand(connection);
                    cmd.CommandText = Properties.Resources.DBSchema;
                    connection.Open();
                    cmd.ExecuteNonQuery();
                    connection.Close();
                }
            }
            var cfg = new Configuration()
            .DataBaseIntegration(db => {
                db.ConnectionString = connectionString;
                db.Dialect<SQLiteDialect>();
            });
            var mapper = new ModelMapper();
            mapper.AddMappings(Assembly.GetExecutingAssembly().GetExportedTypes());
            HbmMapping mapping = mapper.CompileMappingForAllExplicitlyAddedEntities();
            cfg.AddMapping(mapping);
            new SchemaUpdate(cfg).Execute(true, true);
            ISessionFactory sessionFactory = cfg.BuildSessionFactory();
            return sessionFactory.OpenSession();
        }
    }
}
