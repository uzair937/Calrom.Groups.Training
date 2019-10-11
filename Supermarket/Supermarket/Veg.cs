using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Supermarket
{
    public class Veg : VegStock
    {
        public string vegName { get; set; }
        public double cost { get; set; }
        public Veg(string input)
        {
            vegName = returnVeg(input);
            Console.WriteLine("You have added a " + vegName + " to your basket.");
            if (input.Equals("C") && carrotStock > 0)
            {
                carrotStock--;
                cost = carrotCost;
            }
            else if (input.Equals("P") && carrotStock > 0)
            {
                potatoStock--;
                cost = potatoCost;
            }
            else if (input.Equals("S") && carrotStock > 0)
            {
                spinachStock--;
                cost = spinachCost;
            }
            else
            {
                Console.WriteLine(outOfStock());
            }
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

        public string outOfStock()
        {
            return "This item is out of stock.";
        }
    }
}
