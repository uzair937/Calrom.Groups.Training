using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Calrom.Training.AuctionHouse.Database;

namespace Calrom.Training.AuctionHouse.Web.Models
{
    public class LoginViewModel
    {
        public List<LoginViewModel> UserList { get; set; }
        public int UserID { get; set; }
        public string Username { get; set; }
        public string DateOfBirth { get; set; }
        public string Password { get; set; }
        public bool IsAuthenticated { get; set; }
    }
}