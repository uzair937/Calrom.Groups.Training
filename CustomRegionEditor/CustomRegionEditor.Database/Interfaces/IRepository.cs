using System.Collections.Generic;

namespace CustomRegionEditor.Database
{
    interface IRepository<T>
    {
        void AddOrUpdate(T entity);
        void Delete(T entity);
        List<T> List();
        List<T> GetSearchResults(string searchTerm);
    }
}
