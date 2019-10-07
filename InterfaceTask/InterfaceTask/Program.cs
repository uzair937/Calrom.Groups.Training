using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace InterfaceTask
{
    public enum BookingStatus
    {
        bks_id = 0,
        BROPTN = 1,
        BCANCL = 2,
        BQUOTE = 3,
        BBLCOM = 4,
        BCNFIN = 5,
        BOFFER = 6,
        BDPCOM = 7
    }

    public enum Cabin
    {
        AAECO = 0,
        AAPEC = 1,
        AABAF = 2,
        AAFST = 3,
        AABUS = 4
    }

    class Booking
    {
        public List<BookingPart> bookingParts = new List<BookingPart>(); //creates a list of bookingParts
        public string bookingID { get; set; }
        public string bookingRef { get; set; }
        public DateTime bookDate { get; set; }
        public BookingStatus bookingStatus { get; set; }
    }

    class BookingPart
    {
        public string bookingPartID { get; set; }
        public string pnrRef { get; set; }
        public int groupSize { get; set; }
        public List<Sector> sectors = new List<Sector>();
    }

    class Sector : Flight
    {
        public string sectorID { get; set; }
        //public string departureID { get; set; }
        //public string arrivalID { get; set; }
        public DateTime departTime { get; set; }
        public DateTime arrivalTime { get; set; }
        public string operatorID { get; set; }
        public decimal cost { get; set; }
        public Cabin cabin { get; set; }
    }

    class Program
    {
        static void Main(string[] args)
        {
            var bookings = new List<Booking>();
            try
            {
                using (StreamReader streamReader = new StreamReader("C:/Users/ufoolat/Downloads/500-bookings.txt"))
                {
                    streamReader.ReadLine();
                    string line;
                    string[] splits;

                    while ((line = streamReader.ReadLine()) != null)
                    {
                        Booking booking = new Booking();
                        BookingPart bookingPart = new BookingPart();
                        Sector sector = new Sector();
                        splits = line.Split(new char[] { '\t' }, StringSplitOptions.RemoveEmptyEntries);
                        booking.bookingID = splits[0];
                        booking.bookingRef = splits[3];
                        booking.bookDate = Convert.ToDateTime(splits[6]);
                        booking.bookingStatus = (BookingStatus)Enum.Parse(typeof(BookingStatus), splits[14], true);
                        bookingPart.bookingPartID = splits[16];
                        bookingPart.pnrRef = splits[28];
                        bookingPart.groupSize = Convert.ToInt32(splits[47]);
                        sector.sectorID = splits[131];
                        if (splits[136].Any(char.IsDigit))
                        {
                            sector.departTime = Convert.ToDateTime(splits[135]);
                            sector.arrivalTime = Convert.ToDateTime(splits[136]);
                        }
                        else
                        {
                            sector.departTime = Convert.ToDateTime(splits[134]);
                            sector.arrivalTime = Convert.ToDateTime(splits[135]);
                        }
                        sector.departureID = splits[133];
                        sector.arrivalID = splits[134];
                        if (splits[186].Contains(".")) //value changes positions
                        {
                            sector.cost = Convert.ToDecimal(splits[186]);
                        }
                        else
                        {
                            sector.cost = Convert.ToDecimal(splits[185]);
                        }
                        if (splits[142].All(char.IsDigit))
                        {
                            sector.operatorID = splits[144];
                            sector.cabin = (Cabin)Enum.Parse(typeof(Cabin), splits[143], true);
                            sector.flightNumber = Convert.ToInt32(splits[142]);
                        }
                        else
                        {
                            sector.operatorID = splits[143];
                            sector.cabin = (Cabin)Enum.Parse(typeof(Cabin), splits[142], true);
                            sector.flightNumber = Convert.ToInt32(splits[141]);
                        }

                        if (bookings.Count == 0)
                        {
                            bookingPart.sectors.Add(sector);
                            booking.bookingParts.Add(bookingPart);
                            bookings.Add(booking);
                        }
                        else
                        {
                            var foundBooking = false;
                            for (int i = 0; i < bookings.Count; i++)
                            {
                                var currentBooking = bookings[i];
                                if (currentBooking.bookingID == bookings[i].bookingID)
                                {
                                    foundBooking = true;
                                    var foundBookingPart = false;
                                    for (int j = 0; j < currentBooking.bookingParts.Count; j++)
                                    {
                                        if (currentBooking.bookingParts[j].bookingPartID == bookingPart.bookingPartID)
                                        {
                                            foundBookingPart = true;
                                            var foundSector = false;
                                            for (int k = 0; k < currentBooking.bookingParts[j].sectors.Count; k++)
                                            {
                                                if (currentBooking.bookingParts[j].sectors[k].sectorID == sector.sectorID)
                                                {
                                                    foundSector = true;
                                                    break;
                                                }
                                            }

                                            if (!foundSector)
                                            {
                                                bookings[i].bookingParts[j].sectors.Add(sector);
                                            }
                                        }

                                        if (!foundBookingPart)
                                        {
                                            currentBooking.bookingParts.Add(bookingPart);
                                            break;
                                        }
                                    }

                                    if (!foundBooking)
                                    {
                                        bookings.Add(booking);
                                    }
                                }
                            }
                        }
                    }
                    streamReader.Close();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("The file is unreadable.");
                Console.WriteLine(e.Message);
            }
            finally
            {
                ConsoleWriter1 consoleWriter1 = new ConsoleWriter1();
                ConsoleWriter2 consoleWriter2 = new ConsoleWriter2();
                string headers = string.Format("{0} || {1} || {2} || {3} || {4} || {5} || {6} || {7} || {8} || {9} || {10} || {11} || {12} || {13} || {14} || {15}",
                    "Booking ID:", "Booking Reference:", "Booking Date:", "Booking Status:",
                    "Booking Part ID:", "Booking PNR Reference:", "Group Size:",
                    "Sector ID:", "Depature ID:", "Arrival ID:", "Depature Time:", "Arrival Time:", "Operator ID:", "Cost:", "Cabin:", "Flight Number:");
                //consoleWriter1.WriteConsole(headers);
                try
                {
                    using (StreamWriter streamWriter = new StreamWriter("C:/Users/ufoolat/Downloads/filteredBookings.txt"))
                    {
                        streamWriter.WriteLine(headers);
                        foreach (Booking bk in bookings)
                        {
                            foreach (BookingPart pt in bk.bookingParts)
                            {
                                foreach (Sector st in pt.sectors)
                                {
                                    string result = string.Format("{0} || {1} || {2} || {3} || {4} || {5} || {6} || {7} || {8} || {9} || {10} || {11} || {12} || {13} || {14} || {15}",
                                    bk.bookingID, bk.bookingRef, bk.bookDate, bk.bookingStatus,
                                    pt.bookingPartID, pt.pnrRef, pt.groupSize,
                                    st.sectorID, st.departureID, st.arrivalID, st.departTime, st.arrivalTime, st.operatorID, st.cost, st.cabin, st.flightNumber);
                                    //consoleWriter2.WriteConsole(result);
                                    streamWriter.WriteLine(result);
                                }
                            }
                        }
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("The file is unwritable.");
                    Console.WriteLine(e.Message);
                }

                List<Sector> secList = new List<Sector>();

                Console.WriteLine("");
                consoleWriter1.WriteConsole("Please enter two values by pressing enter between each of them.");
                int arg1 = Convert.ToInt32(Console.ReadLine());
                int arg2 = Convert.ToInt32(Console.ReadLine());
                JourneyComparer journeyComparer = new JourneyComparer();
                FlightComparer flightComparer = new FlightComparer();
                SectorComparer sectorComparer = new SectorComparer();

                try
                {
                    using (StreamWriter streamWriter = new StreamWriter("C:/Users/ufoolat/Downloads/comparison.txt"))
                    {
                        foreach (Booking bk in bookings)
                        {
                            foreach (BookingPart pt in bk.bookingParts)
                            {
                                foreach (Sector st in pt.sectors)
                                {
                                    secList.Add(st);
                                }
                            }
                        }

                        consoleWriter1.WriteConsole("Is the journey the same? " + journeyComparer.Compare(secList[arg1], secList[arg2]));
                        consoleWriter2.WriteConsole("Journey one: " + secList[arg1].departureID + " " + secList[arg1].arrivalID + " || " +
                            "Journey two: " + secList[arg2].departureID + " " + secList[arg2].arrivalID);
                        consoleWriter1.WriteConsole("Is the flight the same? " + flightComparer.Compare(secList[arg1], secList[arg2]));
                        consoleWriter2.WriteConsole("Flight one: " + secList[arg1].flightNumber + " " + "Flight two: " + secList[arg2].flightNumber);
                        consoleWriter1.WriteConsole("Are the sectors the same? " + sectorComparer.Compare(secList[arg1], secList[arg2]));
                        consoleWriter2.WriteConsole("Sector one: " + secList[arg1].sectorID + " " + "Sector two: " + secList[arg2].sectorID);

                        streamWriter.WriteLine("Is the journey the same? " + journeyComparer.Compare(secList[arg1], secList[arg2]));
                        streamWriter.WriteLine("Journey one: " + secList[arg1].departureID + " " + secList[arg1].arrivalID + " || " +
                            "Journey two: " + secList[arg2].departureID + " " + secList[arg2].arrivalID);
                        streamWriter.WriteLine("Is the flight the same? " + flightComparer.Compare(secList[arg1], secList[arg2]));
                        streamWriter.WriteLine("Flight one: " + secList[arg1].flightNumber + " " + "Flight two: " + secList[arg2].flightNumber);
                        streamWriter.WriteLine("Are the sectors the same? " + sectorComparer.Compare(secList[arg1], secList[arg2]));
                        streamWriter.WriteLine("Sector one: " + secList[arg1].sectorID + " || " + "Sector two: " + secList[arg2].sectorID);
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
            Console.ReadKey();
        }
    }

    /*To store the data for each of the below classes, do the same as before by creating an object of each of them and storing that object into a list. 
     It should theoretically be possible to directly compare the two objects by retrieving them from the List without needing to use specific values.
     To compare two different objects such as Journey and Flight, using ID's is probably gonna be the most appropriate way to do so.*/

    class ConsoleWriter1 : IConsoleWriter
    {
        public void WriteConsole(string booking)
        {
            Console.WriteLine("*+*\\!!!/*+* " + booking + " ===--+\\/+--===");
        }
    }

    class ConsoleWriter2 : IConsoleWriter
    {
        public void WriteConsole(string booking)
        {
            Console.WriteLine("\\~(0_0)~/ " + booking + " <--(O-O)-->");
        }
    }

    interface IConsoleWriter
    {
        void WriteConsole(string booking);
    }
    
    class Journey
    {
        //contain start and end airports
        public string departureID { get; set; }
        public string arrivalID { get; set; }
    }

    class Flight : Journey
    {
        //airports and flight number
        public int flightNumber { get; set; }
    }

    class JourneyComparer
    {
        public bool Compare(Journey journey1, Journey journey2)
        {
            return journey1.departureID == journey2.departureID && journey1.arrivalID == journey2.arrivalID;
        }
    }

    class FlightComparer
    {
        //pass two variables to a boolean for each of these classes to see if they match
        public bool Compare(Flight flight1, Flight flight2)
        {
            JourneyComparer journeyComparer = new JourneyComparer();
            return flight1.flightNumber == flight2.flightNumber && journeyComparer.Compare(flight1, flight2);
        }
    }

    class SectorComparer : IComparer
    {
        public bool Compare(Sector sector1, Sector sector2)
        {
            FlightComparer flightComparer = new FlightComparer();
            return sector1.sectorID == sector2.sectorID && flightComparer.Compare(sector1, sector2);
        }
    }

    interface IComparer
    {
        bool Compare(Sector sector1, Sector sector2);

    }
}