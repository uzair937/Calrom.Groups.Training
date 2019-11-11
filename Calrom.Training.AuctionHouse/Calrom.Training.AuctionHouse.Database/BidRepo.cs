using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;

namespace Calrom.Training.AuctionHouse.Database
{
    public class BidRepo : IRepository<BidDatabaseModel>
    {
        private static BidRepo Instance = null;
        private static readonly object padlock = new object();
        public static BidRepo getInstance
        {
            get
            {
                if (Instance == null)
                {
                    lock (padlock)
                    {
                        if (Instance == null)
                        {
                            Instance = new BidRepo();
                        }
                    }
                }
                return Instance;
            }
        }

        private BidDatabaseModel _bidContext;
        public BidRepo()
        {
            _bidContext = new BidDatabaseModel();
            _bidContext.BidList = new List<BidDatabaseModel>();
        }
        public void Update(int index, int index2, BidDatabaseModel bidDatabaseModel, UserDatabaseModel _userContext)
        {
            _userContext.UserList[index].BidList[index2] = bidDatabaseModel;
        }

        public void Add(BidDatabaseModel entity)
        {
            _bidContext.BidList.Add(entity);
        }

        public List<BidDatabaseModel> List()
        {
            return _bidContext.BidList;
        }
    }
}
