using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Calrom.Training.AuctionHouse.Database
{
    public class UserDatabaseModel
    {
        public List<UserDatabaseModel> UserList { get; set; }
        public int UserID { get; set; }
        public string Username { get; set; }
        public string DateOfBirth { get; set; }
        public string Password { get; set; }

        public UserDatabaseModel()
        {
            UserList = new List<UserDatabaseModel>();
        }
    }
}