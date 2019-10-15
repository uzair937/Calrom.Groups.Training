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
        protected static int appleStock = 7;
        protected static int bananaStock = 10;
        protected static int orangeStock = 4;
        protected const double appleCost = 0.50;
        protected const double bananaCost = 0.30;
        protected const double orangeCost = 0.45;

        public abstract bool outOfStock();
        public abstract void addItem(GroceryStock groceryStock);
        public abstract void displayStock();
    }
}
