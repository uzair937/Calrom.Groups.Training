using System;
using System.Collections.Generic;

namespace Supermarket
{
    /*
     * Uzair Foolat
     * Supermarket Application
     * 08/10/2019
     * Calrom Manchester
     */
    class Supermarket
    {
        static string[] split; //stores the split characters from the console
        static void getAllStock() //displays the initial stock
        {
            Console.WriteLine("Apples: " + Stock.appleStock + " || " + "Price: " + Stock.appleCost);
            Console.WriteLine("Bananas: " + Stock.bananaStock + " || " + "Price: " + Stock.bananaCost);
            Console.WriteLine("Oranges: " + Stock.orangeStock + " || " + "Price: " + Stock.orangeCost);
        }

        static void calculateTotal() //works out the total cost
        {
            int aCount = 0;
            int bCount = 0;
            int oCount = 0;
            for (int i = 0; i < split.Length; i++)
            {
                if (split[i] == "A")
                {
                    aCount++;
                }
                if (split[i] == "B")
                {
                    bCount++;
                }
                if (split[i] == "O")
                {
                    oCount++;
                }
            }

            if (aCount > 0 && 3 % aCount == 0) //using modulo correctly was the only thing I Googled as I was doing 'aCount % 3' instead of '3 % aCount'
            {
                Stock.totalCost -= 0.3;
                Console.WriteLine("Multi buy discount of - £0.30");
            }
            if (bCount > 0 && 5 % bCount == 0)
            {
                Stock.totalCost -= 0.3;
                Console.WriteLine("Multi buy discount of - £0.30");
            }
            Console.WriteLine("Total cost: " + "£" + Stock.totalCost);
        }

        static void Main(string[] args)
        {
            var basket = new List<Stock>(); //list of items that get selected
            Console.WriteLine("Welcome to Uzair's supermarket!");
            Console.WriteLine("Current Stock: ");
            getAllStock();
            Console.WriteLine("Please enter \"A\" for Apples, \"B\" for Bananas and \"O\" for Oranges.");
            Console.WriteLine("Please enter \"C\" for Carrots, \"P\" for Potatoes and \"S\" for Spinach.");
            Console.WriteLine("Each item must be separated with a space.");
            Console.WriteLine("Press enter after you have chosen your items to see your basket and total cost."); //explains how to use application
            string input = Console.ReadLine().ToUpper(); //needs to be uppercase
            split = input.Split(' '); //splits based on spaces
            foreach (string str in split)
            {
                if (str.Length > 1)
                {
                    Console.WriteLine("Please use the space key between each item.");
                }
                else if (str == "A" || str == "B" || str == "O")
                {
                    basket.Add(new Fruit(str));
                }
                else if (str == "C" || str == "P" || str == "S")
                {
                    basket.Add(new Vegetable(str));
                }
                else if (string.IsNullOrEmpty(str))
                {
                    Console.WriteLine("Please enter \"A\" for Apples, \"B\" for Bananas and \"O\" for Oranges.");
                }
                else
                {
                    Console.WriteLine("Please enter \"O\" for Oranges, \"A\" for Apples and \"B\" for Bananas.");
                }
            }

            if (string.IsNullOrEmpty(Console.ReadLine()))
            {
                Console.WriteLine("The items in your basket are: ");
                foreach (Stock stock in basket)
                {
                    foreach (Fruit fruit in stock.fruitList) //goes through each list to return all the items which have been added
                    {
                        Console.WriteLine(fruit.fruitName + " " + "£" + fruit.cost);
                        Stock.totalCost += fruit.cost; //increments the total cost with the cost of each fruit
                    }
                    foreach (Vegetable veg in stock.vegList)
                    {
                        Console.WriteLine(veg.vegName + " " + "£" + veg.cost);
                        Stock.totalCost += veg.cost;
                    }
                }
            }
            calculateTotal();
            Console.ReadKey();
        }
    }

    abstract class Stock //abstract base class with the stock and costd
    {
        public List<Fruit> fruitList = new List<Fruit>(); //contains all the fruit and vegetables that are bought
        public List<Vegetable> vegList = new List<Vegetable>();
        public static double totalCost { get; set; }

        public static int appleStock = 7;
        public static int bananaStock = 10;
        public static int orangeStock = 4;
        public static double appleCost = 0.50;
        public static double bananaCost = 0.30;
        public static double orangeCost = 0.45;

        public static int carrotStock = 15;
        public static int potatoStock = 20;
        public static int spinachStock = 100;
        public static double carrotCost = 0.10;
        public static double potatoCost = 0.15;
        public static double spinachCost = 0.05;
    }

    class Fruit : Stock //extends stock as it needs the values
    {
        public string fruitName { get; set; }
        public double cost { get; set; }
        public Fruit(string input) //fruit object ready to store in a list
        {
            fruitName = returnFruit(input);
            Console.WriteLine("You have added a " + fruitName + " to your basket.");
            if (input == "A" && appleStock > 0)
            {
                appleStock--; //stock decreases as more are bought
                cost = appleCost;
                fruitList.Add(this); //adds the object to a list
            }
            else if (input == "B" && bananaStock > 0)
            {
                bananaStock--;
                cost = bananaCost;
                fruitList.Add(this);
            }
            else if (input == "O" && orangeStock > 0)
            {
                orangeStock--;
                cost = orangeCost;
                fruitList.Add(this);
            }
            else
            {
                Console.WriteLine(outOfStock());
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

        public string outOfStock()
        {
            return "This item is out of stock.";
        }
    }

    class Vegetable : Stock
    {
        public string vegName { get; set; }
        public double cost { get; set; }
        public Vegetable(string input)
        {
            vegName = returnVeg(input);
            Console.WriteLine("You have added a " + vegName + " to your basket.");
            if (input == "C" && carrotStock > 0)
            {
                carrotStock--;
                cost = carrotCost;
                vegList.Add(this);
            }
            else if (input == "P" && carrotStock > 0)
            {
                potatoStock--;
                cost = potatoCost;
                vegList.Add(this);
            }
            else if (input == "S" && carrotStock > 0)
            {
                spinachStock--;
                cost = spinachCost;
                vegList.Add(this);
            }
            else
            {
                Console.WriteLine(outOfStock());
            }
        }

        public string returnVeg(string input)
        {
            if (input == "C")
            {
                return "Carrot";
            }
            else if (input == "P")
            {
                return "Potato";
            }
            else if (input == "S")
            {
                return "Spinach";
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
