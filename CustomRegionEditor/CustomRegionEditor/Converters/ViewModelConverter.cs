using CustomRegionEditor.Models;
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
            if (customRegionEntryModel.Region?.Id != null)
            {
                newView.Region = AutoMapperConfiguration.GetInstance<RegionViewModel>(customRegionEntryModel.Region);
                newView.Country = new CountryViewModel { Name = string.Empty };
                newView.State = new StateViewModel { Name = string.Empty };
                newView.City = new CityViewModel { Name = string.Empty };
                newView.Airport = new AirportViewModel { Name = string.Empty };
                newView.Value = customRegionEntryModel.Region.Id;
                newView.Name = customRegionEntryModel.Region.Name;
            }
            else if (customRegionEntryModel.Country?.Id != null)
            {
                newView.Country = AutoMapperConfiguration.GetInstance<CountryViewModel>(customRegionEntryModel.Country);
                newView.Region = new RegionViewModel { Name = string.Empty };
                newView.State = new StateViewModel { Name = string.Empty };
                newView.City = new CityViewModel { Name = string.Empty };
                newView.Airport = new AirportViewModel { Name = string.Empty };
                newView.Value = customRegionEntryModel.Country.Id;
                newView.Name = customRegionEntryModel.Country.Name;
            }
            else if (customRegionEntryModel.State?.Id != null)
            {
                newView.State = AutoMapperConfiguration.GetInstance<StateViewModel>(customRegionEntryModel.State);
                newView.Country = new CountryViewModel { Name = string.Empty };
                newView.Region = new RegionViewModel { Name = string.Empty };
                newView.City = new CityViewModel { Name = string.Empty };
                newView.Airport = new AirportViewModel { Name = string.Empty };
                newView.Value = customRegionEntryModel.State.Id;
                newView.Name = customRegionEntryModel.State.Name;
            }
            else if (customRegionEntryModel.City?.Id != null)
            {
                newView.City = AutoMapperConfiguration.GetInstance<CityViewModel>(customRegionEntryModel.City);
                newView.Country = new CountryViewModel { Name = string.Empty };
                newView.State = new StateViewModel { Name = string.Empty };
                newView.Region = new RegionViewModel { Name = string.Empty };
                newView.Airport = new AirportViewModel { Name = string.Empty };
                newView.Value = customRegionEntryModel.City.Id;
                newView.Name = customRegionEntryModel.City.Name;
            }
            else if (customRegionEntryModel.Airport?.Id != null)
            {
                newView.Airport = AutoMapperConfiguration.GetInstance<AirportViewModel>(customRegionEntryModel.Airport);
                newView.Country = new CountryViewModel { Name = string.Empty };
                newView.State = new StateViewModel { Name = string.Empty };
                newView.City = new CityViewModel { Name = string.Empty };
                newView.Region = new RegionViewModel { Name = string.Empty };
                newView.Value = customRegionEntryModel.Airport.Id;
                newView.Name = customRegionEntryModel.Airport.Name;
            }
            else
            {
                newView.Airport = new AirportViewModel { Name = string.Empty };
                newView.Country = new CountryViewModel { Name = string.Empty };
                newView.State = new StateViewModel { Name = string.Empty };
                newView.City = new CityViewModel { Name = string.Empty };
                newView.Region = new RegionViewModel { Name = string.Empty };
                newView.Value = "Error";
                newView.Name = "Error";
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

        public List<CustomRegionGroupViewModel> GetView(List<CustomRegionGroupModel> customRegionGroupViewModels)
        {
            var newList = new List<CustomRegionGroupViewModel>();
            foreach (var model in customRegionGroupViewModels)
            {
                newList.Add(GetView(model));
            }
            return newList;
        }
        
        public ErrorViewModel GetView(ErrorModel errorModel)
        {
            var newView = AutoMapperConfiguration.GetInstance<ErrorViewModel>(errorModel);
            return newView;
        }
        
        public List<AirportViewModel> GetView(List<AirportModel> oldModels)
        {
            if (oldModels == null) return null;
            var newModels = new List<AirportViewModel>();
            foreach (var model in oldModels)
            {
                var newModel = new AirportViewModel
                {
                    Id = model.Id,
                    Name = model.Name,
                };
                newModels.Add(newModel);
            }
            return newModels;
        }

        public List<CityViewModel> GetView(List<CityModel> oldModels)
        {
            if (oldModels == null) return null;
            var newModels = new List<CityViewModel>();
            foreach (var model in oldModels)
            {
                var newModel = new CityViewModel
                {
                    Id = model.Id,
                    Name = model.Name,
                };
                newModels.Add(newModel);
            }
            return newModels;
        }

        public List<StateViewModel> GetView(List<StateModel> oldModels)
        {
            if (oldModels == null) return null;
            var newModels = new List<StateViewModel>();
            foreach (var model in oldModels)
            {
                var newModel = new StateViewModel
                {
                    Id = model.Id,
                    Name = model.Name,
                };
                newModels.Add(newModel);
            }
            return newModels;
        }

        public List<CountryViewModel> GetView(List<CountryModel> oldModels)
        {
            if (oldModels == null) return null;
            var newModels = new List<CountryViewModel>();
            foreach (var model in oldModels)
            {
                var newModel = new CountryViewModel
                {
                    Id = model.Id,
                    Name = model.Name,
                };
                newModels.Add(newModel);
            }
            return newModels;
        }

        public List<RegionViewModel> GetView(List<RegionModel> oldModels)
        {
            if (oldModels == null) return null;
            var newModels = new List<RegionViewModel>();
            foreach (var model in oldModels)
            {
                var newModel = new RegionViewModel
                {
                    Id = model.Id,
                    Name = model.Name,
                };
                newModels.Add(newModel);
            }
            return newModels;
        }
    }
}
