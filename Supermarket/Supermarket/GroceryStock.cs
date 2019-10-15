using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Supermarket
{
    public abstract class GroceryStock //put stock in here and rename
    {
        public static List<GroceryStock> fruitList = new List<GroceryStock>(); //contains all the fruit and vegetables that are bought
        public static List<GroceryStock> vegList = new List<GroceryStock>();
        public static int appleStock = 7;
        public static int bananaStock = 10;
        public static int orangeStock = 4;
        public const double appleCost = 0.50;
        public const double bananaCost = 0.30;
        public const double orangeCost = 0.45;

        public abstract bool outOfStock();
        public abstract void addItem(GroceryStock groceryStock);
        public abstract void displayStock();
    }
}
