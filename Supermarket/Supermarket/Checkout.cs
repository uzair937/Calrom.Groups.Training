using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Supermarket
{
    class Checkout : GroceryStore
    {
        private double totalCost;
        public bool checkDiscount()
        {
            int aCount = 0, bCount = 0;
            foreach (Fruit fruit in fruitList)
            {
                if (fruit.fruitName.Equals("Apple"))
                {
                    aCount++;
                } else if (fruit.fruitName.Equals("Banana"))
                {
                    bCount++;
                }
            }
            if (aCount > 0 && (aCount % 3) == 0)
            {
                return true;
            }
            else if (bCount > 0 && (bCount % 5) == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public void printBasket()
        {
            Console.WriteLine("The items in your basket are: ");

            foreach (Fruit fruit in fruitList) //goes through each list to return all the items which have been added
            {
                Console.WriteLine(fruit.fruitName + " " + "£" + fruit.cost);
                totalCost += fruit.cost; //increments the total cost with the cost of each fruit
            }
            foreach (Veg veg in vegList)
            {
                Console.WriteLine(veg.vegName + " " + "£" + veg.cost);
                totalCost += veg.cost;
            }
        }

        public void calculateTotal() //works out the total cost
        {
            if (checkDiscount()) //using modulo correctly was the only thing I Googled as I was doing 'aCount % 3' instead of '3 % aCount'
            {
                totalCost -= 0.3;
                Console.WriteLine("Multi buy discount of - £0.30");
            }
            Console.WriteLine("Total cost: " + "£" + totalCost);
        }
    }
}
