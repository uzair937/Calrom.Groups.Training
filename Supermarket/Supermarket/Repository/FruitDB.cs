using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Data.Entity;
using System.Threading.Tasks;

namespace Supermarket
{
    public partial class FruitDB
    {
        public int FruitID { get; set; }
        public string FruitName { get; set; }
        public float FruitCost { get; set; }
    }
}