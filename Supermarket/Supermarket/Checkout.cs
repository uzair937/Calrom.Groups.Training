using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Supermarket
{
    class Checkout
    {

        public double totalCost;
        public static bool checkDiscount(List<Fruit> fruitList) //discounts by 0.30 if 3 apples or 5 bananas
        {
            List<double> appleList = new List<double>();
            List<double> bananaList = new List<double>();

            foreach (Fruit fruit in fruitList)
            {
                if (fruit.getFruitName().Equals("Apple"))
                {
                    appleList.Add(fruit.getCost());
                }
                else if (fruit.getFruitName().Equals("Banana"))
                {
                    bananaList.Add(fruit.getCost());
                }
            }
            if (bananaList.isDiscounted())
            {
                return true;
            } else if (appleList.isDiscounted())
            {
                return true;
            } else
            {
                return false;
            }
            
        }

        public void getBasket(List<Fruit> fruitList)
        {
            Console.WriteLine("The items in your basket are: ");

            foreach (Fruit fruit in fruitList) //goes through each list to return all the items which have been added
            {
                Console.WriteLine((fruit.getFruitName().printBasket(fruit.getCost().ToString())));
                totalCost += fruit.getCost(); //increments the total cost with the cost of each fruit
                
            }
            foreach (Veg veg in GroceryStock.vegList)
            {
                Console.WriteLine(veg.vegName + " " + "£" + veg.cost);
                totalCost += veg.cost;
            }
        }

        public void calculateTotal(List<Fruit> fruitList) //works out the total cost
        {
            if (checkDiscount(fruitList)) //using modulo correctly was the only thing I Googled as I was doing 'aCount % 3' instead of '3 % aCount'
            {
                totalCost -= 0.3;
                Console.WriteLine("Multi buy discount of - £0.30");
            }
            totalCost.ToString().printTotal();
        }
    }
}
