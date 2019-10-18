using JobLibrary;
using System.Collections.Generic;

namespace FactoryLibrary
{
    public abstract class GenericDatabaseTools
    {
        public abstract bool AddData(List<IEntity>[] db);
        public abstract List<IEntity>[] GetData();
    }

    public enum DatabaseSelector
    {
        JSON,
        XML,
        SQL
    }
}
