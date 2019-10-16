using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Supermarket
{
    public class FruitFactory : Factory
    {
        private static FruitFactory Instance = null;
        private static readonly object padlock = new object();
        public static FruitFactory getInstance
        {
            get
            {
                if (Instance == null)
                {
                    lock (padlock)
                    {
                        if (Instance == null)
                        {
                            Instance = new FruitFactory();
                        }
                    }
                }
                return Instance;
            }
        }
        //public static Director directorInstance { get { return Director.getInstance; } }

        public override Fruit getFruit(FruitTypes type)
        {
            IFruitBuilder builder = new ConcreteBuilder();
            Fruit fruit = null;
            switch(type)
            {
                case FruitTypes.Apple:
                    builder.BuildApple();
                    fruit = builder.GetFruit();
                    break;
                case FruitTypes.Banana:
                    builder.BuildBanana();
                    fruit = builder.GetFruit();
                    break;
                case FruitTypes.Orange:
                    builder.BuildOrange();
                    fruit = builder.GetFruit();
                    break;
                default:
                    fruit = null;
                    break;
            }
            Console.WriteLine("You have added an " + fruit.getFruitName() + " to your basket.");
            return fruit;
        }
    }
}
