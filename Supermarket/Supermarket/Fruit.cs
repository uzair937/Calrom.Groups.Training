using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Supermarket
{
    public class Fruit //dont need to derrive from grocerystock
    {
        public string fruitName;
        public double cost;

        internal static readonly Fruit Instance = new Fruit();

        public void addItem(GroceryStock fruit)
        {
            GroceryStock.fruitList.Add(fruit);
            Console.WriteLine("You have added an " + fruitName + " " + cost);
        }

        public bool outOfStock()
        {
            Console.WriteLine("This item is out of stock.");
            return true;
        }

        public void displayStock()
        {
            Console.WriteLine("Apples " + GroceryStock.appleStock + " Cost: " + GroceryStock.appleCost);
            Console.WriteLine("Bananas " + GroceryStock.bananaStock + " Cost: " + GroceryStock.bananaCost);
            Console.WriteLine("Oranges " + GroceryStock.orangeStock + " Cost: " + GroceryStock.orangeCost);
        }
    }
}
