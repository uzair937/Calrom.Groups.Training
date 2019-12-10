using System.Collections.Generic;

namespace CustomRegionEditor.Database.Interfaces
{
    public interface IRepository<T>
    {
        void AddOrUpdate(T entity);

        T FindById(string id);

        void Delete(T entity);

        void DeleteById(string id);

        List<T> List();
    }
}
