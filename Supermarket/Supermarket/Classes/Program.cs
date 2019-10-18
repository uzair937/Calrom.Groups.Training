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
        public static List<Fruit> fruitList = new List<Fruit>();
        public static FruitFactory factoryInstance { get { return FruitFactory.getInstance; } }
        public static Checkout checkoutInstance { get { return Checkout.getInstance; } }
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
                    switch (str)
                    {
                        case "A":
                            fruitList.Add(factoryInstance.getFruit(FruitTypes.Apple));
                            break;
                        case "B":
                            fruitList.Add(factoryInstance.getFruit(FruitTypes.Banana));
                            break;
                        case "O":
                            fruitList.Add(factoryInstance.getFruit(FruitTypes.Orange));
                            break;
                        default:
                            break;
                    }
                }
                else if (str.Equals("C") || str.Equals("P") || str.Equals("S"))
                {
                    //
                }
                else
                {
                    Console.WriteLine("Please enter \"O\" for Oranges, \"A\" for Apples and \"B\" for Bananas.");
                }
            }
            if (string.IsNullOrEmpty(Console.ReadLine()))
            {
                //storeDB();
                checkoutInstance.getBasket(fruitList);
            }
            checkoutInstance.calculateTotal(fruitList);
            Console.ReadKey();
        }

        private static void storeDB()
        {
            if (fruitList.Count > 0)
            {
                FruitRepo fruitRepo = new FruitRepo();

                foreach (Fruit fruit in fruitList)
                {
                    Random random = new Random();
                    FruitDB fruitDB = new FruitDB();
                    fruitDB.FruitID = random.Next();
                    fruitDB.FruitName = fruit.fruitName;
                    fruitDB.FruitCost = (float)fruit.cost;
                    fruitRepo.Insert(fruitDB);
                }
            }
            else
            {
                Console.WriteLine("Basket is empty.");
            }
            
        }

        private static char[] split; //stores the split characters from the console

        private static void initialiseStore()
        {
            Console.WriteLine("Welcome to Uzair's supermarket!");
            Console.WriteLine("Current Stock: ");
            Console.WriteLine("Please enter \"A\" for Apples, \"B\" for Bananas and \"O\" for Oranges.");
            Console.WriteLine("Please enter \"C\" for Carrots, \"P\" for Potatoes and \"S\" for Spinach.");
            Console.WriteLine("Press enter after you have chosen your items to see your basket and total cost."); //explains how to use application
        }
    }
}