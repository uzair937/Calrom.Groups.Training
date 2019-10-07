using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fundamentals
{
    class Program
    {
        static void Main(string[] args)
        {
            #region linq
            var flights = new List<Flight>();
            flights.Add(new Flight() { FlightNumber = "QF0001", Airline = "QF", ArrivalDate = new DateTime(2019, 12, 02), DepartureDate = new DateTime(2019, 12, 01) });
            flights.Add(new Flight() { FlightNumber = "QF0002", Airline = "QF", ArrivalDate = new DateTime(2019, 12, 02), DepartureDate = new DateTime(2019, 12, 01) });
            flights.Add(new Flight() { FlightNumber = "QF0003", Airline = "QF", ArrivalDate = new DateTime(2019, 12, 02), DepartureDate = new DateTime(2019, 12, 01) });
            flights.Add(new Flight() { FlightNumber = "QF0004", Airline = "QF", ArrivalDate = new DateTime(2019, 12, 02), DepartureDate = new DateTime(2019, 12, 01) });
            var groupByFlight = flights.GroupBy(f => f.FlightNumber);
            Console.WriteLine("Please enter a flight number to find the flight.");
            var number = Console.ReadLine().ToUpper();
            var flightSelected = flights.Where(f => f.FlightNumber == number).SingleOrDefault();
            #endregion

            if (flightSelected != null)
            {
                Console.WriteLine("Flight Details: " + flightSelected.FlightNumber);
                Console.WriteLine("Airline: " + flightSelected.Airline);
                Console.WriteLine("Arrival Date: " + flightSelected.ArrivalDate);
                Console.WriteLine("Departure Date: " + flightSelected.DepartureDate);
            }
            else
            {
                Console.WriteLine("This flight does not exist.");
            }
            Console.ReadKey();
        }
    }

    public class Flight
    {
        public string FlightNumber { get; set; }
        public DateTime DepartureDate { get; set; }
        public DateTime ArrivalDate { get; set; }
        public string Airline { get; set; }

        public void DisplayFlight(Flight flight)
        {
            Console.WriteLine($"{flight.FlightNumber}, {flight.DepartureDate.ToShortDateString()}, {flight.Airline}");
        }

        #region func

        #endregion
    }
}
