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

        public abstract bool outOfStock();
        public abstract void addItem(GroceryStock groceryStock);
        public abstract void displayStock();
    }
}
