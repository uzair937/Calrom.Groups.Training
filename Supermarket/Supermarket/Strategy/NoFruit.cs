using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Supermarket
{
    public class NoFruit : IRetrieveStrategy
    {
        public void GetItem(List<Fruit> fruitList)
        {
            Console.WriteLine("There are no fruit in your basket.");
        }
    }
}
