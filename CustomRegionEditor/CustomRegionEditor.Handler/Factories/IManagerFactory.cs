using CustomRegionEditor.Handler.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomRegionEditor.Handler.Factories
{
    public interface IManagerFactory
    {
        ICustomRegionManager CreateCustomRegionManager();
    }
}
