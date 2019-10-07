using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace Bookings
{
    public class DatabaseContext : DbContext
    {
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<BookingPart> BookingParts { get; set; }
        public DbSet<Sector> Sectors { get; set; }
    }
    public class flight : journey
    {
        public string FlightNumber { get; set; }
    }
    public class journey
    {
        public string DepartureAirportID { get; set; }
        public string ArrivalAirportID { get; set; }
    }

    public class Booking
    {
        public string BookingID { get; set; }
        public string BookingReference { get; set; }
        public DateTime BookingDate { get; set; }
        public BookingStatusEnum BookingStatusID { get; set; }
        public virtual List<BookingPart> BookingParts { get; set; }

        public Booking()
        {
            BookingParts = new List<BookingPart>();
        }
    }
    public class BookingPart
    {
        public string BookingPartID { get; set; }
        public string PnrReference { get; set; }
        public int GroupSize { get; set; }
        public virtual List<Sector> Sectors { get; set; }

        public BookingPart()
        {
            Sectors = new List<Sector>();
        }
    }
    public class Sector : flight
    {
        public string SectorID { get; set; }
        public DateTime DepartureDate { get; set; }
        public DateTime ArrivalDate { get; set; }
        public string OperatingAirline { get; set; }
        public OperatingCabinEnum OperatingCabin { get; set; }
        public decimal SectorFare { get; set; }
    }
    public enum BookingStatusEnum { BROPTN, BCANCL, BQUOTE, BBLCOM, BCNFIN, BOFFER, BDPCOM };
    public enum OperatingCabinEnum { AAECO, AABAF, AAPEC, AAFST, AABUS };
}
