using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dealership
{
    public class VehicleQuery
    {
        public List<Car> getMercedesOnly(List<Car> listofcars)
        {
            var gettingmerc = listofcars.Where(m => m.CarName == "Mercedes").ToList();
            return gettingmerc;
        }

        public List<Car> getAudiorBMW(List<Car> listofcars)
        {
            var gettingaudisorbmw = listofcars.Where(m => m.CarName == "Mercedes" || m.CarName == "Audi").ToList();
            return gettingaudisorbmw;
        }

        public List<Car> sortMileage(List<Car> listofcars)
        {
            var sortingMileage = listofcars.OrderBy(m => m.Mileage).ToList();
            return sortingMileage;
        }
        public List<Car> sortcarNames(List<Car> listofcars)
        {
            List<Car> sortingNames = listofcars.OrderBy(m => m.CarName).ToList();
            return sortingNames;
        }

        public List<Car> mileageDescending(List<Car> listofcars)
        {
            var mileageDesc = listofcars.OrderByDescending(m => m.Mileage).ToList();
            return mileageDesc;
        }

        public List<Car> gettingfirstchar(List<Car> listofcars, char getletter)
        {
            var cars = listofcars.Where(m => m.Buyer[0] == getletter).ToList();
            return cars; 
        }

        public Car getting(List<Car> listofcars)
        {
            var orderedList = listofcars.OrderBy(m => m.Mileage).ToList();
            var car = orderedList.Last();
            return car;
        }


    }
}
