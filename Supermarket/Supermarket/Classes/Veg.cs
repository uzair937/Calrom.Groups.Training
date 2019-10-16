using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Supermarket
{
    public class Veg : GroceryStock
    {
        public string vegName { get; set; }
        public double cost { get; set; }
        public override void addItem(GroceryStock groceryStock)
        {
            vegList.Add(groceryStock);
        }

        public string returnVeg(string input)
        {
            if (input.Equals("C"))
            {
                return "Carrot";
            }
            else if (input.Equals("P"))
            {
                return "Potato";
            }
            else if (input.Equals("S"))
            {
                return "Spinach";
            }
            else
            {
                return "Unavailable";
            }
        }

        public override bool outOfStock()
        {
            Console.WriteLine("This item is out of stock.");
            return true;
        }

        public override void displayStock()
        {
            
        }
    }
}
