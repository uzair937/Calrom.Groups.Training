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
                ItemID = productDatabaseModel.ItemID,
                ItemName = productDatabaseModel.ItemName,
                ItemDescription = productDatabaseModel.ItemDescription,
                ItemPrice = productDatabaseModel.ItemPrice,
                CurrentBid = productDatabaseModel.CurrentBid,
                Bid = new BidModel()
                {
                    BidID = productDatabaseModel.ItemID
                }
            };
            using (var dbSession = NHibernateHelper.OpenSession())
            {
                dbSession.SaveOrUpdate(productModel);
                dbSession.Flush();
            }
        }

        public void ConvertBid(BidDatabaseModel bidDatabaseModel)
        {
            BidModel bidModel = new BidModel()
            {
                BidID = bidDatabaseModel.ItemID
            };
            using (var dbSession = NHibernateHelper.OpenSession())
            {
                ProductModel productModel = dbSession.Get<ProductModel>(bidDatabaseModel.ItemID);
                productModel.Bid = bidModel;
                dbSession.SaveOrUpdate(productModel);
            }
        }

        public void ConvertUser(UserDatabaseModel userDatabaseModel)
        {
            UserModel userModel = new UserModel()
            {
                //UserID = userDatabaseModel.UserID,
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
        
        public void GetBid(int ID)
        {
            using (var dbSession = NHibernateHelper.OpenSession())
            {
                dbSession.Get<BidModel>(ID);
            }
        }
        
        public void GetUser(int ID)
        {
            using (var dbSession = NHibernateHelper.OpenSession())
            {
                dbSession.Get<UserModel>(ID);
            }
        }
    }
}
