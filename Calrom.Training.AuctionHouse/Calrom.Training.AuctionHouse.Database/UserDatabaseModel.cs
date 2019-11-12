using System.Collections.Generic;

namespace Calrom.Training.AuctionHouse.Database
{
    public class UserDatabaseModel
    {
        public List<UserDatabaseModel> UserList { get; set; }
        public int UserID { get; set; }
        public string Username { get; set; }
        public string DateOfBirth { get; set; }
        public string Password { get; set; }
        public List<int> ItemIDS { get; set; }
    }
}