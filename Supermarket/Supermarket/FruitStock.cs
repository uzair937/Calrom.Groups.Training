using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Supermarket
{
    public abstract class FruitStock : GroceryStore
    {
        protected static int appleStock = 7;
        protected static int bananaStock = 10;
        protected static int orangeStock = 4;
        protected const double appleCost = 0.50;
        protected const double bananaCost = 0.30;
        protected const double orangeCost = 0.45;

        public static void displayStock()
        {
            Console.WriteLine("Apples: " + appleStock + " || " + "Price: " + appleCost);
            Console.WriteLine("Bananas: " + bananaStock + " || " + "Price: " + bananaCost);
            Console.WriteLine("Oranges: " + orangeStock + " || " + "Price: " + orangeCost);
        }
    }
}
