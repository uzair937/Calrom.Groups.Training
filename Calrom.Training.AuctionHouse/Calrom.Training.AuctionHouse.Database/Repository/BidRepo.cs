using System.Collections.Generic;

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

        private List<BidDatabaseModel> _bidContext;
        public BidRepo()
        {
            _bidContext = new List<BidDatabaseModel>();
        }

        public void Add(BidDatabaseModel entity)
        {
            _bidContext.Add(entity);
        }

        public List<BidDatabaseModel> List()
        {
            return _bidContext;
        }
    }
}
