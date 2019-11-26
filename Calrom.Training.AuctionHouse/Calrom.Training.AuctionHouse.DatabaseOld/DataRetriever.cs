using System;
using System.Collections.Generic;
using System.Text;

namespace Calrom.Training.AuctionHouse.Database
{
    public class DataRetriever
    {
        private static DataRetriever Instance = null;
        private static readonly object padlock = new object();
        public static DataRetriever GetInstance
        {
            get
            {
                if (Instance == null)
                {
                    lock (padlock)
                    {
                        if (Instance == null)
                        {
                            Instance = new DataRetriever();
                        }
                    }
                }
                return Instance;
            }
        }

        public ProductModel GetProduct(int ID)
        {
            using (var dbSession = NHibernateHelper.OpenSession())
            {
                return dbSession.Get<ProductModel>(ID);
            }
        }

        public BidModel GetBid(int ID)
        {
            using (var dbSession = NHibernateHelper.OpenSession())
            {
                return dbSession.Get<BidModel>(ID);
            }
        }

        public UserModel GetUser(int ID)
        {
            using (var dbSession = NHibernateHelper.OpenSession())
            {
                return dbSession.Get<UserModel>(ID);
            }
        }
    }
}
