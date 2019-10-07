using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace BookingStatic
{
    static class Program
    {
        static void Main(string[] args)
        {
            string FilteredPath = (@"C:\Users\wbooth\Documents\booktask\filteredbookings.txt");
            string BookingPath = (@"C:\Users\wbooth\Documents\booktask\500-bookings.txt");
            string FilteredCsv = (@"C:\Users\wbooth\Documents\booktask\filteredCsv.txt");
            List<Booking> Bookings = new List<Booking>();
            bool added = false;
            try
            {
                using (StreamReader sr = new StreamReader(BookingPath)) ////filter
                {
                    using (StreamWriter sw = new StreamWriter(FilteredPath))
                    {
                        while (!sr.EndOfStream)
                        {
                            string[] LineSplit = sr.ReadLine().Split('\t');
                            sw.Write(LineSplit[0] + "\t");
                            sw.Write(LineSplit[3] + "\t");
                            sw.Write(LineSplit[6] + "\t");
                            sw.Write(LineSplit[14] + "\t");
                            sw.Write(LineSplit[28] + "\t");
                            sw.Write(LineSplit[32] + "\t");
                            sw.Write(LineSplit[47] + "\t");             ////generates reduced text file
                            sw.Write(LineSplit[131] + "\t");
                            sw.Write(LineSplit[133] + "\t");
                            sw.Write(LineSplit[134] + "\t");
                            sw.Write(LineSplit[135] + "\t");
                            sw.Write(LineSplit[136] + "\t");
                            sw.Write(LineSplit[137] + "\t");
                            sw.Write(LineSplit[143] + "\t");
                            sw.Write(LineSplit[186] + "\t");
                            sw.Write(LineSplit[142] + "\n");
                        }
                    }
                }
                using (StreamReader sr = new StreamReader(FilteredPath))
                {
                    sr.ReadLine();
                    while (!sr.EndOfStream)
                    {
                        int check = 0, checkPart = 0, checkSec = 0;
                        String[] LineSplit = sr.ReadLine().Split('\t');
                        for (check = 0; check < Bookings.Count && !added; check++) if (LineSplit[0] == Bookings[check].BookingID) added = true;
                        if (!added)
                        {
                            Booking BookingTemp = new Booking();
                            BookingTemp.BookingID = LineSplit[0];
                            BookingTemp.BookingReference = LineSplit[1];                    ////adds a new booking if the ID is unique
                            BookingTemp.BookingDate = Convert.ToDateTime(LineSplit[2]);
                            BookingTemp.BookingStatusID = (BookingStatusEnum)Enum.Parse(typeof(BookingStatusEnum), LineSplit[3], true);
                            Bookings.Add(BookingTemp);
                        }

                        added = false;
                        for (check = 0; check < Bookings.Count && added; check++) for (checkPart = 0; checkPart < Bookings[check].BookingParts.Count && !added; checkPart++) if (LineSplit[4] == Bookings[check].BookingParts[checkPart].BookingPartID) added = true;
                        if (!added)
                        {
                            for (check = 0; check < Bookings.Count && !added; check++) if (LineSplit[0] == Bookings[check].BookingID)
                            {
                                 BookingPart BookingPartTemp = new BookingPart();
                                 BookingPartTemp.BookingPartID = LineSplit[4];
                                 BookingPartTemp.PnrReference = LineSplit[5];                      ////adds new booking parts when the ID is new
                                 BookingPartTemp.GroupSize = Convert.ToInt16(LineSplit[6]);
                                 Bookings[check].BookingParts.Add(BookingPartTemp);
                                 added = true;
                            }
                        }

                        added = false;
                        for (check = 0; check < Bookings.Count && !added; check++) for (checkPart = 0; checkPart < Bookings[check].BookingParts.Count && !added; checkPart++) for (checkSec = 0; checkSec < Bookings[check].BookingParts[checkPart].Sectors.Count && !added; checkSec++)
                        {
                            if (LineSplit[7] == Bookings[check].BookingParts[checkPart].Sectors[checkSec].SectorID)
                            {
                                Bookings[check].BookingParts[checkPart].Sectors[checkSec].SectorFare += Convert.ToDecimal(LineSplit[14]);
                                added = true;                   ////If sectorID is the same this sector adds the fares together
                            }
                        }

                        if (!added)
                        {
                            for (check = 0; check < Bookings.Count && !added; check++) for (checkPart = 0; checkPart < Bookings[check].BookingParts.Count && !added; checkPart++) if (LineSplit[4] == Bookings[check].BookingParts[checkPart].BookingPartID)
                                {
                                    Sector SectorTemp = new Sector();
                                    SectorTemp.SectorID = LineSplit[7];
                                    SectorTemp.DepartureAirportID = LineSplit[8];
                                    SectorTemp.ArrivalAirportID = LineSplit[9];
                                    SectorTemp.DepartureDate = Convert.ToDateTime(LineSplit[10]);           ////Adds each individual booking as long as the sector ID is unique
                                    SectorTemp.ArrivalDate = Convert.ToDateTime(LineSplit[11]);
                                    SectorTemp.OperatingAirline = LineSplit[12];
                                    SectorTemp.OperatingCabin = (OperatingCabinEnum)Enum.Parse(typeof(OperatingCabinEnum), LineSplit[13], true);
                                    SectorTemp.SectorFare = Convert.ToDecimal(LineSplit[14]);
                                    SectorTemp.FlightNumber = LineSplit[15];
                                    Bookings[check].BookingParts[checkPart].Sectors.Add(SectorTemp);
                                    added = true;
                                }
                        }

                        added = false;
                    }
                }
                using (StreamWriter sw = new StreamWriter(FilteredCsv))
                {
                    sw.Write("BookingID,\t");
                    sw.Write("BookingReference,\t");
                    sw.Write("BookingDate,\t");
                    sw.Write("BookingPartID,\t");
                    sw.Write("PnrReference,\t");
                    sw.Write("GroupSize,\t");
                    sw.Write("SectorID,\t");                      ////prints sorted csv output
                    sw.Write("DepartureAirport,\t");
                    sw.Write("ArrivalAirport,\t");
                    sw.Write("DepartureDate,\t");
                    sw.Write("ArrivalDate,\t");
                    sw.Write("OperatingAirline,\t");
                    sw.Write("OperatingCabin,\t");
                    sw.Write("SectorFare,\t");
                    sw.Write("\n");
                    for (int i = 0; i < Bookings.Count; i++) for (int j = 0; j < Bookings[i].BookingParts.Count; j++) for (int k = 0; k < Bookings[i].BookingParts[j].Sectors.Count; k++)
                            {
                                sw.Write(Bookings[i].BookingID + ",\t");
                                sw.Write(Bookings[i].BookingReference + ",\t");
                                sw.Write(Bookings[i].BookingDate + ",\t");
                                sw.Write(Bookings[i].BookingParts[j].BookingPartID + ",\t");
                                sw.Write(Bookings[i].BookingParts[j].PnrReference + ",\t");
                                sw.Write(Bookings[i].BookingParts[j].GroupSize + ",\t");
                                sw.Write(Bookings[i].BookingParts[j].Sectors[k].SectorID + ",\t");
                                sw.Write(Bookings[i].BookingParts[j].Sectors[k].DepartureAirportID + ",\t");
                                sw.Write(Bookings[i].BookingParts[j].Sectors[k].ArrivalAirportID + ",\t");
                                sw.Write(Bookings[i].BookingParts[j].Sectors[k].DepartureDate + ",\t");
                                sw.Write(Bookings[i].BookingParts[j].Sectors[k].ArrivalDate + ",\t");
                                sw.Write(Bookings[i].BookingParts[j].Sectors[k].OperatingAirline + ",\t");
                                sw.Write(Bookings[i].BookingParts[j].Sectors[k].OperatingCabin + ",\t");
                                sw.Write(Bookings[i].BookingParts[j].Sectors[k].SectorFare + ",\t");
                                sw.Write("\n");
                            }
                }   
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            Console.WriteLine("Done.");
            while (true) //Compare different objects, followed by child to parent and parent to child comparisons
            {
                Console.WriteLine("Enter two booking rows to be compared from the Filtered Csv");
                int CompOne = Convert.ToInt16(Console.ReadLine());
                int CompTwo = Convert.ToInt16(Console.ReadLine());
                int[] BookingOne = Find.Booking(CompOne, Bookings);
                int[] BookingTwo = Find.Booking(CompTwo, Bookings);
                Sector SecRefOne = Bookings[BookingOne[0]].BookingParts[BookingOne[1]].Sectors[BookingOne[2]];
                Sector SecRefTwo = Bookings[BookingTwo[0]].BookingParts[BookingTwo[1]].Sectors[BookingTwo[2]];
                Sector CompareSectorOne = SecRefOne;
                Sector CompareSectorTwo = SecRefTwo;
                flight CompareFlightOne = new flight { FlightNumber = SecRefOne.FlightNumber, ArrivalAirportID = SecRefOne.ArrivalAirportID, DepartureAirportID = SecRefOne.DepartureAirportID };
                flight CompareFlightTwo = new flight { FlightNumber = SecRefTwo.FlightNumber, ArrivalAirportID = SecRefTwo.ArrivalAirportID, DepartureAirportID = SecRefTwo.DepartureAirportID };
                journey CompareJourneyOne = new journey { ArrivalAirportID = SecRefOne.ArrivalAirportID, DepartureAirportID = SecRefOne.DepartureAirportID };
                journey CompareJourneyTwo = new journey { ArrivalAirportID = SecRefTwo.ArrivalAirportID, DepartureAirportID = SecRefTwo.DepartureAirportID };
                Console.WriteLine("Are the Journeys the same? - " + Convert.ToString(JourneyCompare.Compare(CompareJourneyOne, CompareJourneyTwo)));
                Console.WriteLine("ArrivalAirport - " + Convert.ToString(CompareJourneyOne.ArrivalAirportID) + " " + Convert.ToString(CompareJourneyTwo.ArrivalAirportID));
                Console.WriteLine("DepartureAirport - " + Convert.ToString(CompareJourneyOne.DepartureAirportID) + " " + Convert.ToString(CompareJourneyTwo.DepartureAirportID));
                Console.WriteLine("Are the Flights the same? - " + Convert.ToString(FlightCompare.Compare(CompareFlightOne, CompareFlightTwo)));
                Console.WriteLine("FlightNumber - " + Convert.ToString(CompareFlightOne.FlightNumber) + " " + Convert.ToString(CompareFlightTwo.FlightNumber));
                Console.WriteLine("Are the Sectors the same? - " + Convert.ToString(SectorCompare.Compare(CompareSectorOne, CompareSectorTwo)));
                Console.WriteLine("SectorID - " + Convert.ToString(CompareSectorOne.SectorID) + " " + Convert.ToString(CompareSectorTwo.SectorID));
                try
                {
                    Console.WriteLine("Putting Sector into FlightCompare and JourneyCompare");
                    Console.WriteLine("Are the Flights the same? - " + Convert.ToString(FlightCompare.Compare(CompareSectorOne, CompareSectorTwo)));
                    Console.WriteLine("Are the Journeys the same? - " + Convert.ToString(JourneyCompare.Compare(CompareSectorOne, CompareSectorTwo)));
                }
                catch (Exception e) { Console.WriteLine(e.Message); }
                try
                {
                    Console.WriteLine("Putting Flight into SectorCompare and JourneyCompare");
                    Console.WriteLine("Are the Sectors the same? - " + Convert.ToString(SectorCompare.Compare((Sector)(CompareFlightOne), (Sector)CompareFlightTwo)));
                    Console.WriteLine("Are the Journeys the same? - " + Convert.ToString(JourneyCompare.Compare(CompareFlightOne, CompareFlightTwo)));
                }
                catch (Exception e) { Console.WriteLine(e.Message); }
                try
                {
                    Console.WriteLine("Putting Journeys into SectorCompare and JourneyCompare");
                    Console.WriteLine("Are the Sectors the same? - " + Convert.ToString(SectorCompare.Compare((Sector)(CompareJourneyOne), (Sector)CompareJourneyTwo)));
                    Console.WriteLine("Are the Flights the same? - " + Convert.ToString(FlightCompare.Compare((flight)CompareJourneyOne, (flight)CompareJourneyTwo)));
                }
                catch (Exception e) { Console.WriteLine(e.Message); }
            }
        }
    }
}
