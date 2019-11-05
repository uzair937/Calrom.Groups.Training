using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Calrom.Training.AuctionHouse.Web.Models
{
    public class LoginViewModel
    {
        public List<UserViewModel> UserList { get; set; }

        public LoginViewModel()
        {
            UserList = new List<UserViewModel>();
        }
    }
}