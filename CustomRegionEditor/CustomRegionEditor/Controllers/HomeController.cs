using CustomRegionEditor.Database;
using CustomRegionEditor.EntityMapper;
using CustomRegionEditor.ViewModels;
using System.Collections.Generic;
using System.Web.Mvc;

namespace CustomRegionEditor.Controllers
{
    public class HomeController : Controller
    {
        public static CustomRegionRepo CustomRegionRepo { get { return CustomRegionRepo.GetInstance; } }

        public ActionResult ViewRegions()
        {
            var customRegionViewModel = new CustomRegionViewModel()
            {

            };
            var customRegionList = CustomRegionRepo.List();
            foreach (var cr in customRegionList)
            {
                var newRegion = AutoMapperConfiguration.GetInstance<CustomRegionViewModel>(cr);
            }
            return View(customRegionViewModel);

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