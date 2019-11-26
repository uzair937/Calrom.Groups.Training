using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Calrom.Training.AuctionHouse.Database
{
    public class UserModel
    {
        public virtual int UserID { get; set; }
        public virtual string Username { get; set; }
        public virtual string DateOfBirth { get; set; }
        public virtual string Password { get; set; }
        public virtual IList<BidModel> BidList { get; set; }

        public virtual void AddBid(BidModel bidModel)
        {
            if (BidList == null)
            {
                BidList = new List<BidModel>();
            }
            BidList.Add(bidModel);
        }
        public virtual void RemoveBid(BidModel bidModel)
        {
            BidList.Remove(bidModel);
        }
    }
}