using System;
using System.Collections.Generic;
using System.Text;

namespace Calrom.Training.AuctionHouse.Database
{
    interface IRepository<T>
    {
        void Add(T entity);
        List<T> List();
    }
}
