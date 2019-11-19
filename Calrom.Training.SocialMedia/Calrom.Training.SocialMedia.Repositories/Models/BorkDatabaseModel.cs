using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Calrom.Training.SocialMedia.Database.Models
{
    public class BorkModel
    {
        public DateTime DateBorked { get; set; }
        public string BorkText { get; set; }
        public int UserId { get; set; }

    }
}