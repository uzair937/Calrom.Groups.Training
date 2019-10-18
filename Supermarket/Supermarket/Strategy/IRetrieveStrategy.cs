using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Supermarket
{
    public interface IRetrieveStrategy
    {
        void GetItem(List<Fruit> fruitList);
    }
}
