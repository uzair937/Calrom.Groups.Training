using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Supermarket
{
    public class ConcreteBuilder : IFruitBuilder
    {
        private Fruit fruit = null;
        public void BuildApple()
        {
            fruit = new Apple();
        }

        public void BuildBanana()
        {
            fruit = new Banana();
        }

        public void BuildOrange()
        {
            fruit = new Orange();
        }

        public Fruit GetFruit()
        {
            return fruit;
        }
    }
}
