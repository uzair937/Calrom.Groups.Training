using CustomRegionEditor.Database;
using CustomRegionEditor.Database.Models;
using CustomRegionEditor.EntityMapper;
using CustomRegionEditor.ViewModels;
using CustomRegionEditor.Web.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomRegionEditor.Web.Converters
{
    public class ViewModelConverter : IViewModelConverter
    {
        public CustomRegionEntryViewModel GetView(CustomRegionEntryModel customRegionEntryModel)
        {
            var newView = AutoMapperConfiguration.GetInstance<CustomRegionEntryViewModel>(customRegionEntryModel);
            if (customRegionEntryModel.Region != null)
            {
                newView.Region = AutoMapperConfiguration.GetInstance<RegionViewModel>(customRegionEntryModel.Region);
                newView.Country = new CountryViewModel { Name = string.Empty };
                newView.State = new StateViewModel { Name = string.Empty };
                newView.City = new CityViewModel { Name = string.Empty };
                newView.Airport = new AirportViewModel { Name = string.Empty };
            }
            else if (customRegionEntryModel.Country != null)
            {
                newView.Country = AutoMapperConfiguration.GetInstance<CountryViewModel>(customRegionEntryModel.Country);
                newView.Region = new RegionViewModel { Name = string.Empty };
                newView.State = new StateViewModel { Name = string.Empty };
                newView.City = new CityViewModel { Name = string.Empty };
                newView.Airport = new AirportViewModel { Name = string.Empty };
            }
            else if (customRegionEntryModel.State != null)
            {
                newView.State = AutoMapperConfiguration.GetInstance<StateViewModel>(customRegionEntryModel.State);
                newView.Country = new CountryViewModel { Name = string.Empty };
                newView.Region = new RegionViewModel { Name = string.Empty };
                newView.City = new CityViewModel { Name = string.Empty };
                newView.Airport = new AirportViewModel { Name = string.Empty };
            }
            else if (customRegionEntryModel.City != null)
            {
                newView.City = AutoMapperConfiguration.GetInstance<CityViewModel>(customRegionEntryModel.City);
                newView.Country = new CountryViewModel { Name = string.Empty };
                newView.State = new StateViewModel { Name = string.Empty };
                newView.Region = new RegionViewModel { Name = string.Empty };
                newView.Airport = new AirportViewModel { Name = string.Empty };
            }
            else if (customRegionEntryModel.Airport != null)
            {
                newView.Airport = AutoMapperConfiguration.GetInstance<AirportViewModel>(customRegionEntryModel.Airport);
                newView.Country = new CountryViewModel { Name = string.Empty };
                newView.State = new StateViewModel { Name = string.Empty };
                newView.City = new CityViewModel { Name = string.Empty };
                newView.Region = new RegionViewModel { Name = string.Empty };
            }
            return newView;
        }

        public CustomRegionGroupViewModel GetView(CustomRegionGroupModel customRegionGroupViewModel)
        {
            var newView = AutoMapperConfiguration.GetInstance<CustomRegionGroupViewModel>(customRegionGroupViewModel);
            newView.CustomRegions = new List<CustomRegionEntryViewModel>();
            if (customRegionGroupViewModel.CustomRegionEntries != null)
            {
                foreach (var cre in customRegionGroupViewModel.CustomRegionEntries)
                {
                    newView.CustomRegions.Add(GetView(cre));
                }
            }
            return newView;
        }

        public CustomRegionGroupModel GetView(CustomRegionGroupViewModel customRegionGroupViewModel)
        {
            var newView = AutoMapperConfiguration.GetInstance<CustomRegionGroupModel>(customRegionGroupViewModel);
            
            return newView;
        }

        public List<CustomRegionGroupViewModel> GetView(List<CustomRegionGroupModel> customRegionGroupViewModels)
        {
            var newList = new List<CustomRegionGroupViewModel>();
            foreach (var model in customRegionGroupViewModels)
            {
                newList.Add(GetView(model));
            }
            return newList;
        }
    }
}
