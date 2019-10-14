using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Supermarket
{
    public abstract class GroceryStock //put stock in here and rename
    {
        public static List<Fruit> fruitList = new List<Fruit>(); //contains all the fruit and vegetables that are bought
        public static List<Veg> vegList = new List<Veg>();
        protected static int appleStock = 7;
        protected static int bananaStock = 10;
        protected static int orangeStock = 4;
        protected const double appleCost = 0.50;
        protected const double bananaCost = 0.30;
        protected const double orangeCost = 0.45;

        public bool outOfStock()
        {
            Console.WriteLine("This item is out of stock.");
            return true;
        }
    }
}
