using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Supermarket
{
    public class Fruit : GroceryStock //dont need to derrive from grocerystock
    {

        public string fruitName { get; set; }
        public double cost { get; set; }

        public Fruit(string input)
        {
            fruitName = returnFruit(input);
            addFruit(input);
        }

        public void addFruit(string input)
        {
            bool empty = false;
            if (input == "A")
            {
                cost = appleCost;
                appleStock--;
                fruitList.Add(this);
            }
            else if (input == "B")
            {
                cost = bananaCost;
                bananaStock--;
                fruitList.Add(this);
            }
            else if (input == "O")
            {
                cost = orangeCost;
                orangeStock--;
                fruitList.Add(this);
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

            public string returnFruit(string input) //return method to save on manually hard coding
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
