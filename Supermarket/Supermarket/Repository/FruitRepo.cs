using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Supermarket
{
    public class FruitRepo : IFruitRepo
    {
        private readonly FruitContext fruitContext;

        public FruitRepo()
        {
            fruitContext = new FruitContext();
        }
        public FruitRepo(FruitContext context)
        {
            fruitContext = context;
        }
        public void Delete(int FruitID)
        {
            FruitDB fruitDB = fruitContext.fruitDB.Find(FruitID);
            fruitContext.fruitDB.Remove(fruitDB);
        }

        public IEnumerable<FruitDB> GetAll()
        {
            return fruitContext.fruitDB.ToList();
        }

        public FruitDB GetById(int FruitID)
        {
            return fruitContext.fruitDB.Find(FruitID);
        }

        public void Insert(FruitDB fruit)
        {
            fruitContext.fruitDB.Add(fruit);
        }

        public void Save()
        {
            fruitContext.SaveChanges();
        }

        public void Update(FruitDB fruit)
        {
            fruitContext.Entry(fruit).State = System.Data.Entity.EntityState.Modified;
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    fruitContext.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
