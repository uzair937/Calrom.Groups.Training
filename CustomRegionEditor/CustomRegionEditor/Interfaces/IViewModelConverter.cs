using CustomRegionEditor.Database;
using CustomRegionEditor.Database.Models;
using CustomRegionEditor.EntityMapper;
using CustomRegionEditor.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomRegionEditor.Web.Interfaces
{
    public interface IViewModelConverter
    {
        CustomRegionEntryViewModel GetView(CustomRegionEntryModel customRegionEntryModel);

        CustomRegionGroupViewModel GetView(CustomRegionGroupModel customRegionGroupViewModel);

        List<CustomRegionGroupViewModel> GetView(List<CustomRegionGroupModel> customRegionGroupViewModels);
    }
}
