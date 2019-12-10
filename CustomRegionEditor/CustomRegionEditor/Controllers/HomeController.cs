using CustomRegionEditor.Database;
using CustomRegionEditor.Database.Interfaces;
using CustomRegionEditor.Database.Models;
using CustomRegionEditor.EntityMapper;
using CustomRegionEditor.ViewModels;
using CustomRegionEditor.Web.Converters;
using CustomRegionEditor.Web.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace CustomRegionEditor.Controllers
{
    public class HomeController : Controller
    {
        public HomeController(ICustomRegionGroupRepository customRegionGroupRepository, ICustomRegionEntryRepository customRegionEntryRepository, IViewModelConverter iViewModelConverter)
        {
            this.CustomRegionGroupRepository = customRegionGroupRepository;
            this.IViewModelConverter = iViewModelConverter;
            this.CustomRegionEntryRepository = customRegionEntryRepository;
        }

        private LayoutViewModel SetupLayoutModel()
        {
            var layoutViewModel = new LayoutViewModel
            {
                ContentViewModel = new ContentViewModel(),
                SidebarViewModel = new SidebarViewModel()
            };
            layoutViewModel.ContentViewModel.EditViewModel = new EditViewModel() { IsEditing = false };
            layoutViewModel.ContentViewModel.SearchViewModel = new SearchViewModel() { IsSearching = false };

            return layoutViewModel;
        }

        private void SetupAutoCompleteList()
        {
            Airports = this.CustomRegionGroupRepository.GetNames("airport").Distinct().ToList();
            Cities = this.CustomRegionGroupRepository.GetNames("city").Distinct().ToList();
            States = this.CustomRegionGroupRepository.GetNames("state").Distinct().ToList();
            Countries = this.CustomRegionGroupRepository.GetNames("country").Distinct().ToList();
            Regions = this.CustomRegionGroupRepository.GetNames("region").Distinct().ToList();
        }

        private static List<string> Airports = null;
        private static List<string> Cities = null;
        private static List<string> States = null;
        private static List<string> Countries = null;
        private static List<string> Regions = null;
        private readonly IViewModelConverter IViewModelConverter = null;

        public ICustomRegionGroupRepository CustomRegionGroupRepository { get; private set; }
        public ICustomRegionEntryRepository CustomRegionEntryRepository { get; private set; }

        public ActionResult Index()
        {
            var layoutViewModel = SetupLayoutModel();
            SetupAutoCompleteList();
            return View(layoutViewModel);
        }

        [HttpPost]
        public ActionResult Search(string searchTerm, string filter)
        {
            var SearchResults = new List<CustomRegionGroupModel>();
            var contentViewModel = new ContentViewModel
            {
                EditViewModel = new EditViewModel() { IsEditing = false },
                SearchViewModel = new SearchViewModel() { IsSearching = true, ValidResults = false }
            };
            if (filter.Contains("Filter"))
            {
                contentViewModel = new ContentViewModel
                {
                    //EditViewModel = new EditViewModel() { IsEditing = true, CustomRegionGroupViewModel = this.CustomRegionGroupRepository.GetFilteredResults(searchTerm, filter) },
                    SearchViewModel = new SearchViewModel() { IsSearching = false }
                };
            }
            else
            {
                SearchResults = this.CustomRegionGroupRepository.GetSearchResults(searchTerm, filter);
                contentViewModel = new ContentViewModel
                {
                    EditViewModel = new EditViewModel() { IsEditing = false },
                    SearchViewModel = new SearchViewModel() { IsSearching = true, ValidResults = false }
                };
            }
            if (SearchResults.Count > 0)
            {
                contentViewModel.SearchViewModel.ValidResults = true;
                contentViewModel.SearchViewModel.SearchResults = IViewModelConverter.GetView(SearchResults);
            }

            return PartialView("_Content", contentViewModel);
        }

        [HttpPost]
        public ActionResult DeleteRegionGroup(string regionId)
        {
            this.CustomRegionGroupRepository.DeleteById(regionId);
            return null;
        }

        [HttpPost]
        public ActionResult DeleteEntry(string entryId)
        {
            this.CustomRegionEntryRepository.DeleteById(entryId);
            return null;
        }

        [HttpPost]
        public ActionResult AddRegion(string entry, string type, string regionId)
        {
            this.CustomRegionGroupRepository.AddByType(entry, type, regionId);
            return null;
        }

        [HttpPost]
        public ActionResult SaveChanges(string name, string description, string regionId)
        {
            this.CustomRegionGroupRepository.ChangeDetails(name, description, regionId);
            return null;
        }

        [HttpPost]
        public ActionResult NewCustomRegion()
        {
            var newRegion = this.CustomRegionGroupRepository.AddNewRegion();
            var contentViewModel = new ContentViewModel
            {
                EditViewModel = new EditViewModel() { IsEditing = true },
                SearchViewModel = new SearchViewModel() { IsSearching = false }
            };
            contentViewModel.EditViewModel.CustomRegionGroupViewModel = IViewModelConverter.GetView(newRegion);
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
            var FoundRegion = this.CustomRegionGroupRepository.FindById(regionId);
            FoundRegion.CustomRegionEntries = FoundRegion.CustomRegionEntries.OrderBy(a => a.Airport?.AirportId).ThenBy(a => a.City?.CityName).ThenBy(a => a.State?.StateName).ThenBy(a => a.Country?.CountryName).ThenBy(a => a.Region?.RegionName).ToList();
            contentViewModel.EditViewModel.CustomRegionGroupViewModel = IViewModelConverter.GetView(FoundRegion);

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

        public CustomRegionGroupModel GetFilteredResults(string countryName, string filter)
        {

            var crg = new CustomRegionGroupModel()
            {
                CustomRegionEntries = new List<CustomRegionEntryModel>()
            };
            switch (filter)
            {
                case "countryFilter":

                    crg.CustomRegionEntries = CustomRegionGroupRepository.GetCountrySubRegions(CustomRegionGroupRepository.GetCountry(countryName));
                    break;

                case "cityFilter":
                    crg.CustomRegionEntries = GetCitySubRegions(GetCity(countryName));

                    break;

                default:
                    break;
            }

            return crg;
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