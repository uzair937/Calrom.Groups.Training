﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dealership
{
    public class RetrieveCar
    {
       public Cars GetCar1()
        {
            Cars cars = new Cars();
            cars.Buyer = "Bilal";
            cars.CarName = "Mercedes";
            cars.CarModel = "C Class";
            cars.Year = 2019;
            cars.Mileage = 990;
            cars.Colour = "White";
            cars.CarRegistration = "MB19 KNG";
            return cars;
        }

        public Cars GetCar2()
        {
            Cars cars = new Cars();
            cars.Buyer = "Burhan";
            cars.CarName = "Audi";
            cars.CarModel = "A7 Sportsback";
            cars.Year = 2018;
            cars.Mileage = 1400;
            cars.Colour = "Nardo Grey";
            cars.CarRegistration = "BK17 HNL";
            return cars;
        }

        public Cars GetCar3()
        {
            Cars cars = new Cars();
            cars.Buyer = "Atif";
            cars.CarName = "Mercedes";
            cars.CarModel = "S Class";
            cars.Year = 2019;
            cars.Mileage = 500;
            cars.Colour = "Black";
            cars.CarRegistration = "BO55";
            return cars;
        }

        public Cars GetCar4()
        {
            Cars cars = new Cars();
            cars.Buyer = "Usman";
            cars.CarName = "BMW";
            cars.CarModel = "5 Series";
            cars.Year = 2016;
            cars.Mileage = 7730;
            cars.Colour = "Navy Blue";
            cars.CarRegistration = "UZ5Y FLY";
            return cars;
        }

        public Cars GetCar5()
        {
            Cars cars = new Cars();
            cars.Buyer = "Ali";
            cars.CarName = "Mercedes";
            cars.CarModel = "C63 S AMG";
            cars.Year = 2019;
            cars.Mileage = 450;
            cars.Colour = "Black";
            cars.CarRegistration = "C K1NG";
            return cars;
        }

        public Cars GetCar6()
        {
            Cars cars = new Cars();
            cars.Buyer = "Rizwan";
            cars.CarName = "Audi";
            cars.CarModel = "RS6 Performance";
            cars.Year = 2018;
            cars.Mileage = 1500;
            cars.Colour = "Nardo Grey";
            cars.CarRegistration = "R56 R1Z";
            return cars;
        }
    }
}
