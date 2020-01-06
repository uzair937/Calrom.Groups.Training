using System.Collections.Generic;
using CustomRegionEditor.Database.Models;
using CustomRegionEditor.Models;

namespace CustomRegionEditor.Handler.Interfaces
{
    public interface IModelConverter
    {
        CustomRegionEntry GetDbModel(CustomRegionEntryModel customRegionEntryModel);

        CustomRegionGroup GetDbModel(CustomRegionGroupModel customRegionGroupModel);

        CustomRegionEntryModel GetModel(CustomRegionEntry customRegionEntry);

        List<CustomRegionEntryModel> GetModel(List<CustomRegionEntry> customRegionGroup);

        CustomRegionGroupModel GetModel(CustomRegionGroup customRegionGroup);

        List<CustomRegionGroupModel> GetModel(List<CustomRegionGroup> customRegionGroup);

        List<RegionModel> GetModel(List<Region> regions);
        RegionModel GetModel(Region region);

        List<CountryModel> GetModel(List<Country> regions);
        CountryModel GetModel(Country region);

        List<StateModel> GetModel(List<State> regions);
        StateModel GetModel(State region);

        List<CityModel> GetModel(List<City> regions);
        CityModel GetModel(City region);

        List<AirportModel> GetModel(List<Airport> regions);
        AirportModel GetModel(Airport region);
    }
}
