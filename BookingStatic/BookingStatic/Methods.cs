using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingStatic
{
    public static class SectorCompare
    {
        public static bool Compare(Sector one, Sector two)
        {
            return (one.DepartureDate == two.DepartureDate && one.OperatingCabin == two.OperatingCabin && FlightCompare.Compare(one, two));
        }
    }
    public static class FlightCompare
    {
        public static bool Compare(flight one, flight two)
        {
            return (one.FlightNumber == two.FlightNumber && JourneyCompare.Compare(one, two));
        }
    }
    public static class JourneyCompare
    {
        public static bool Compare(journey one, journey two)
        {
            return (one.ArrivalAirportID == two.ArrivalAirportID && one.DepartureAirportID == two.DepartureAirportID);
        }
    }
    public static class Find
    {
        public static int[] Booking(int input, List<Booking> Bookings)
        {
            int count = 0;
            for (int i = 0; i < Bookings.Count; i++) for (int j = 0; j < Bookings[i].BookingParts.Count; j++) for (int k = 0; k < Bookings[i].BookingParts[j].Sectors.Count; k++)
            {
                if (count++ == input) { int[] BookingIJK = { i, j, k }; return BookingIJK; }
            }
            int[] fail = { 0, 0, 0 };
            return fail;
        }
    }
}
