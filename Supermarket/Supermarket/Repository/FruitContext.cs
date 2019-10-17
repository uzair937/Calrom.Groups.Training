using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace Supermarket
{
    public partial class FruitContext : DbContext
    {
        public FruitContext() : base("name=FruitContext")
        {
            //empty
        }
        public virtual DbSet<FruitDB> fruitDB { get; set; }
    }
}
