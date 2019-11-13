using System;
using System.Collections.Generic;
using System.Text;

namespace Calrom.Training.SocialMedia.Database.Repositories
{
    public interface IRepository<T>
    {
        IEnumerable<T> List();

        IEnumerable<T> FollowedUserBorks(int userId);

        void Add(T entity);

        void Delete(T entity);

        T FindById(int Id);
    }
}
