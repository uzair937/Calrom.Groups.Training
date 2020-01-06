using CustomRegionEditor.Database.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomRegionEditor.Database.Interfaces
{
    public interface IEagerLoader
    {
        CustomRegionEntry LoadEntities(CustomRegionEntry oldModel);

        List<CustomRegionEntry> LoadEntities(List<CustomRegionEntry> oldModel);   
        
        CustomRegionGroup LoadEntities(CustomRegionGroup oldModel);

        List<CustomRegionGroup> LoadEntities(List<CustomRegionGroup> oldModel);

        Airport LoadEntities(Airport oldModel);

        City LoadEntities(City oldModel);

        State LoadEntities(State oldModel);

        Country LoadEntities(Country oldModel);

        Region LoadEntities(Region oldModel);

        Models.System LoadEntities(Models.System oldModel);
    }
}
