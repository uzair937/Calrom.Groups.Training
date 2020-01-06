using System.Collections.Generic;

namespace CustomRegionEditor.Database.Interfaces
{
    public interface IRepository<T>
    {
        T AddOrUpdate(T entity);

        T FindById(string id);

        void Delete(T entity);

        List<T> List();
    }
}
