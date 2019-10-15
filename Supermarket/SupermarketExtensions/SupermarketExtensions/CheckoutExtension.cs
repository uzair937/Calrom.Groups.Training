using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Supermarket
{
    public static class CheckoutExtension
    {
        private static double totalCost;
        public static string printBasket(this string str, string str2) //parameterised
        {
            return str + " " + "£" + str2;
        }

        public static void printTotal(this string str) //no parameters
        {
            Console.WriteLine("Total cost: " + "£" + str);
        }

        public static bool isDiscounted<T>(this IList<T> costList) //generics
        {
            if (costList.Count == 3 || costList.Count == 5)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static void setCost(this double cost)
        {
            totalCost = cost;
        }
    }
} //want to access properties using extension methods?
