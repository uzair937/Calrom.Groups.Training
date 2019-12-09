using CustomRegionEditor.Database.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomRegionEditor.Database.Interfaces
{
    public interface ILazyLoader
    {
        CustomRegionEntryModel LoadEntities(CustomRegionEntryModel oldModel);

        List<CustomRegionEntryModel> LoadEntities(List<CustomRegionEntryModel> oldModel);   
        
        CustomRegionGroupModel LoadEntities(CustomRegionGroupModel oldModel);

        List<CustomRegionGroupModel> LoadEntities(List<CustomRegionGroupModel> oldModel);

        AirportModel LoadEntities(AirportModel oldModel);

        CityModel LoadEntities(CityModel oldModel);

        StateModel LoadEntities(StateModel oldModel);

        CountryModel LoadEntities(CountryModel oldModel);

        RegionModel LoadEntities(RegionModel oldModel);

        SystemModel LoadEntities(SystemModel oldModel);
    }
}
