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
        public CustomRegionViewModel GetView(CustomRegionEntryModel customRegionEntryModel)
        {
            var newView = AutoMapperConfiguration.GetInstance<CustomRegionViewModel>(customRegionEntryModel);
            if (customRegionEntryModel.reg != null)
            {
                newView.Region = AutoMapperConfiguration.GetInstance<RegionViewModel>(customRegionEntryModel.reg);
                newView.Country = new CountryViewModel { Name = string.Empty };
                newView.State = new StateViewModel { Name = string.Empty };
                newView.City = new CityViewModel { Name = string.Empty };
                newView.Airport = new AirportViewModel { Name = string.Empty };
            }
            else if (customRegionEntryModel.cnt != null)
            {
                newView.Country = AutoMapperConfiguration.GetInstance<CountryViewModel>(customRegionEntryModel.cnt);
                newView.Region = new RegionViewModel { Name = string.Empty };
                newView.State = new StateViewModel { Name = string.Empty };
                newView.City = new CityViewModel { Name = string.Empty };
                newView.Airport = new AirportViewModel { Name = string.Empty };
            }
            else if (customRegionEntryModel.sta != null)
            {
                newView.State = AutoMapperConfiguration.GetInstance<StateViewModel>(customRegionEntryModel.sta);
                newView.Country = new CountryViewModel { Name = string.Empty };
                newView.Region = new RegionViewModel { Name = string.Empty };
                newView.City = new CityViewModel { Name = string.Empty };
                newView.Airport = new AirportViewModel { Name = string.Empty };
            }
            else if (customRegionEntryModel.cty != null)
            {
                newView.City = AutoMapperConfiguration.GetInstance<CityViewModel>(customRegionEntryModel.cty);
                newView.Country = new CountryViewModel { Name = string.Empty };
                newView.State = new StateViewModel { Name = string.Empty };
                newView.Region = new RegionViewModel { Name = string.Empty };
                newView.Airport = new AirportViewModel { Name = string.Empty };
            }
            else if (customRegionEntryModel.apt != null)
            {
                newView.Airport = AutoMapperConfiguration.GetInstance<AirportViewModel>(customRegionEntryModel.apt);
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
            newView.CustomRegions = new List<CustomRegionViewModel>();
            if (customRegionGroupViewModel.CustomRegionEntries != null)
            {
                foreach (var cre in customRegionGroupViewModel.CustomRegionEntries)
                {
                    newView.CustomRegions.Add(GetView(cre));
                }
            }
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
