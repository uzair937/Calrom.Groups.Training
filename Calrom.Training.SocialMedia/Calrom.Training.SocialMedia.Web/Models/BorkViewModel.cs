using Calrom.Training.SocialMedia.Database.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Calrom.Training.SocialMedia.Web.Models
{
    public class BorkViewModel
    {
        public DateTime DateBorked { get; set; }
        public string BorkText { get; set; }
        public int UserId { get; set; }

        public BorkDatabaseModel getDb()
        {
            var newBork = new BorkDatabaseModel
            {
                DateBorked = DateBorked,
                BorkText = BorkText,
                UserId = UserId
            };
            return newBork;
        }

        public BorkViewModel getView(BorkDatabaseModel getBork)
        {
            var newBork = new BorkViewModel
            {
                DateBorked = getBork.DateBorked,
                BorkText = getBork.BorkText,
                UserId = getBork.UserId
            };
            return newBork;
        }

    }
}