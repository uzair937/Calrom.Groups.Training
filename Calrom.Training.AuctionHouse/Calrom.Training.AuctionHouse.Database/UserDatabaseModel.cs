using System.ComponentModel.DataAnnotations;
using Xunit.Sdk;

namespace Calrom.Training.AuctionHouse.Database
{
    public class UserDatabaseModel
    {
        public int UserID { get; set; }
        [Required(ErrorMessage = "Please enter a username.")]
        public string Username { get; set; }
        [Required(ErrorMessage = "Please your date of birth.")]
        public string DateOfBirth { get; set; }
        [Required(ErrorMessage = "You must enter a password.")]
        public string Password { get; set; }
    }
}