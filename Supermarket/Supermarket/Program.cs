using System;
using System.Collections.Generic;

namespace Supermarket
{
    /*
     * Uzair Foolat
     * Supermarket Application
     * 11/10/2019
     * Calrom Manchester
     */
    public class Program
    {
        private static char[] split; //stores the split characters from the console

        private static void initialiseStore()
        {
            Console.WriteLine("Welcome to Uzair's supermarket!");
            Console.WriteLine("Current Stock: ");

            VegStock.displayStock();
            Console.WriteLine("Please enter \"A\" for Apples, \"B\" for Bananas and \"O\" for Oranges.");
            Console.WriteLine("Please enter \"C\" for Carrots, \"P\" for Potatoes and \"S\" for Spinach.");
            Console.WriteLine("Press enter after you have chosen your items to see your basket and total cost."); //explains how to use application
        }

        private static void Main(string[] args)
        {
            initialiseStore();
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
                    new Fruit(str);
                }
                else if (str.Equals("C") || str.Equals("P") || str.Equals("S"))
                {
                    new Veg(str);
                }
                else
                {
                    Console.WriteLine("Please enter \"O\" for Oranges, \"A\" for Apples and \"B\" for Bananas.");
                }
            }
            Checkout checkout = new Checkout();
            if (string.IsNullOrEmpty(Console.ReadLine()))
            {
                checkout.getBasket();
            }
            checkout.calculateTotal();
            Console.ReadKey();
        }
    }
}
