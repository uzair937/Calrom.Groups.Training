using CustomRegionEditor.Database.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomRegionEditor.Database.Interfaces
{
    public interface ICustomRegionGroupTempRepo
    {
        void Add(CustomRegionGroupModel model);
        void Delete(CustomRegionGroupModel model);
        void DeleteSubregion(CustomRegionEntryModel model);
        void DestroySession();
        IList<CustomRegionGroupModel> List();
        void Update(IList<CustomRegionEntryModel> list, string regionId);
    }
}
