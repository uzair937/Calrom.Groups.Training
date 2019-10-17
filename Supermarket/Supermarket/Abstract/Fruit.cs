using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Supermarket
{
    public abstract class Fruit //dont need to derrive from grocerystock
    {
        public int stock;
        public double cost;
        public abstract void setCost();
        public abstract void setStock();
        public abstract double getCost();
        public abstract int getStock();
        public abstract bool checkStock();
        public abstract string getFruitName();
    }
} //inventory from XML maybe?
