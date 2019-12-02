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
            var repo = CustomRegionRepo.GetRegionRepo();
            SearchViewModel searchViewModel = new SearchViewModel
            {
                SearchTerm = searchTerm
            };
            CustomRegionViewModel customRegionViewModel = repo.GetSearchResults(searchTerm);
            searchViewModel.CustomRegionList.Add(customRegionViewModel);

            return PartialView("_SearchRegion", searchViewModel);
        }
    }
}