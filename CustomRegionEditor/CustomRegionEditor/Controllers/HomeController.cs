using CustomRegionEditor.Database;
using CustomRegionEditor.EntityMapper;
using CustomRegionEditor.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
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

        private void SetupAutoCompleteList()
        {
            Airports = CustomRegionRepo.GetNames("airport").Distinct().ToList();
            Cities = CustomRegionRepo.GetNames("city").Distinct().ToList();
            States = CustomRegionRepo.GetNames("state").Distinct().ToList();
            Countries = CustomRegionRepo.GetNames("country").Distinct().ToList();
            Regions = CustomRegionRepo.GetNames("region").Distinct().ToList();

        }

        private static List<string> Airports = null;
        private static List<string> Cities = null;
        private static List<string> States = null;
        private static List<string> Countries = null;
        private static List<string> Regions = null;
        private static ViewModelConverter ViewModelConverter { get { return ViewModelConverter.GetInstance; } }
        private static CustomRegionGroupRepo CustomRegionRepo { get { return CustomRegionGroupRepo.GetInstance; } }

        public ActionResult Index()
        {
            var layoutViewModel = SetupLayoutModel();
            SetupAutoCompleteList();
            return View(layoutViewModel);
        }

        [HttpPost]
        public ActionResult Search(string searchTerm)
        {
            var contentViewModel = new ContentViewModel
            {
                EditViewModel = new EditViewModel() { IsEditing = false },
                SearchViewModel = new SearchViewModel() { IsSearching = true, ValidResults = false }
            };
            var SearchResults = CustomRegionRepo.GetSearchResults(searchTerm);
            if (SearchResults.Count > 0)
            {
                contentViewModel.SearchViewModel.ValidResults = true;
                contentViewModel.SearchViewModel.SearchResults = ViewModelConverter.GetView(SearchResults);
            }

            return PartialView("_Content", contentViewModel);
        }

        [HttpPost]
        public ActionResult DeleteRegionGroup(string regionId)
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
        public ActionResult SaveChanges(string name, string description, string regionId)
        {
            CustomRegionRepo.ChangeDetails(name, description, regionId);
            return null;
        }
        
        [HttpPost]
        public ActionResult NewCustomRegion()
        {
            var newRegion = CustomRegionRepo.AddNewRegion();
            var contentViewModel = new ContentViewModel
            {
                EditViewModel = new EditViewModel() { IsEditing = true },
                SearchViewModel = new SearchViewModel() { IsSearching = false }
            };
            contentViewModel.EditViewModel.CustomRegionGroupViewModel = ViewModelConverter.GetView(newRegion);
            contentViewModel.EditViewModel.CustomRegionGroupViewModel.CustomRegions = new List<CustomRegionViewModel>();

            return PartialView("_Content", contentViewModel);
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
            FoundRegion.CustomRegionEntries = FoundRegion.CustomRegionEntries.OrderBy(a => a.apt?.apt_id).ThenBy(a => a.cty?.city_name).ThenBy(a => a.sta?.state_name).ThenBy(a => a.cnt?.country_name).ThenBy(a => a.reg?.region_name).ToList();
            contentViewModel.EditViewModel.CustomRegionGroupViewModel = ViewModelConverter.GetView(FoundRegion);

            return PartialView("_Content", contentViewModel);
        }
        
        [HttpPost]
        public ActionResult AutoComplete(string type, string text)
        {
            var autoCompleteViewModel = new AutoCompleteViewModel();
            autoCompleteViewModel.Suggestions = new List<string>();
            text = text.ToUpper();
            if (text != "" && text != null && text != " ")
            {
                switch (type)
                {
                    case "region":
                        autoCompleteViewModel.Suggestions = GetRegions(text);
                        break;
                    case "country":
                        autoCompleteViewModel.Suggestions = GetCountries(text);
                        break;
                    case "state":
                        autoCompleteViewModel.Suggestions = GetStates(text);
                        break;
                    case "city":
                        autoCompleteViewModel.Suggestions = GetCities(text);
                        break;
                    case "airport":
                        autoCompleteViewModel.Suggestions = GetAirports(text);
                        break;
                }
            }
            return PartialView("_AutoComplete", autoCompleteViewModel);
        }

        public List<string> GetCountries(string term)
        {
            return Countries.Where(c => c.StartsWith(term)).ToList();
        }

        public List<string> GetRegions(string term)
        {
            return Regions.Where(c => c.StartsWith(term)).ToList();
        }

        public List<string> GetStates(string term)
        {
            return States.Where(c => c.StartsWith(term)).ToList();
        }

        public List<string> GetCities(string term)
        {
            return Cities.Where(c => c.StartsWith(term)).ToList();
        }

        public List<string> GetAirports(string term)
        {
            return Airports.Where(c => c.StartsWith(term)).ToList();
        }
    }
}