using CustomRegionEditor.Database.Models;
using System.Collections.Generic;

namespace CustomRegionEditor.Database.Interfaces
{
    public interface ICustomRegionEntryRepository : IRepository<CustomRegionEntryModel>
    {
        void Delete(List<CustomRegionEntryModel> entity);
    }
}
