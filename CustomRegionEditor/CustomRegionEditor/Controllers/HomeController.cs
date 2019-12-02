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
            layoutViewModel.ContentViewModel.EditViewModel = new EditViewModel() { IsEditing = false } ;
            layoutViewModel.ContentViewModel.SearchViewModel = new SearchViewModel() { IsSearching = false };

            return layoutViewModel;
        }

        private static ViewModelConverter ViewModelConverter { get { return ViewModelConverter.GetInstance; } }
        private static CustomRegionRepo CustomRegionRepo { get { return CustomRegionRepo.GetInstance; } }

        public ActionResult Index()
        {
            var layoutViewModel = SetupLayoutModel();
            return View(layoutViewModel);
        }

        [HttpPost]
        public ActionResult Search(string searchTerm)
        {
            var contentViewModel = new ContentViewModel
            {
                EditViewModel = new EditViewModel() { IsEditing = false },
                SearchViewModel = new SearchViewModel() { IsSearching = true }
            };
            var SearchResults = CustomRegionRepo.GetSearchResults(searchTerm);
            contentViewModel.SearchViewModel.SearchResults = ViewModelConverter.GetView(SearchResults);

            return PartialView("_Content", contentViewModel);
        }

        [HttpPost]
        public ActionResult DeleteRegion(string regionId)
        {
            CustomRegionRepo.DeleteById(regionId);
            return null;
        }

        [HttpPost]
        public ActionResult DeleteEntry(string entryId, string regionId)
        {
            CustomRegionRepo.DeleteEntry(entryId, regionId);
            return null;
        }

        [HttpPost]
        public ActionResult AddRegion(string entry, string type, string regionId)
        {
            CustomRegionRepo.AddByType(entry, type, regionId);
            return null;
        }

        [HttpPost]
        public ActionResult EditRegionGroup(string regionId)
        {
            var contentViewModel = new ContentViewModel
            {
                EditViewModel = new EditViewModel() { IsEditing = true },
                SearchViewModel = new SearchViewModel() { IsSearching = false }
            };
            var FoundRegion = CustomRegionRepo.FindById(regionId);
            contentViewModel.EditViewModel.CustomRegionGroupViewModel = ViewModelConverter.GetView(FoundRegion);

            return PartialView("_Content", contentViewModel);
        }
    }
}