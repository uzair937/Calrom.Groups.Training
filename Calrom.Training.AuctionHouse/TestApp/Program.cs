using Calrom.Training.AuctionHouse.Database;
using System;

namespace TestApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var product = new ProductModel()
            {
                ItemName = "test",
                ItemDescription = "description",
                ItemPrice = 5,
                ImageSrc = "ducky.jpg"
            };
            using (var dbSession = NHibernateHelper.OpenSession())
            {
                dbSession.SaveOrUpdate(product);
                dbSession.Flush();
                Console.WriteLine("Success!");
            }
            Console.WriteLine(GetProduct(1).ItemName);
            Console.ReadKey();
        }

        private static ProductModel GetProduct(int ID)
        {
            var product = new ProductModel();
            using (var dbSession = NHibernateHelper.OpenSession())
            {
                product = dbSession.Get<ProductModel>(ID);
            }
            return product;
        }
    }
}
