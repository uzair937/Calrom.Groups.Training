using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Supermarket
{
    public static class CheckoutExtension
    {
        public static string printBasket(this string str, string str2) //parameterised
        {
            return str + " " + "£" + str2;
        }

        public static void printTotal(this string str) //no parameters
        {
            Console.WriteLine("Total cost: " + "£" + str);
        }

        public static bool isDiscounted(this List<double> costList)
        {
            if (costList.Count == 3 || costList.Count == 5)
            {
                return true;
            } else
            {
                return false;
            }
        }
    }
}
