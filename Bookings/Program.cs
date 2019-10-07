using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Data.Entity;

namespace Bookings
{
    static class Program
    {
        static void Main(string[] args)
        {
            string FilteredPath = (@"C:\Users\wbooth\Documents\booktask\filteredbookings.txt");
            string BookingPath = (@"C:\Users\wbooth\Documents\booktask\500-bookings.txt");
            string FilteredCsv = (@"C:\Users\wbooth\Documents\booktask\filteredCsv.txt");
            var Bookings = new List<Booking>();
            bool added = false;
            int[] info = new int[3];
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
                        string[] LineSplit = sr.ReadLine().Split('\t');
                        for (check = 0; check < Bookings.Count && !added; check++) if (LineSplit[0] == Bookings[check].BookingID) added = true;
                        if (!added)
                        {
                            Booking BookingTemp = new Booking
                            {
                                BookingID = LineSplit[0],
                                BookingReference = LineSplit[1],                    ////adds a new booking if the ID is unique
                                BookingDate = Convert.ToDateTime(LineSplit[2]),
                                BookingStatusID = (BookingStatusEnum)Enum.Parse(typeof(BookingStatusEnum), LineSplit[3], true)
                            };
                            Bookings.Add(BookingTemp);
                        }

                        added = false;
                        for (check = 0; check < Bookings.Count && !added; check++) for (checkPart = 0; checkPart < Bookings[check].BookingParts.Count && !added; checkPart++) if (LineSplit[4] == Bookings[check].BookingParts[checkPart].BookingPartID) added = true;
                        if (!added)
                        {
                            for (check = 0; check < Bookings.Count && !added; check++) if (LineSplit[0] == Bookings[check].BookingID)
                                {
                                    BookingPart BookingPartTemp = new BookingPart
                                    {
                                        BookingPartID = LineSplit[4],
                                        PnrReference = LineSplit[5],                      ////adds new booking parts when the ID is new
                                        GroupSize = Convert.ToInt16(LineSplit[6])
                                    };
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
                                        Sector SectorTemp = new Sector
                                        {
                                            SectorID = LineSplit[7],
                                            DepartureAirportID = LineSplit[8],
                                            ArrivalAirportID = LineSplit[9],
                                            DepartureDate = Convert.ToDateTime(LineSplit[10]),           ////Adds each individual booking as long as the sector ID is unique
                                            ArrivalDate = Convert.ToDateTime(LineSplit[11]),
                                            OperatingAirline = LineSplit[12],
                                            OperatingCabin = (OperatingCabinEnum)Enum.Parse(typeof(OperatingCabinEnum), LineSplit[13], true),
                                            SectorFare = Convert.ToDecimal(LineSplit[14]),
                                            FlightNumber = LineSplit[15]
                                        };
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
                    info[0] = 0;
                    info[1] = 0;
                    info[2] = 0;
                    for (int i = 0; i < Bookings.Count; i++)
                    {
                        info[0]++;
                        for (int j = 0; j < Bookings[i].BookingParts.Count; j++)
                        {
                            info[1]++;
                            for (int k = 0; k < Bookings[i].BookingParts[j].Sectors.Count; k++)
                            {
                                info[2]++;
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
                }
                using (var db = new DatabaseContext())
                {
                    int i = 0;
                    foreach (Booking addBook in Bookings)
                    {
                        int j = 0;
                        var BookAdded = db.Bookings.Where(f => f.BookingID == addBook.BookingID).FirstOrDefault();
                        if (BookAdded == null) db.Bookings.Add(addBook);
                        else
                        {
                            addBook.BookingID = Guid.NewGuid().ToString();
                            db.Bookings.Add(addBook);
                        }
                        foreach (BookingPart addBookPart in Bookings[i].BookingParts)
                        {
                            var BookPartAdded = db.BookingParts.FirstOrDefault(f => f.BookingPartID == addBookPart.BookingPartID);
                            if (BookPartAdded == null) db.BookingParts.Add(addBookPart);
                            else
                            {
                                addBookPart.BookingPartID = Guid.NewGuid().ToString();
                                db.BookingParts.Add(addBookPart);
                            }
                            foreach (Sector addSec in Bookings[i].BookingParts[j].Sectors)
                            {
                                var SectorAdded = db.Sectors.Where(f => f.SectorID == addSec.SectorID).FirstOrDefault();
                                if (SectorAdded == null) db.Sectors.Add(addSec);
                                else 
                                {
                                    addSec.SectorID = Guid.NewGuid().ToString();
                                    db.Sectors.Add(addSec);
                                }
                            }
                            j++;
                        }
                        i++;
                    }
                    db.SaveChanges();
                }
                
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                if (e.InnerException != null) Console.WriteLine(e.InnerException.Message);
            }
            BookingForm.EntryPrep(Bookings, info);
            DisplayResults(info);
        }
        public static void DisplayResults(int[] info)
        {
            Random ran = new Random();
            int num = ran.Next(3);
            ITextOut Display = new DispayResults1();
            switch (num)
            {
                case 0:
                    Display = new DispayResults1();
                    break;
                case 1:
                    Display = new DispayResults2();
                    break;
                case 2:
                    Display = new DispayResults3();
                    break;
            }
            Display.PrintResults(info);
        }
    }
}

