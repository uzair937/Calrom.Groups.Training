using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Calrom.Training.SocialMedia.ViewModels
{
    public class BorkViewModel
    {
        public DateTime DateBorked { get; set; }
        public string BorkText { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string UserPP { get; set; }
    }
}