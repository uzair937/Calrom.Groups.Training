using System;
using System.Collections.Generic;
using System.Text;

namespace Calrom.Training.AuctionHouse.Database
{
    public class ProductRepo : IRepository<ProductDatabaseModel>
    {
        ProductDatabaseModel _productContext;
        public ProductRepo()
        {
            _productContext = new ProductDatabaseModel();
        }
        public void Add(ProductDatabaseModel entity)
        {
            _productContext.ProductList.Add(entity);
        }

        public List<ProductDatabaseModel> List()
        {
            return _productContext.ProductList;
        }
    }
}
