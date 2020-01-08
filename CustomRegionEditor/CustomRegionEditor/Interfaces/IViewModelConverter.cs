using CustomRegionEditor.Models;
using CustomRegionEditor.ViewModels;
using System.Collections.Generic;

namespace CustomRegionEditor.Web.Interfaces
{
    public interface IViewModelConverter
    {
        CustomRegionEntryViewModel GetView(CustomRegionEntryModel customRegionEntryModel);

        CustomRegionGroupViewModel GetView(CustomRegionGroupModel customRegionGroupViewModel);

        List<CustomRegionGroupViewModel> GetView(List<CustomRegionGroupModel> customRegionGroupViewModels);

        ErrorViewModel GetView(ErrorModel customRegionGroupViewModels);
    }
}
