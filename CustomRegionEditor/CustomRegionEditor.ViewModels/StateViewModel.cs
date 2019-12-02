using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CustomRegionEditor.Models
{
    public class StateViewModel
    {
        public int StateID { get; set; }
        public string StateName { get; set; }
        public string StateCountry { get; set; }
        public List<string> Cities { get; set; }
        public List<string> Airports { get; set; }
    }
}