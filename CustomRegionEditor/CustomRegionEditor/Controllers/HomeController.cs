using CustomRegionEditor.Database;
using CustomRegionEditor.Models;
using System.Web.Mvc;

namespace CustomRegionEditor.Controllers
{
    public class HomeController : Controller
    {
        public static CustomRegionRepo CustomInstance { get { return CustomRegionRepo.GetInstance; } }

        public ActionResult ViewRegions()
        {
            var customRegionViewModel = new CustomRegionViewModel()
            {

            };
            var customRegionList = CustomInstance.List();
            foreach (var cr in customRegionList)
            {
                var newRegion = AutoMapperConfiguration.GetInstance<CustomRegionViewModel>(cr);

            }
            return View(customRegionViewModel);

        }
        public ActionResult Search(string searchTerm)
        {
            CustomRegionViewModel customRegionViewModel = new CustomRegionViewModel();
            SearchViewModel searchViewModel = new SearchViewModel();
            searchViewModel.CustomRegionList.Add(customRegionViewModel);

            return PartialView("_SearchResults", searchViewModel);
        }
    }
}