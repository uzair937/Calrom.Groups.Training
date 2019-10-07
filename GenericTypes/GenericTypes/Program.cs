using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenericTypes
{
    public delegate List<Flight> myMethodDelegate(List<Flight> myFlight);
    public class Flight
    {
        public string FlightNumber { get; set; }
        public DateTime DepartureDate { get; set; }
        public DateTime ArrivalDate { get; set; }
        public string Airline { get; set; }
        public int Cost { get; set; }

        public void DisplayFlight(Flight flight)
        {
            Console.WriteLine($"{flight.FlightNumber}, {flight.DepartureDate.ToShortDateString()}, {flight.Airline}");
        }
    }
    public class delegateLinq
    {
        public List<Flight> orderByADate(List<Flight> flights)
        {
            return flights.OrderBy(a => a.ArrivalDate).ToList();            
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            MyGenericClass<int> intGenericClass = new MyGenericClass<int>(25);
            int val = intGenericClass.genericMethod(30);
            
            MyGenericClass<string> strGenericClass = new MyGenericClass<string>("Hello Generic World");
            strGenericClass.genericProperty = "This is a generic example.";
            string result = strGenericClass.genericMethod("Generic Parameter");
            var flights = new List<Flight>();
            void reinitialize()
            {
                flights.Add(new Flight() { FlightNumber = "QF0001", Airline = "QFQ", ArrivalDate = new DateTime(2019, 12, 06), DepartureDate = new DateTime(2019, 12, 01), Cost = 50 });
                flights.Add(new Flight() { FlightNumber = "QF0002", Airline = "QFW", ArrivalDate = new DateTime(2019, 12, 07), DepartureDate = new DateTime(2019, 12, 01), Cost = 50 });
                flights.Add(new Flight() { FlightNumber = "QF0003", Airline = "QFE", ArrivalDate = new DateTime(2019, 12, 04), DepartureDate = new DateTime(2019, 12, 01), Cost = 50 });
                flights.Add(new Flight() { FlightNumber = "QF0004", Airline = "QFR", ArrivalDate = new DateTime(2019, 12, 02), DepartureDate = new DateTime(2019, 12, 01), Cost = 50 });
                flights.Add(new Flight() { FlightNumber = "QF0005", Airline = "QFQ", ArrivalDate = new DateTime(2019, 12, 06), DepartureDate = new DateTime(2019, 12, 01), Cost = 50 });
                flights.Add(new Flight() { FlightNumber = "QF0006", Airline = "QFW", ArrivalDate = new DateTime(2019, 12, 03), DepartureDate = new DateTime(2019, 12, 01), Cost = 50 });
                flights.Add(new Flight() { FlightNumber = "QF0007", Airline = "QFE", ArrivalDate = new DateTime(2019, 12, 05), DepartureDate = new DateTime(2019, 12, 01), Cost = 50 });
                flights.Add(new Flight() { FlightNumber = "QF0008", Airline = "QFR", ArrivalDate = new DateTime(2019, 12, 07), DepartureDate = new DateTime(2019, 12, 01), Cost = 50 });
                flights.Add(new Flight() { FlightNumber = "QF0009", Airline = "QFQ", ArrivalDate = new DateTime(2019, 12, 09), DepartureDate = new DateTime(2019, 12, 01), Cost = 50 });
                flights.Add(new Flight() { FlightNumber = "QF00010", Airline = "QFW", ArrivalDate = new DateTime(2019, 12, 03), DepartureDate = new DateTime(2019, 12, 01), Cost = 50 });
                flights.Add(new Flight() { FlightNumber = "QF00011", Airline = "QFE", ArrivalDate = new DateTime(2019, 12, 11), DepartureDate = new DateTime(2019, 12, 01), Cost = 50 });
                flights.Add(new Flight() { FlightNumber = "QF00012", Airline = "QFR", ArrivalDate = new DateTime(2019, 12, 04), DepartureDate = new DateTime(2019, 12, 01), Cost = 50 });
                flights.Add(new Flight() { FlightNumber = "QF00013", Airline = "QFQ", ArrivalDate = new DateTime(2019, 12, 01), DepartureDate = new DateTime(2019, 12, 01), Cost = 50 });
                flights.Add(new Flight() { FlightNumber = "QF00014", Airline = "QFW", ArrivalDate = new DateTime(2019, 12, 03), DepartureDate = new DateTime(2019, 12, 01), Cost = 50 });
                flights.Add(new Flight() { FlightNumber = "QF00015", Airline = "QFE", ArrivalDate = new DateTime(2019, 12, 01), DepartureDate = new DateTime(2019, 12, 01), Cost = 50 });
                flights.Add(new Flight() { FlightNumber = "QF00016", Airline = "QFR", ArrivalDate = new DateTime(2019, 12, 08), DepartureDate = new DateTime(2019, 12, 01), Cost = 50 });
                flights.Add(new Flight() { FlightNumber = "QF00017", Airline = "QFQ", ArrivalDate = new DateTime(2019, 12, 05), DepartureDate = new DateTime(2019, 12, 01), Cost = 50 });
                flights.Add(new Flight() { FlightNumber = "QF00018", Airline = "QFW", ArrivalDate = new DateTime(2019, 12, 04), DepartureDate = new DateTime(2019, 12, 01), Cost = 50 });
                flights.Add(new Flight() { FlightNumber = "QF00019", Airline = "QFE", ArrivalDate = new DateTime(2019, 12, 03), DepartureDate = new DateTime(2019, 12, 01), Cost = 50 });
                flights.Add(new Flight() { FlightNumber = "QF00020", Airline = "QFR", ArrivalDate = new DateTime(2019, 12, 04), DepartureDate = new DateTime(2019, 12, 01), Cost = 50 });
                flights.Add(new Flight() { FlightNumber = "QF00021", Airline = "QFQ", ArrivalDate = new DateTime(2019, 12, 08), DepartureDate = new DateTime(2019, 12, 01), Cost = 50 });
                flights.Add(new Flight() { FlightNumber = "QF00022", Airline = "QFW", ArrivalDate = new DateTime(2019, 12, 09), DepartureDate = new DateTime(2019, 12, 01), Cost = 50 });
                flights.Add(new Flight() { FlightNumber = "QF00023", Airline = "QFE", ArrivalDate = new DateTime(2019, 12, 10), DepartureDate = new DateTime(2019, 12, 01), Cost = 50 });
                flights.Add(new Flight() { FlightNumber = "QF00024", Airline = "QFR", ArrivalDate = new DateTime(2019, 12, 05), DepartureDate = new DateTime(2019, 12, 01), Cost = 50 });
            }
            reinitialize();
            delegateLinq delinq = new delegateLinq();           //initialise delegate
            myMethodDelegate orderByDate = null;
            orderByDate += delinq.orderByADate;
            Action<Flight> printSearch = (s) =>
            {
                Console.WriteLine(s.Airline + ": " + s.FlightNumber); //prints the passed flight information
            };

            var flightSearchedWithLinq = flights.Where(f => f.Airline == "QFR").FirstOrDefault();
            if (flightSearchedWithLinq != null) Console.WriteLine($"QFR -> {flightSearchedWithLinq.FlightNumber}");
            else Console.WriteLine("Invalid");

            flightSearchedWithLinq = flights.Where(f => f.Airline == "QFW").FirstOrDefault();
            if (flightSearchedWithLinq != null) Console.WriteLine($"QFW -> {flightSearchedWithLinq.FlightNumber}");
            else Console.WriteLine("Invalid");

            flightSearchedWithLinq = flights.Where(f => f.Airline == "QFP").FirstOrDefault();
            if (flightSearchedWithLinq != null) Console.WriteLine($"QFP -> {flightSearchedWithLinq.FlightNumber}");
            else Console.WriteLine("Invalid");

            Console.WriteLine("Order by Date");
            var orderResult = orderByDate.Invoke(flights);                                      //Use delegate to order by arrival date
            foreach (var item in orderResult) printSearch(item);

            Console.WriteLine("Total Cost, " + flights.Sum(x => x.Cost));

            Console.WriteLine("Airline Types");

            var distinctAirlines = flights.GroupBy(a => a.Airline);
            foreach (var flight in distinctAirlines)                                                //GroupBy
                Console.WriteLine(flight.Key);


            Console.WriteLine("Order by Descending");
            flights.OrderByDescending(a => a.Airline).OrderBy(a => a.DepartureDate);
            foreach (var item in flights) printSearch(item);           //OrderDescending

            List<Flight> FuncMethod (List<Flight> f1)
            {
                f1.RemoveAll(x => x.Airline != "QFQ");
                Console.WriteLine("Remove all but QFQ");                        //RemoveAll in a local func
                return f1;
            }
            var flightCull = new List<Flight>();
            flightCull = flights;       
            FuncMethod(flightCull);
            foreach (var item in flightCull) printSearch(item);

            reinitialize();
            Console.WriteLine("Remove QFQ");                                                //reinitialise func and larger remove all
            flights.RemoveAll(x => x.Airline == "QFQ");
            foreach (var item in flights) printSearch(item);

            while (true)
            {
                Console.WriteLine("Input any search");
                string linqsearch = Console.ReadLine();                             //search loop                          
                linqsearch = linqsearch.ToUpper();          
                flightSearchedWithLinq = flights.Where(f => f.FlightNumber == linqsearch).FirstOrDefault();
                if (flightSearchedWithLinq != null) Console.WriteLine($"{linqsearch} -> {flightSearchedWithLinq.Airline}");
                else
                {
                    flightSearchedWithLinq = flights.Where(f => f.Airline == linqsearch).FirstOrDefault();
                    if (flightSearchedWithLinq != null) Console.WriteLine($"{linqsearch} -> {flightSearchedWithLinq.FlightNumber}");
                    else Console.WriteLine("Invalid");
                }
                
            }
        }
    }
    class MyGenericClass<T> where T : IConvertible
    {
        private T genericMemberVariable;

        public MyGenericClass(T value)
        {
            genericMemberVariable = value;
        }

        public T genericMethod(T genericParameter)
        {
            Console.WriteLine("Parameter type: {0}, value: {1}", typeof(T).ToString(), genericParameter);
            Console.WriteLine("Return type: {0}, value: {1}", typeof(T).ToString(), genericMemberVariable);
            if (typeof(T).Equals(typeof(int)))
            {
                int inputint = int.Parse(genericParameter.ToString());
                if (inputint > 10 && inputint < 35)
                {
                    Console.WriteLine($"Input '{inputint}' is > 10 and < 35");
                }
                else
                {
                    Console.WriteLine($"Input '{inputint}' is < 10 or > 35");
                }
            }
            else
            {
                int InputLength = genericProperty.ToString().Length;
                if (InputLength > 10 && InputLength < 35)
                {
                    Console.WriteLine($"Input '{genericProperty.ToString()}' is > 10 and < 35, {InputLength}");
                }
                else
                {
                    Console.WriteLine($"Input '{genericProperty.ToString()}' is < 10 or > 35, {InputLength}");
                }
            }

            return genericMemberVariable;
        }

        public T genericProperty { get; set; }
    }
}
