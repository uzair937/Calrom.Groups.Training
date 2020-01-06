using CustomRegionEditor.Database.Models;
using System.Collections.Generic;

namespace CustomRegionEditor.Database.Interfaces
{
    public interface ICustomRegionEntryRepository : IRepository<CustomRegionEntry>
    {
        void Delete(List<CustomRegionEntry> entity);
    }
}
