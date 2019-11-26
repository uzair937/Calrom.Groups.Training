using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;

namespace Calrom.Training.AuctionHouse.Database
{
    public class ProductRepo : IRepository<ProductModel>
    {
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

        private List<ProductModel> _productContext;

        public ProductRepo()
        {
            _productContext = new List<ProductModel>();
        }

        public void Add(ProductModel entity)
        {
            using (var dbSession = NHibernateHelper.OpenSession())
            {
                dbSession.SaveOrUpdate(entity);
                dbSession.Flush();
            }
        }

        public List<ProductModel> List()
        {
            using (var dbSession = NHibernateHelper.OpenSession())
            {
                _productContext = dbSession.Query<ProductModel>().ToList();
            }
            return _productContext;
        }
    }
}
