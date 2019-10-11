using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Supermarket
{
    public abstract class GroceryStore
    {
        public static List<Fruit> fruitList = new List<Fruit>(); //contains all the fruit and vegetables that are bought
        public static List<Veg> vegList = new List<Veg>();
        //protected static List<Apple> appleList = new List<Apple>();
        //protected static List<Banana> bananaList = new List<Banana>();
        //protected static List<Orange> orangeList = new List<Orange>();

        public bool outOfStock()
        {
            Console.WriteLine("This item is out of stock.");
            return true;
        }
    }
}
