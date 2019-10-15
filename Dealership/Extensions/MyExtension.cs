using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Extensions
{
    public static class MyExtension
    {
       public static void GetMaxMileage<T>(this T maxmile)
       {
          Console.WriteLine(maxmile.ToString());
       }

       public static void FirstCharofBuyer(this string listofcars)
       {
            Console.WriteLine(listofcars.ToString());
       }
        
    }
}
