using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Supermarket
{
    public interface IFruitBuilder
    {
        void BuildApple();
        void BuildBanana();
        void BuildOrange();
        Fruit GetFruit();
    }
}
