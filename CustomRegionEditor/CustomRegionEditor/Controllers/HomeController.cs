using CustomRegionEditor.Database;
using CustomRegionEditor.EntityMapper;
using CustomRegionEditor.ViewModels;
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
            var customList = new List<CustomRegionViewModel>();
            SearchViewModel searchViewModel = new SearchViewModel
            {
                SearchTerm = searchTerm,
                CustomRegionList = customList
            };

            return PartialView("_SearchResults", searchViewModel);
        }
    }
}