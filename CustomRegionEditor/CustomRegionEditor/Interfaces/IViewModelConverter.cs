using CustomRegionEditor.Models;
using CustomRegionEditor.ViewModels;
using System.Collections.Generic;

namespace CustomRegionEditor.Web.Interfaces
{
    public interface IViewModelConverter
    {
        CustomRegionEntryViewModel GetView(CustomRegionEntryModel customRegionEntryModel);

        CustomRegionGroupViewModel GetView(CustomRegionGroupModel customRegionGroupModel);

        CustomRegionGroupModel GetModel(CustomRegionGroupViewModel customRegionGroupViewModel);

        CustomRegionEntryModel GetModel(CustomRegionEntryViewModel customRegionGroupViewModel);

        List<CustomRegionGroupViewModel> GetView(List<CustomRegionGroupModel> customRegionGroupModels);

        ErrorViewModel GetView(ErrorModel customRegionGroupModels);

        List<AirportViewModel> GetView(List<AirportModel> oldModels);

        List<CityViewModel> GetView(List<CityModel> oldModels);

        List<StateViewModel> GetView(List<StateModel> oldModels);

        List<CountryViewModel> GetView(List<CountryModel> oldModels);

        List<RegionViewModel> GetView(List<RegionModel> oldModels);

        List<ErrorViewModel> GetView(List<ErrorModel> oldModels);
    }
}
