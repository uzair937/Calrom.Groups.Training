using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Bookings
{
    public partial class BookingForm : Form
    {
        public static List<Booking> Bookings = new List<Booking>();
        public static BookingForm BookInterface = new BookingForm();
        public static int[] info;
        public BookingForm()
        {
            InitializeComponent();
        }
        public static void EntryPrep(List<Booking> BookingsComp, int[] infoSent)
        {
            info = infoSent;
            Bookings = BookingsComp;
            BookInterface.topLeftBox.Text = ("Enter two booking rows to be compared from the Filtered_Csv file");
            BookInterface.DetailsBox.Text = ("Ready.");
            Application.Run(BookInterface);
        }
        private void CompareButton_Click(object sender, EventArgs e)
        {    //Compare different objects, followed by child to parent and parent to child comparisons
            BookInterface.DetailsBox.Text = "";
            BookInterface.topLeftBox.Text = ("Enter two booking rows to be compared from the Filtered_Csv file");
            int CompOne = Convert.ToInt16(BookInterface.BookEntry1.Text);
            int CompTwo = Convert.ToInt16(BookInterface.BookEntry2.Text);
            int[] BookingOne = FindBooking(CompOne);
            int[] BookingTwo = FindBooking(CompTwo);
            Sector SecRefOne = Bookings[BookingOne[0]].BookingParts[BookingOne[1]].Sectors[BookingOne[2]];
            Sector SecRefTwo = Bookings[BookingTwo[0]].BookingParts[BookingTwo[1]].Sectors[BookingTwo[2]];
            Sector CompareSectorOne = SecRefOne;
            Sector CompareSectorTwo = SecRefTwo;
            flight CompareFlightOne = new flight { FlightNumber = SecRefOne.FlightNumber, ArrivalAirportID = SecRefOne.ArrivalAirportID, DepartureAirportID = SecRefOne.DepartureAirportID };
            flight CompareFlightTwo = new flight { FlightNumber = SecRefTwo.FlightNumber, ArrivalAirportID = SecRefTwo.ArrivalAirportID, DepartureAirportID = SecRefTwo.DepartureAirportID };
            journey CompareJourneyOne = new journey { ArrivalAirportID = SecRefOne.ArrivalAirportID, DepartureAirportID = SecRefOne.DepartureAirportID };
            journey CompareJourneyTwo = new journey { ArrivalAirportID = SecRefTwo.ArrivalAirportID, DepartureAirportID = SecRefTwo.DepartureAirportID };
            ICompare CompareTest = new JourneyCompare();
            BookInterface.DetailsBox.Text += ("Are the Journeys the same? - " + Convert.ToString(CompareTest.Compare(CompareJourneyOne, CompareJourneyTwo)));
            BookInterface.DetailsBox.Text += ("\nArrivalAirport - " + Convert.ToString(CompareJourneyOne.ArrivalAirportID) + " " + Convert.ToString(CompareJourneyTwo.ArrivalAirportID));
            BookInterface.DetailsBox.Text += ("\nDepartureAirport - " + Convert.ToString(CompareJourneyOne.DepartureAirportID) + " " + Convert.ToString(CompareJourneyTwo.DepartureAirportID));
            CompareTest = new FlightCompare();
            BookInterface.DetailsBox.Text += ("\nAre the Flights the same? - " + Convert.ToString(CompareTest.Compare(CompareFlightOne, CompareFlightTwo)));
            BookInterface.DetailsBox.Text += ("\nFlightNumber - " + Convert.ToString(CompareFlightOne.FlightNumber) + " " + Convert.ToString(CompareFlightTwo.FlightNumber));
            CompareTest = new SectorCompare();
            BookInterface.DetailsBox.Text += ("\nAre the Sector Departure Dates and Operating Cabins the same? - " + Convert.ToString(CompareTest.Compare(CompareSectorOne, CompareSectorTwo)));
            BookInterface.DetailsBox.Text += ("\nSectorID - " + Convert.ToString(CompareSectorOne.SectorID) + " " + Convert.ToString(CompareSectorTwo.SectorID));
            try
            {
                BookInterface.DetailsBox.Text += ("\nPutting Sector into FlightCompare and JourneyCompare ");
                CompareTest = new FlightCompare();
                BookInterface.DetailsBox.Text += ("\nAre the Flights the same? - " + Convert.ToString(CompareTest.Compare(CompareSectorOne, CompareSectorTwo)));
                CompareTest = new JourneyCompare();
                BookInterface.DetailsBox.Text += ("\nAre the Journeys the same? - " + Convert.ToString(CompareTest.Compare(CompareSectorOne, CompareSectorTwo)));
            }
            catch (Exception e1) { BookInterface.DetailsBox.Text += (e1.Message); }
            try
            {
                BookInterface.DetailsBox.Text += ("\nPutting Flight into SectorCompare and JourneyCompare ");
                CompareTest = new SectorCompare();
                BookInterface.DetailsBox.Text += ("\nAre the Sectors the same? - " + Convert.ToString(CompareTest.Compare((Sector)(CompareFlightOne), (Sector)CompareFlightTwo)));
                CompareTest = new JourneyCompare();
                BookInterface.DetailsBox.Text += ("\nAre the Journeys the same? - " + Convert.ToString(CompareTest.Compare(CompareFlightOne, CompareFlightTwo)));
            }
            catch (Exception e1) { BookInterface.DetailsBox.Text += (e1.Message); }
            try
            {
                BookInterface.DetailsBox.Text += ("\nPutting Journeys into SectorCompare and JourneyCompare ");
                CompareTest = new SectorCompare();
                BookInterface.DetailsBox.Text += ("\nAre the Sectors the same? - " + Convert.ToString(CompareTest.Compare((Sector)(CompareJourneyOne), (Sector)CompareJourneyTwo)));
                CompareTest = new JourneyCompare();
                BookInterface.DetailsBox.Text += ("\nAre the Flights the same? - " + Convert.ToString(CompareTest.Compare((flight)CompareJourneyOne, (flight)CompareJourneyTwo)));
            }
            catch (Exception e1) { BookInterface.DetailsBox.Text += ("\n" + e1.Message); }
        }
        static int[] FindBooking(int input)
        {
            int count = 0, i = 0, j = 0, k = 0;
            foreach (Booking book in Bookings)
            {
                j = 0;
                k = 0;
                foreach (BookingPart bookPart in Bookings[i].BookingParts)
                {
                    k = 0;
                    foreach (Sector sec in Bookings[i].BookingParts[j].Sectors)
                    {
                        if (count == input)
                        {
                            int[] BookingID = { i, j, k };
                            return BookingID;
                        }
                        k++;
                        count++;
                    }
                    j++;
                }
                i++;
            }
            int[] fail = { 0, 0, 0 };
            return fail;
        }

        private void viewButton_Click(object sender, EventArgs e)
        {
            int BookRef = Convert.ToInt16(BookInterface.viewBooking.Text);
            int[] BookingView = FindBooking(BookRef);
            int i = BookingView[0], j = BookingView[1], k = BookingView[2];
            BookInterface.DetailsBox.Text = ("");
            BookInterface.DetailsBox.Text += ("BookingID: " + Bookings[i].BookingID + ",\n");
            BookInterface.DetailsBox.Text += ("BookingReference: " + Bookings[i].BookingReference + ",\n");
            BookInterface.DetailsBox.Text += ("BookingDate: " + Bookings[i].BookingDate + ",\n");
            BookInterface.DetailsBox.Text += ("BookingPartID: " + Bookings[i].BookingParts[j].BookingPartID + ",\n");
            BookInterface.DetailsBox.Text += ("PnrReference: " + Bookings[i].BookingParts[j].PnrReference + ",\n");
            BookInterface.DetailsBox.Text += ("GroupSize: " + Bookings[i].BookingParts[j].GroupSize + ",\n");
            BookInterface.DetailsBox.Text += ("SectorID: " + Bookings[i].BookingParts[j].Sectors[k].SectorID + ",\n");
            BookInterface.DetailsBox.Text += ("DepartureAirport: " + Bookings[i].BookingParts[j].Sectors[k].DepartureAirportID + ",\n");
            BookInterface.DetailsBox.Text += ("ArrivalAirport: " + Bookings[i].BookingParts[j].Sectors[k].ArrivalAirportID + ",\n");
            BookInterface.DetailsBox.Text += ("DepartureDate: " + Bookings[i].BookingParts[j].Sectors[k].DepartureDate + ",\n");
            BookInterface.DetailsBox.Text += ("ArrivalDate: " + Bookings[i].BookingParts[j].Sectors[k].ArrivalDate + ",\n");
            BookInterface.DetailsBox.Text += ("OperatingAirline: " + Bookings[i].BookingParts[j].Sectors[k].OperatingAirline + ",\n");
            BookInterface.DetailsBox.Text += ("OperatingCabin: " + Bookings[i].BookingParts[j].Sectors[k].OperatingCabin + ",\n");
            BookInterface.DetailsBox.Text += ("SectorFare: " + Bookings[i].BookingParts[j].Sectors[k].SectorFare + ",\n");
        }
        private void RandomButt_Click(object sender, EventArgs e)
        {
            Program.DisplayResults(info);
        }
    }
}
