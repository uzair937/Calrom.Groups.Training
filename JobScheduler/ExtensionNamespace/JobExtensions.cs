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
            return _result[0].AddToSelf("\n" + _result[1]);
        }

        public static string AddToSelf<T>(this T inspect, string param)
        {
            if (inspect == null) return param;
            return inspect + param;
        }
    }
}
