using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookings
{
    public class SectorCompare : FlightCompare
    {
        public bool CompareS(Sector one, Sector two)
        {
            return (one.DepartureDate == two.DepartureDate && one.OperatingCabin == two.OperatingCabin && CompareF(one, two));
        }
    }
    public class FlightCompare : JourneyCompare
    {
        public bool CompareF(flight one, flight two)
        {
            return (one.FlightNumber == two.FlightNumber && CompareJ(one, two));
        }
    }
    public class JourneyCompare
    {
        public bool CompareJ(journey one, journey two)
        {
            return (one.ArrivalAirportID == two.ArrivalAirportID && one.DepartureAirportID == two.DepartureAirportID);
        }
    }
}
