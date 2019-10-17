using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JobLibrary;

namespace JobLibrary
{
    public interface IDatabaseRepos<T> where T : IEntity
    {
        IEnumerable<T> List { get; }
        void Add(T db);
        void Delete(T db);
        T FindById(int id);
    }
}
