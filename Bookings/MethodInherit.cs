using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookings
{
    interface ICompare
    {
        bool Compare(journey one, journey two);
    }
    public class SectorCompare : FlightCompare, ICompare
    {
        public bool Compare(Sector one, Sector two)
        {
            return (one.DepartureDate == two.DepartureDate && one.OperatingCabin == two.OperatingCabin && Compare((flight)one, (flight)two));
        }
    }
    public class FlightCompare : JourneyCompare, ICompare
    {
        public bool Compare(flight one, flight two)
        {
            return (one.FlightNumber == two.FlightNumber && CompareFR((journey)one, (journey)two));
        }
    }
    public class JourneyCompare : ICompare
    {
        public bool Compare(journey one, journey two)
        {
            return (CompareFR(one, two));
        }
        protected bool CompareFR(journey one, journey two)
        {
            return (CompareAFR(one, two));
        }
        private bool CompareAFR(journey one, journey two)
        {
            return (one.ArrivalAirportID == two.ArrivalAirportID && one.DepartureAirportID == two.DepartureAirportID);
        }
    }
    public class DispayResults1 : BookingForm, ITextOut
    {
        public void PrintResults(int[] info)
        {
            BookInterface.DetailsBox.Text = "";
            BookInterface.DetailsBox.Text += "THERE WAS A TOTAL OF\n";
            BookInterface.DetailsBox.Text += (Convert.ToString(info[0]) + " Bookings\n");
            BookInterface.DetailsBox.Text += (Convert.ToString(info[1]) + " Booking Parts\n");
            BookInterface.DetailsBox.Text += (Convert.ToString(info[2]) + " Sectors\n");
            BookInterface.DetailsBox.Text += "\\/\\/\\/\\/\\/\\/\\/\\/\\/\\/\\/";
        }
    }
    public class DispayResults2 : BookingForm, ITextOut
    {
        public void PrintResults(int[] info)
        {
            BookInterface.DetailsBox.Text = "";
            BookInterface.DetailsBox.Text += Convert.ToString(info[0]);
            BookInterface.DetailsBox.Text += "\tBOOKINGS\n";
            BookInterface.DetailsBox.Text += Convert.ToString(info[1]);
            BookInterface.DetailsBox.Text += "\tBOOKING PARTS\n";
            BookInterface.DetailsBox.Text += Convert.ToString(info[2]);
            BookInterface.DetailsBox.Text += "\tSECTORS\n";
        }
    }
    public class DispayResults3 : BookingForm, ITextOut
    {
        public void PrintResults(int[] info)
        {
            int x = 0;
            BookInterface.DetailsBox.Text = "";
            do
            {
                BookInterface.DetailsBox.Text += "AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA";
                x++;
            } while (x < 10);
        }
    }
    interface ITextOut
    {
        void PrintResults(int[] info);
    }

}
