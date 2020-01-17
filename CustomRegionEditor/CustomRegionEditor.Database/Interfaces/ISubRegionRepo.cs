using CustomRegionEditor.Database.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomRegionEditor.Database.Interfaces
{
    public interface ISubRegionRepo<T>
    {
        T Find(string entry);

        List<CustomRegionEntry> GetSubRegions(T model);

        List<T> List();
    }
}
