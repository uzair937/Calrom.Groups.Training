using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dealership
{
    public static class CarFactory
    {
        private static Car CreateBMW()
        {
            BMW bmw = new BMW();
            return bmw;
        }

        private static Car CreateMercedes()
        {
            Mercedes merc = new Mercedes();
            return merc;
        }

        private static Car CreateAudi()
        {
            Audi audi = new Audi();
            return audi;
        }

        private static Car CreateTesla()
        {
            Tesla tesla = new Tesla();
            return tesla;
        }

        private static Car CreateVW()
        {
            Volkswagen VW = new Volkswagen();
            return VW;
        }

        private static Car CreateRR()
        {
            RR rr = new RR();
            return rr;
        }

        public static Car CreateCar(Cars car)
        {
            Car newcar = null;
            switch (car)
            {
                case Cars.Mercedes:
                    newcar = CreateMercedes();
                    break;

                case Cars.BMW:
                    newcar = CreateBMW();
                    break;

                case Cars.Audi:
                    newcar = CreateAudi();
                    break;

                case Cars.RR:
                    newcar = CreateRR();
                    break;

                case Cars.Tesla:
                    newcar = CreateTesla();
                    break;

                case Cars.Volkswagen:
                    newcar = CreateVW();
                    break;
            } 

            return newcar;
        }

            //if (car == Cars.Mercedes)
            //{
            //    var merc = CreateMercedes();
            //    return merc;
            //}

            //if (car == Cars.BMW)
            //{
            //    var bmw = CreateBMW();
            //    return bmw;
            //}

            //if (car == Cars.Audi)
            //{
            //    var audi = CreateAudi();
            //    return audi;
            //}

            //if (car == Cars.Tesla)
            //{
            //    var gettesla = CreateTesla();
            //    return gettesla;
            //}

            //if (car == Cars.Volkswagen)
            //{
            //    var getVW = CreateVW();
            //    return getVW;
            //}

            //if (car == Cars.RR)
            //{
            //    var getRR = CreateRR();
            //    return getRR;
            //}
            

        
       
    }
}
