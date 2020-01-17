using CustomRegionEditor.Database.Models;
using System;
using System.Collections.Generic;

namespace CustomRegionEditor.Database.Interfaces
{
    public interface ICustomRegionEntryRepository : IRepository<CustomRegionEntry>
    {
        void DeleteById(Guid id);
    }
}
