
namespace FactoryLibrary
{
    public class DatabaseFactory : AbstractFactory
    {
        private static DatabaseFactory obj;

        private DatabaseFactory() { }

        public static AbstractFactory GetFac()
        {
            if (obj == null) obj = new DatabaseFactory();
            return obj;
        }

        public override GenericDatabaseTools GetDatabase(DatabaseSelector dbs)
        {
            if (dbs == DatabaseSelector.JSON)
            {
                return new JsonDatabase();
            }
            else if (dbs == DatabaseSelector.XML)
            {
                return new XmlDatabase();
            }
            else  if (dbs == DatabaseSelector.SQL)
            {
                return new SqlDatabase();
            }
            return null;
        }
    }
}
