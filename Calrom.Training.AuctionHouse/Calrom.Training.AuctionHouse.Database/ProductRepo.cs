using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;

namespace Calrom.Training.AuctionHouse.Database
{
    public class ProductRepo : IRepository<ProductDatabaseModel>
    {
        private static ProductRepo Instance = null;
        private static readonly object padlock = new object();
        public static ProductRepo getInstance
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
            var result = BitConverter.ToInt32(salt,0) & int.MaxValue;
            return result;
        }

        ProductDatabaseModel _productContext;
        public ProductRepo()
        {
            _productContext = new ProductDatabaseModel();
        }
        public void Add(ProductDatabaseModel entity)
        {
            entity.ItemID = GetRandom();
            _productContext.ProductList.Add(entity);
        }

        public List<ProductDatabaseModel> List()
        {
            return _productContext.ProductList;
        }
    }
}
