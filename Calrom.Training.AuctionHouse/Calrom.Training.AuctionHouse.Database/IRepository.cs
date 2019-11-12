using System.Collections.Generic;

namespace Calrom.Training.AuctionHouse.Database
{
    interface IRepository<T>
    {
        void Add(T entity);
        List<T> List();
    }
}
