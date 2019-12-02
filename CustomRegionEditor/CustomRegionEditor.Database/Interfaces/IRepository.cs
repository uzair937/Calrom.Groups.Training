using System.Collections.Generic;

namespace CustomRegionEditor.Database
{
    interface IRepository<T>
    {
        void Add(T entity);
        void Delete(T entity);
        List<T> List();
        List<T> GetSearchResults(string searchTerm);
    }
}
