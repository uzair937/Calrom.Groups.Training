using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Supermarket
{
    public interface IFruitRepo
    {
        IEnumerable<FruitDB> GetAll();
        FruitDB GetById(int FruitID);
        void Insert(FruitDB fruit);
        void Update(FruitDB fruit);
        void Delete(int FruitID);
        void Save();
    }
}