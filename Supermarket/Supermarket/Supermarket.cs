using System;
using System.Collections.Generic;

namespace Supermarket
{
    /*
     * Uzair Foolat
     * Supermarket Application
     * 10/10/2019
     * Calrom Manchester
     */
    public class Supermarket
    {
        private static char[] split; //stores the split characters from the console
        private static double totalCost;

        private void calculateTotal(GroceryStore groceryStore) //works out the total cost
        {
            if (groceryStore.checkDiscount()) //using modulo correctly was the only thing I Googled as I was doing 'aCount % 3' instead of '3 % aCount'
            {
                totalCost -= 0.3;
                Console.WriteLine("Multi buy discount of - £0.30");
            }
            Console.WriteLine("Total cost: " + "£" + totalCost);
        }

        private static void initApp()
        {
            Console.WriteLine("Welcome to Uzair's supermarket!");
            Console.WriteLine("Current Stock: ");
            FruitStock.displayStock();
            VegStock.displayStock();
            Console.WriteLine("Please enter \"A\" for Apples, \"B\" for Bananas and \"O\" for Oranges.");
            Console.WriteLine("Please enter \"C\" for Carrots, \"P\" for Potatoes and \"S\" for Spinach.");
            Console.WriteLine("Press enter after you have chosen your items to see your basket and total cost."); //explains how to use application
        }

        private static void Main(string[] args)
        {
            GroceryStore groceryStore = new GroceryStore();
            initApp();
            string input = Console.ReadLine().ToUpper(); //needs to be uppercase
            split = input.ToCharArray(); //splits
            foreach (char chars in split)
            {
                string str = chars.ToString();
                if (str.Equals(" ") || string.IsNullOrEmpty(str))
                {
                    Console.WriteLine("Please type in all the items you wish to purchase without any spaces.");
                }
                else if (str.Equals("A") || str.Equals("B") || str.Equals("O"))
                {
                    groceryStore.fruitList.Add(new Fruit(str));
                }
                else if (str.Equals("C") || str.Equals("P") || str.Equals("S"))
                {
                    groceryStore.vegList.Add(new Veg(str));
                }
                else
                {
                    Console.WriteLine("Please enter \"O\" for Oranges, \"A\" for Apples and \"B\" for Bananas.");
                }
            }

            if (string.IsNullOrEmpty(Console.ReadLine()))
            {
                Console.WriteLine("The items in your basket are: ");

                    foreach (Fruit fruit in groceryStore.fruitList) //goes through each list to return all the items which have been added
                    {
                        Console.WriteLine(fruit.fruitName + " " + "£" + fruit.cost);
                        totalCost += fruit.cost; //increments the total cost with the cost of each fruit
                    }
                    foreach (Veg veg in groceryStore.vegList)
                    {
                        Console.WriteLine(veg.vegName + " " + "£" + veg.cost);
                        totalCost += veg.cost;
                    }

            }
            var supermarket = new Supermarket();
            supermarket.calculateTotal(groceryStore);
            Console.ReadKey();
        }
    }
}
