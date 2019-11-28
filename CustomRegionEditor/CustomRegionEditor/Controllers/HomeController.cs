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
            CustomRegionViewModel customRegionViewModel = new CustomRegionViewModel();
            SearchViewModel searchViewModel = new SearchViewModel();
            searchViewModel.CustomRegionList.Add(customRegionViewModel);

            return PartialView("_SearchResults", searchViewModel);
        }
    }
}