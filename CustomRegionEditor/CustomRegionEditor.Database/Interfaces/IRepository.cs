using System.Collections.Generic;

namespace CustomRegionEditor.Database
{
    interface IRepository<T>
    {
        void Add(T entity);
        List<T> List();
        List<T> GetSearchResults(string searchTerm);
    }
}
