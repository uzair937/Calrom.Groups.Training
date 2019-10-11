using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Supermarket
{
    public class Fruit : GroceryStore
    {
        protected static int appleStock = 7;
        protected static int bananaStock = 10;
        protected static int orangeStock = 4;
        public string fruitName { get; set; }
        public double cost { get; set; }
        public string input { get; set; }

        public Fruit()
        {
            fruitName = returnFruit();
            addFruit();
        }

        public void addFruit()
        {
            bool empty = false;
            if (input == "A")
            {
                new Apple();
            }
            else if (input == "B")
            {
                new Banana();
            }
            else if (input == "O")
            {
                new Orange();
            }
            else
            {
                empty = outOfStock();
            }
            if (!empty)
            {
                Console.WriteLine("You have added a " + fruitName + " to your basket.");
            } else
            {
                fruitList.Add(this);
            }
        }

            public string returnFruit() //return method to save on manually hard coding
            {
                if (input == "A")
                {
                    return "Apple";
                }
                else if (input == "B")
                {
                    return "Banana";
                }
                else if (input == "O")
                {
                    return "Orange";
                }
                else
                {
                    return "Unavailable";
                }
            }
        }
}
