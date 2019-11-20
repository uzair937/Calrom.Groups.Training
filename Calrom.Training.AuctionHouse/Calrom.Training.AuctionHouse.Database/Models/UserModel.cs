using System.ComponentModel.DataAnnotations;
using Xunit.Sdk;

namespace Calrom.Training.AuctionHouse.Database
{
    public class UserModel
    {
        public virtual int UserID { get; set; }
        public virtual string Username { get; set; }
        public virtual string DateOfBirth { get; set; }
        public virtual string Password { get; set; }
    }
}