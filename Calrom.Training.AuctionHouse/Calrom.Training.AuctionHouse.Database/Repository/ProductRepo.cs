using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;

namespace Calrom.Training.AuctionHouse.Database
{
    public class ProductRepo : IRepository<ProductDatabaseModel>
    {
        private static DataConverter DataInstance{ get { return DataConverter.GetInstance; } }
        private static ProductRepo Instance = null;
        private static readonly object padlock = new object();
        public static ProductRepo GetInstance
        {
            get
            {
                if (Instance == null)
                {
                    lock (padlock)
                    {
                        if (Instance == null)
                        {
                            Instance = new ProductRepo();
                        }
                    }
                }
                return Instance;
            }
        }

        public static int GetRandom()
        {
            var rng = RandomNumberGenerator.Create();
            var salt = new byte[4];
            rng.GetBytes(salt);
            var result = BitConverter.ToInt32(salt, 0) & int.MaxValue;
            return result;
        }

        private List<ProductDatabaseModel> _productContext;

        public ProductRepo()
        {
            _productContext = new List<ProductDatabaseModel>();
        }

        public void Add(ProductDatabaseModel entity)
        {
            entity.ItemID = GetRandom();
            //_productContext.Add(entity);
            DataInstance.ConvertProduct(entity);
        }

        public List<ProductDatabaseModel> List()
        {
            return _productContext;
        }

        public List<ProductModel> DBList()
        {
            var list = new List<ProductModel>();
            using (var dbSession = NHibernateHelper.OpenSession())
            {
                list = dbSession.Query<ProductModel>().ToList();
            }
            return list;
        }
    }
}
