using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dealership
{
    public class AllVehicles
    {
        public List<Cars> getMercedesOnly(List<Cars> listofcars)
        {
            var gettingmerc = listofcars.Where(m => m.CarName == "Mercedes").ToList();
            return gettingmerc;
        }

        public List<Cars> getRangeRover(List<Cars> listofcars)
        {
            var gettingaudisorbmw = listofcars.Where(m => m.CarName == "Mercedes" || m.CarName == "Audi").ToList();
            return gettingaudisorbmw;
        }

        public List<Cars> sortMileage(List<Cars> listofcars)
        {
            List<Cars> sortingMileage = listofcars.OrderBy(m => m.Mileage).ToList();
            return sortingMileage;
        }

        public List<Cars> MaxMileage(List<Cars> listofcars)
        {
            List<Cars> maxMileage = listofcars.OrderBy(m => m.Mileage).ToList();
            return maxMileage;
        }
        public List<Cars> sortcarNames(List<Cars> listofcars)
        {
            List<Cars> sortingNames = listofcars.OrderBy(m => m.CarName).ToList();
            return sortingNames;
        }


    }
}
