using System;
using System.Collections.Generic;
using System.Text;

namespace Calrom.Training.SocialMedia.Database.ORMRepositories
{
    public interface IRepository<T>
    {
        IEnumerable<T> List();

        void AddOrUpdate(T entity);

        void Delete(T entity);

        T FindById(int Id);
    }
}
