using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dealership
{
    public static class CarFactory
    {
        public static BMW CreateBMW()
        {
            BMW bmw = new BMW();
            return bmw;
        }

        public static Mercedes CreateMercedes()
        {
            Mercedes merc = new Mercedes();
            return merc;
        }

        public static Audi CreateAudi()
        {
            Audi audi = new Audi();
            return audi;
        }

        public static Car CreateCar(BMW b, Mercedes m, Audi a)
        {
            var newcar = CreateCar(b, m, a);
            return newcar;
        }

        

    }
}
