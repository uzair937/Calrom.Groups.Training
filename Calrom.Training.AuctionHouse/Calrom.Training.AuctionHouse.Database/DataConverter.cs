using System;
using System.Collections.Generic;
using System.Text;

namespace Calrom.Training.AuctionHouse.Database
{
    public class DataConverter
    {
        private static DataConverter Instance = null;
        private static readonly object padlock = new object();
        public static DataConverter GetInstance
        {
            get
            {
                if (Instance == null)
                {
                    lock (padlock)
                    {
                        if (Instance == null)
                        {
                            Instance = new DataConverter();
                        }
                    }
                }
                return Instance;
            }
        }

        public void ConvertProduct(ProductDatabaseModel productDatabaseModel)
        {
            ProductModel productModel = new ProductModel()
            {
                ItemName = productDatabaseModel.ItemName,
                ItemDescription = productDatabaseModel.ItemDescription,
                ItemPrice = productDatabaseModel.ItemPrice,
                CurrentBid = productDatabaseModel.CurrentBid
            };
            using (var dbSession = NHibernateHelper.OpenSession())
            {
                dbSession.SaveOrUpdate(productModel);
                dbSession.Flush();
            }
        }

        public void ConvertBid(BidDatabaseModel bidDatabaseModel)
        {
            BidModel bidModel = new BidModel();
            using (var dbSession = NHibernateHelper.OpenSession())
            {
                bidModel.Product = GetProduct(bidDatabaseModel.ItemID);
                bidModel.User = GetUser(bidDatabaseModel.UserID);
                dbSession.SaveOrUpdate(bidModel);
            }
        }

        public void ConvertUser(UserDatabaseModel userDatabaseModel)
        {
            UserModel userModel = new UserModel()
            {
                Username = userDatabaseModel.Username,
                Password = userDatabaseModel.Password,
                DateOfBirth = userDatabaseModel.DateOfBirth
            };
            using (var dbSession = NHibernateHelper.OpenSession())
            {
                dbSession.SaveOrUpdate(userModel);
                dbSession.Flush();
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
