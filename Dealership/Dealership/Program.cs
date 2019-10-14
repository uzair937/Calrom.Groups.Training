using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dealership
{
    public class Program
    {
        static void Main(string[] args)
        {

            List<Car> listofcars = new List<Car>();
            RetrieveCar retrieveCar = new RetrieveCar();
            var retcar1 = retrieveCar.GetCar1();
            var retcar2 = retrieveCar.GetCar2();
            var retcar3 = retrieveCar.GetCar3();
            var retcar4 = retrieveCar.GetCar4();
            var retcar5 = retrieveCar.GetCar5();
            var retcar6 = retrieveCar.GetCar6(); 

            listofcars.Add(retcar1);
            listofcars.Add(retcar2);
            listofcars.Add(retcar3);
            listofcars.Add(retcar4);
            listofcars.Add(retcar5);
            listofcars.Add(retcar6);

            VehicleQuery vehicleQuery = new VehicleQuery();

            char getletter = 'B';
            var get1stchar = vehicleQuery.gettingfirstchar(listofcars, getletter);

            foreach (var item in get1stchar)
            {
                Console.WriteLine(item.ToString());
            }

            var maxmileage = vehicleQuery.getting(listofcars);
            Console.WriteLine(maxmileage.ToString());
        

            Console.ReadKey();
         
          
        }
    }
}
