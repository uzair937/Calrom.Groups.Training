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
        public HomeController(ICustomRegionGroupRepository customRegionGroupRepository, ICustomRegionEntryRepository customRegionEntryRepository, IViewModelConverter ViewModelConverter, ISubRegionRepo<CityModel> cityRepo, ISubRegionRepo<StateModel> stateRepo, ISubRegionRepo<CountryModel> countryRepo, ISubRegionRepo<RegionModel> regionRepo)
        {
            this.StateRepo = stateRepo;
            this.CityRepo = cityRepo;
            this.CountryRepo = countryRepo;
            this.RegionRepo = regionRepo;
            this.CustomRegionGroupRepository = customRegionGroupRepository;
            this.ViewModelConverter = ViewModelConverter;
            this.CustomRegionEntryRepository = customRegionEntryRepository;
            SetupAutoCompleteList();
        }


        private ISubRegionRepo<CityModel> CityRepo { get; }
        private ISubRegionRepo<StateModel> StateRepo { get; }
        private ISubRegionRepo<CountryModel> CountryRepo { get; }
        private ISubRegionRepo<RegionModel> RegionRepo { get; }

        private List<string> Airports = null;
        private List<string> Cities = null;
        private List<string> States = null;
        private List<string> Countries = null;
        private List<string> Regions = null;
        private readonly IViewModelConverter ViewModelConverter = null;

        private void SetupAutoCompleteList()
        {
            Airports = this.CustomRegionGroupRepository.GetNames("airport").Distinct().ToList();
            Cities = this.CustomRegionGroupRepository.GetNames("city").Distinct().ToList();
            States = this.CustomRegionGroupRepository.GetNames("state").Distinct().ToList();
            Countries = this.CustomRegionGroupRepository.GetNames("country").Distinct().ToList();
            Regions = this.CustomRegionGroupRepository.GetNames("region").Distinct().ToList();
        }

        public ICustomRegionGroupRepository CustomRegionGroupRepository { get; private set; }
        public ICustomRegionEntryRepository CustomRegionEntryRepository { get; private set; }

        [HttpPost]
        public ActionResult Search(string searchTerm, string filter)
        {
            var SearchResults = new List<CustomRegionGroupModel>();
            var contentViewModel = new ContentViewModel();
            if (filter.Contains("Filter"))
            {
                contentViewModel = new ContentViewModel
                {
                    SubRegionViewModel = new SubRegionViewModel()
                    {
                        IsViewing = true,
                        CustomRegionGroupViewModel = ViewModelConverter.GetView(GetSubRegions(searchTerm, filter))
                    },
                };
            }
            else
            {
                SearchResults = this.CustomRegionGroupRepository.GetSearchResults(searchTerm, filter);
                contentViewModel = new ContentViewModel
                {
                    SearchViewModel = new SearchViewModel()
                    {
                        IsSearching = true,
                        InvalidSearchTerm = searchTerm
                    }
                };
            }
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
            if (regionId == null || regionId == "" || regionId == "undefined")
            {
                regionId = this.CustomRegionGroupRepository.AddNewRegion(name, description).Id.ToString();
            }
            else
            {
                this.CustomRegionGroupRepository.ChangeDetails(name, description, regionId);
            }

            var FoundRegion = this.CustomRegionGroupRepository.FindById(regionId);
            FoundRegion.CustomRegionEntries = FoundRegion.CustomRegionEntries.OrderBy(a => a.Airport?.AirportId)
                                                                            .ThenBy(a => a.City?.CityName)
                                                                            .ThenBy(a => a.State?.StateName)
                                                                            .ThenBy(a => a.Country?.CountryName)
                                                                            .ThenBy(a => a.Region?.RegionName).ToList();
            var contentViewModel = new ContentViewModel
            {
                EditViewModel = new EditViewModel()
                {
                    IsEditing = true,
                    ExistingRegion = true,
                    CustomRegionGroupViewModel = ViewModelConverter.GetView(FoundRegion)
                },
            };
            return PartialView("_Content", contentViewModel);
        }

        [HttpPost]
        public ActionResult NewCustomRegion()
        {
            var customRegionGroupViewModel = new CustomRegionGroupViewModel
            {
                Name = "",
                Description = "",
                CustomRegions = new List<CustomRegionEntryViewModel>()
            };
            var contentViewModel = new ContentViewModel
            {
                EditViewModel = new EditViewModel()
                {
                    IsEditing = true,
                    CustomRegionGroupViewModel = customRegionGroupViewModel
                },
            };
            return PartialView("_Content", contentViewModel);
        }

        [HttpPost]
        public ActionResult EditRegionGroup(string regionId)
        {
            var contentViewModel = new ContentViewModel
            {
                SearchViewModel = new SearchViewModel()
                {
                    IsSearching = true,
                }
            };
            if (regionId == "" || regionId == null)
            {
                return PartialView("_Content", contentViewModel);
            }

            var FoundRegion = this.CustomRegionGroupRepository.FindById(regionId);
            FoundRegion.CustomRegionEntries = FoundRegion.CustomRegionEntries.OrderBy(a => a.Airport?.AirportId)
                                                                            .ThenBy(a => a.City?.CityName)
                                                                            .ThenBy(a => a.State?.StateName)
                                                                            .ThenBy(a => a.Country?.CountryName)
                                                                            .ThenBy(a => a.Region?.RegionName).ToList();
            contentViewModel = new ContentViewModel
            {
                EditViewModel = new EditViewModel()
                {
                    IsEditing = true,
                    ExistingRegion = true,
                    CustomRegionGroupViewModel = ViewModelConverter.GetView(FoundRegion)
                },
            };
            return PartialView("_Content", contentViewModel);
        }

        [HttpPost]
        public ActionResult AutoComplete(string type, string text)
        {
            var autoCompleteViewModel = new AutoCompleteViewModel();
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

        public CustomRegionGroupModel GetSubRegions(string searchTerm, string filter)
        {
            var customRegionGroupModel = new CustomRegionGroupModel()
            {
                CustomRegionEntries = new List<CustomRegionEntryModel>()
            };
            switch (filter)
            {
                case "regionFilter":
                    var region = RegionRepo.FindByName(searchTerm);
                    customRegionGroupModel.CustomRegionEntries = RegionRepo.GetSubRegions(region);
                    break;

                case "countryFilter":
                    var country = CountryRepo.FindByName(searchTerm);
                    customRegionGroupModel.CustomRegionEntries = CountryRepo.GetSubRegions(country);
                    break;

                case "stateFilter":
                    var state = StateRepo.FindByName(searchTerm);
                    customRegionGroupModel.CustomRegionEntries = StateRepo.GetSubRegions(state);
                    break;

                case "cityFilter":
                    var city = CityRepo.FindByName(searchTerm);
                    customRegionGroupModel.CustomRegionEntries = CityRepo.GetSubRegions(city);
                    break;

                default:
                    break;
            }
            return customRegionGroupModel;
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

        public ActionResult Index()
        {
            var layoutViewModel = new LayoutViewModel();

            return View(layoutViewModel);
        }
    }
}