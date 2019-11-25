using NHibernate;
using System.Collections.Generic;
using System.Linq;

namespace Calrom.Training.AuctionHouse.Database
{
    public class BidRepo : IRepository<BidModel>
    {
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

        private List<BidModel> _bidContext;
        public BidRepo()
        {
            _bidContext = new List<BidModel>();
        }

        public void Add(BidModel entity)
        {
            using (var dbSession = NHibernateHelper.OpenSession())
            {
                dbSession.SaveOrUpdate(entity);
                dbSession.Flush();
            }
        }

        public List<BidModel> List()
        {
            var updatedList = new List<BidModel>();
            using (var dbSession = NHibernateHelper.OpenSession())
            {
                _bidContext = dbSession.Query<BidModel>().ToList();
                
                foreach (var child in _bidContext)
                {
                   updatedList.Add(PopulateObjects(child));
                }
            }
            return updatedList;
        }
    }
}
