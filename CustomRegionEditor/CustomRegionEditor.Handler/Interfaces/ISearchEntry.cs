using System.Collections.Generic;
using CustomRegionEditor.Models;

namespace CustomRegionEditor.Handler.Interfaces
{
    public interface ISearchEntry
    {
        CustomRegionEntryModel FindById(string id);
    }
}
