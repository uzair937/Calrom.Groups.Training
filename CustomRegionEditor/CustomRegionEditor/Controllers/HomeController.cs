using CustomRegionEditor.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CustomRegionEditor.Controllers
{
    public class HomeController : Controller
    {
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