using NHibernate;
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

        public BidModel PopulateObjects(BidModel bidModel)
        {
            var updatedModel = bidModel;
            if (bidModel.Product == null)
            {
                updatedModel.Product = new ProductModel();
            }
            if (bidModel.User == null)
            {
                updatedModel.User = new UserModel();
            }
            updatedModel.User = bidModel.User;
            updatedModel.Product = bidModel.Product;
            return updatedModel;
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
            var updatedList = new List<BidModel>();
            using (var dbSession = NHibernateHelper.OpenSession())
            {
                list = dbSession.Query<BidModel>().ToList();
                
                foreach (var child in list)
                {
                   updatedList.Add(PopulateObjects(child));
                }
            }

            return updatedList;
        }
    }
}
