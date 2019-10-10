using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Supermarket
{
    public class GroceryStore
    {
        public List<Fruit> fruitList = new List<Fruit>(); //contains all the fruit and vegetables that are bought
        public List<Veg> vegList = new List<Veg>();

        public bool checkDiscount()
        {
            int aCount = 0, bCount = 0;
            foreach (Fruit fruit in fruitList)
            {
                if (fruit.fruitName.Equals("Apple"))
                {
                    aCount++;
                } else if (fruit.fruitName.Equals("Banana"))
                {
                    bCount++;
                }
            }
            if (aCount > 0 && (aCount % 3) == 0)
            {
                return true;
            }
            else if (bCount > 0 && (bCount % 5) == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
