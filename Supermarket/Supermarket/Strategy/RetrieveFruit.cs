using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Supermarket
{
    public class RetrieveFruit : IRetrieveStrategy
    {
        public void GetItem(List<Fruit> fruitList)
        {
            if (fruitList.Count > 0)
            {
                foreach (Fruit fruit in fruitList)
                {
                    Console.WriteLine((fruit.getFruitName().printBasket(fruit.getCost().ToString())));
                    Checkout.totalCost += fruit.getCost();
                }
            } else
            {
                NoFruit noFruit = new NoFruit();
                noFruit.GetItem(fruitList);
            }

        }
    }
}
