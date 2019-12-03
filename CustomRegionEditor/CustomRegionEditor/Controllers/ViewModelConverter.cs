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

        public CustomRegionGroupViewModel GetView(CustomRegionGroupModel customRegionGroupViewModel)
        {
            return AutoMapperConfiguration.GetInstance<CustomRegionGroupViewModel>(customRegionGroupViewModel);
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
