using CustomRegionEditor.Database;
using CustomRegionEditor.Database.Models;
using CustomRegionEditor.EntityMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomRegionEditor.ViewModels
{
    public class ViewModelConverter
    {
        private static ViewModelConverter viewModelConverter = null;

        public static ViewModelConverter GetInstance
        {
            get
            {
                if (viewModelConverter == null)
                {
                    viewModelConverter = new ViewModelConverter();
                }
                return viewModelConverter;
            }
        }

        public CustomRegionViewModel GetView(CustomRegionEntryModel customRegionEntryModel)
        {
            var newView = AutoMapperConfiguration.GetInstance<CustomRegionViewModel>(customRegionEntryModel);
            if (customRegionEntryModel.reg != null)
            {
                newView.Region = AutoMapperConfiguration.GetInstance<RegionViewModel>(customRegionEntryModel.reg);
            }
            else if (customRegionEntryModel.cnt != null)
            {
                newView.Country = AutoMapperConfiguration.GetInstance<CountryViewModel>(customRegionEntryModel.cnt);
            }
            else if (customRegionEntryModel.sta != null)
            {
                newView.State = AutoMapperConfiguration.GetInstance<StateViewModel>(customRegionEntryModel.sta);
            }
            else if (customRegionEntryModel.cty != null)
            {
                newView.City = AutoMapperConfiguration.GetInstance<CityViewModel>(customRegionEntryModel.cty);
            }
            else if (customRegionEntryModel.apt != null)
            {
                newView.Airport = AutoMapperConfiguration.GetInstance<AirportViewModel>(customRegionEntryModel.apt);
            }
            return newView;
        }

        public CustomRegionGroupViewModel GetView(CustomRegionGroupModel customRegionGroupViewModel)
        {
            var newView = AutoMapperConfiguration.GetInstance<CustomRegionGroupViewModel>(customRegionGroupViewModel);
            foreach (var cre in customRegionGroupViewModel.CustomRegionEntries)
            {
                newView.CustomRegions.Add(GetView(cre));
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
