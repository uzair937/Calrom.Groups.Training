using CustomRegionEditor.Database;
using CustomRegionEditor.EntityMapper;
using CustomRegionEditor.ViewModels;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace CustomRegionEditor.Controllers
{
    public class HomeController : Controller
    {
        private LayoutViewModel SetupLayoutModel()
        {
            var layoutViewModel = new LayoutViewModel
            {
                ContentViewModel = new ContentViewModel(),
                SidebarViewModel = new SidebarViewModel()
            };
            layoutViewModel.ContentViewModel.IsEditing = false;
            layoutViewModel.ContentViewModel.IsSearching = false;

            return layoutViewModel;
        }
        public static CustomRegionRepo CustomRegionRepo { get { return CustomRegionRepo.GetInstance; } }

        public ActionResult Index()
        {
            var layoutViewModel = SetupLayoutModel();
            return View(layoutViewModel);
        }

        public ActionResult Search(string searchTerm)
        {
            var searchModelList = CustomRegionRepo.GetSearchResults(searchTerm);
            var newList = new List<CustomRegionViewModel>();
            //newList = AutoMapperConfiguration.GetInstance<CustomRegionViewModel>(searchModelList);
            SearchViewModel searchViewModel = new SearchViewModel
            {
                SearchTerm = searchTerm,
                CustomRegionList = newList
            };

            return PartialView("_SearchResults", searchViewModel);
        }

        public ActionResult DeleteRegion(string regionId)
        {
            var customList = new List<CustomRegionViewModel>();
            SearchViewModel searchViewModel = new SearchViewModel
            {
                CustomRegionList = customList
            };

            return PartialView("_SearchResults", searchViewModel);
        }

        public ActionResult EditRegion(string regionId)
        {
            var contentViewModel = new ContentViewModel
            {
                EditViewModel = new EditViewModel(),
                IsEditing = true,
                IsSearching = false
            };
            var FoundRegion = CustomRegionRepo.FindById(regionId);
            contentViewModel.EditViewModel.CustomRegionViewModel = 

            return PartialView("_SearchResults", contentViewModel);
        }
    }
}