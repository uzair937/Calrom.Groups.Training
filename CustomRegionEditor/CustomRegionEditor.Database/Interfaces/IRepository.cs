using System.Collections.Generic;

namespace CustomRegionEditor.Database.Interfaces
{
    public interface IRepository<T>
    {
        void AddOrUpdate(T entity);
        void Delete(T entity);
        List<T> List();
    }
}
