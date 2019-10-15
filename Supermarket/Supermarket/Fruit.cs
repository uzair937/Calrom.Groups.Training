using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Supermarket
{
    public class Fruit : GroceryStock //dont need to derrive from grocerystock
    {
        public string fruitName;
        public double cost;

        internal static readonly Fruit Instance = new Fruit();

        public override void addItem(GroceryStock fruit)
        {
            fruitList.Add(fruit);
            Console.WriteLine("You have added an " + fruitName + " " + cost);
        }

        public override bool outOfStock()
        {
            Console.WriteLine("This item is out of stock.");
            return true;
        }

        public override void displayStock()
        {
            Console.WriteLine("Apples " + appleStock + " Cost: " + appleCost);
            Console.WriteLine("Bananas " + bananaStock + " Cost: " + bananaCost);
            Console.WriteLine("Oranges " + orangeStock + " Cost: " + orangeCost);
        }
    }
}
