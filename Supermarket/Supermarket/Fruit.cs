using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Supermarket
{
    public class Fruit : FruitStock
    {
        public string fruitName { get; set; }
        public double cost { get; set; }
        public Fruit(string input) //fruit object ready to store in a list
        {
            fruitName = returnFruit(input);
            addFruit(input);
        }

        public void addFruit(string input)
        {
            if (input.Equals("A") && appleStock > 0)
            {
                appleStock--; //stock decreases as more are bought
                cost = appleCost;
                fruitList.Add(this); //adds the object to a list
            }
            else if (input.Equals("B") && bananaStock > 0)
            {
                bananaStock--;
                cost = bananaCost;
                fruitList.Add(this);
            }
            else if (input.Equals("O") && orangeStock > 0)
            {
                orangeStock--;
                cost = orangeCost;
                fruitList.Add(this);
            }
            else
            {
                Console.WriteLine(outOfStock());
            }
            Console.WriteLine("You have added a " + fruitName + " to your basket.");
        }

            public string returnFruit(string input) //return method to save on manually hard coding
            {
                if (input.Equals("A"))
                {
                    return "Apple";
                }
                else if (input.Equals("B"))
                {
                    return "Banana";
                }
                else if (input.Equals("O"))
                {
                    return "Orange";
                }
                else
                {
                    return "Unavailable";
                }
            }

            public string outOfStock()
            {
                return "This item is out of stock.";
            }
        }
}
