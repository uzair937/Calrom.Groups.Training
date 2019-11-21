using System.Collections.Generic;
using System.Linq;

namespace Calrom.Training.AuctionHouse.Database
{
    public class BidRepo : IRepository<BidDatabaseModel>
    {
        private static DataConverter DataInstance { get { return DataConverter.GetInstance; } }
        private static BidRepo Instance = null;
        private static readonly object padlock = new object();
        public static BidRepo GetInstance
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
            //_bidContext.Add(entity);
            DataInstance.ConvertBid(entity);
        }

        public List<BidDatabaseModel> List()
        {
            return _bidContext;
        }

        public List<BidModel> DBList()
        {
            var list = new List<BidModel>();
            using (var dbSession = NHibernateHelper.OpenSession())
            {
                list = dbSession.Query<BidModel>().ToList();
            }
            return list;
        }
    }
}
