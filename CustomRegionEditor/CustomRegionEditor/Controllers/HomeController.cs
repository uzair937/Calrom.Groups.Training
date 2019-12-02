using CustomRegionEditor.Database;
using CustomRegionEditor.EntityMapper;
using CustomRegionEditor.Models;
using System.Web.Mvc;

namespace CustomRegionEditor.Controllers
{
    public class HomeController : Controller
    {
        public static CustomRegionRepo CustomRegionRepoInstance { get { return CustomRegionRepo.GetInstance; } }

        public ActionResult ViewRegions()
        {
            var customRegionViewModel = new CustomRegionViewModel()
            {

            };
            var customRegionList = CustomRegionRepoInstance.List();
            foreach (var cr in customRegionList)
            {
                var newRegion = AutoMapperConfiguration.GetInstance<CustomRegionViewModel>(cr);

            }
            return View(customRegionViewModel);

        }
        public ActionResult Search(string searchTerm)
        {
            
            SearchViewModel searchViewModel = new SearchViewModel
            {
                SearchTerm = searchTerm
            };

            return PartialView("_SearchRegion", searchViewModel);
        }
    }
}