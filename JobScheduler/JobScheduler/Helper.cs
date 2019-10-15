using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobScheduler
{
    public static class Helper
    {
        public static string FirstandLast<T>(IEnumerable<T> inspect)
        {
            var _result = new string[2];
            _result[0] = inspect.ElementAt(0).ToString();
            _result[1] = inspect.ElementAt(inspect.Count() - 1).ToString();
            return _result[0] + "\n" + _result[1] ?? "";
        }
    }
}
