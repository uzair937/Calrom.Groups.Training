using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobLibrary
{
    public static class DatabaseFactory
    {
        public static GenericDatabase GetDatabase(DatabaseSelector dbs)
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
