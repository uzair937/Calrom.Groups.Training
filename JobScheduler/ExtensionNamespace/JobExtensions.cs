using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobLibrary
{
    public static class JobExtensions
    {
        public static string FirstandLast<T>(this IEnumerable<T> inspect)
        {
            var _result = new string[2];
            _result[0] = inspect.ElementAt(0).ToString();
            _result[1] = inspect.ElementAt(inspect.Count() - 1).ToString();
            return _result[0].AddToSelf("\n" + _result[1] ?? "") ?? "";
        }

        public static string AddToSelf<T>(this T inspect, string param)
        {
            return (inspect + param) ?? param;
        }

        public static bool Print<T>(this T param)
        {
            try
            {
                Console.WriteLine(param);
                return true;
            }
            catch
            {
                Console.WriteLine("Failed To Print");
                return false;
            }
        }
    }
}
