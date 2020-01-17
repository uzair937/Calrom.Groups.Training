using System;
using System.Collections.Generic;

namespace CustomRegionEditor.Database.Interfaces
{
    public interface IRepository<T>
    {
        T AddOrUpdate(T entity);

        T FindById(Guid id);

        void Delete(T entity);

        List<T> List();
    }
}
