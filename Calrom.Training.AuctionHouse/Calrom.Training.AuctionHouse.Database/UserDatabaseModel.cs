using System.Collections.Generic;

namespace Calrom.Training.AuctionHouse.Database
{
    public class UserDatabaseModel
    {
        public int UserID { get; set; }
        public string Username { get; set; }
        public string DateOfBirth { get; set; }
        public string Password { get; set; }
    }
}