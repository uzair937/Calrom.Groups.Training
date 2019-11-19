using System.ComponentModel.DataAnnotations;
using Xunit.Sdk;

namespace Calrom.Training.AuctionHouse.Database
{
    public class UserDatabaseModel
    {
        public virtual int UserID { get; set; }
        [Required(ErrorMessage = "Please enter a username.")]
        public virtual string Username { get; set; }
        [Required(ErrorMessage = "Please your date of birth.")]
        public virtual string DateOfBirth { get; set; }
        [Required(ErrorMessage = "You must enter a password.")]
        public virtual string Password { get; set; }
    }
}