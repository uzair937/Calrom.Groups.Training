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
using CustomRegionEditor.Database.Repositories;

namespace CustomRegionEditor.Controllers
{
    public class HomeController : Controller
    {
        public HomeController(ICustomRegionGroupRepository customRegionGroupRepository, ICustomRegionEntryRepository customRegionEntryRepository, IViewModelConverter ViewModelConverter, ISubRegionRepo<CityModel> cityRepo, ISubRegionRepo<StateModel> stateRepo, ISubRegionRepo<CountryModel> countryRepo, ISubRegionRepo<RegionModel> regionRepo, ICustomRegionGroupTempRepo customRegionGroupTempRepo)
        {
            this.StateRepo = stateRepo;
            this.CityRepo = cityRepo;
            this.CountryRepo = countryRepo;
            this.RegionRepo = regionRepo;
            this.CustomRegionGroupRepository = customRegionGroupRepository;
            this.ViewModelConverter = ViewModelConverter;
            this.CustomRegionEntryRepository = customRegionEntryRepository;
            this.CustomRegionGroupTempRepo = customRegionGroupTempRepo;
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
        public ICustomRegionGroupTempRepo CustomRegionGroupTempRepo { get; private set; }

        [HttpPost]
        public ActionResult Search(string searchTerm, string filter)
        {
            var SearchResults = new List<CustomRegionGroupModel>();
            bool ValidSearch = false;
            var contentViewModel = new ContentViewModel();
            if (filter.Contains("Filter"))
            {
                var SubSearchResults = ViewModelConverter.GetView(GetSubRegions(searchTerm, filter));
                contentViewModel = new ContentViewModel
                {
                    SubRegionViewModel = new SubRegionViewModel()
                    {
                        IsViewing = true,
                        CustomRegionGroupViewModel = SubSearchResults,
                        InvalidSearchTerm = searchTerm
                    },
                };
                if (SubSearchResults.CustomRegions.Count > 0) ValidSearch = true;
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
                if (SearchResults.Count > 0) ValidSearch = true;
            }
            if (ValidSearch)
            {
                contentViewModel.SubRegionViewModel.ValidResults = true;
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
        public ActionResult AddRegion(AddRegionViewModel addRegionViewModel)
        {
            var updatedCustomRegionGroupModel = this.CustomRegionGroupRepository.AddByType(addRegionViewModel.Entry, addRegionViewModel.Type, addRegionViewModel.RegionId);

            var editViewModel = new EditViewModel()
            {
                CustomRegionGroupViewModel = ViewModelConverter.GetView(updatedCustomRegionGroupModel),
                IsEditing = true,
                ExistingRegion = true
            };

            return PartialView("_EditRegion", editViewModel);
        }

        [HttpPost]
        public ActionResult SaveChanges(CustomRegionGroupViewModel customRegionGroupViewModel)
        {
            if (string.IsNullOrEmpty(customRegionGroupViewModel.ID))
            {
                customRegionGroupViewModel.ID = this.CustomRegionGroupRepository.AddNewRegion(customRegionGroupViewModel.Name, customRegionGroupViewModel.Description).Id.ToString();
            }
            else
            {
                this.CustomRegionGroupRepository.ChangeDetails(customRegionGroupViewModel.Name, customRegionGroupViewModel.Description, customRegionGroupViewModel.ID);
            }

            var foundRegion = this.CustomRegionGroupTempRepo.List().FirstOrDefault(a => a.Id == Guid.Parse(customRegionGroupViewModel.ID));
            if (foundRegion.CustomRegionEntries != null)
            {
                foundRegion.CustomRegionEntries = foundRegion.CustomRegionEntries.OrderBy(a => a.Airport?.AirportId)
                                                                                            .ThenBy(a => a.City?.CityName)
                                                                                            .ThenBy(a => a.State?.StateName)
                                                                                            .ThenBy(a => a.Country?.CountryName)
                                                                                            .ThenBy(a => a.Region?.RegionName).ToList();
            }
            else
            {
                foundRegion = new CustomRegionGroupModel()
                {
                    Name = customRegionGroupViewModel.Name,
                    Description = customRegionGroupViewModel.Description,
                    CustomRegionEntries = new List<CustomRegionEntryModel>()
                };
            }
            var contentViewModel = new ContentViewModel
            {
                EditViewModel = new EditViewModel()
                {
                    IsEditing = true,
                    ExistingRegion = true,
                    CustomRegionGroupViewModel = ViewModelConverter.GetView(foundRegion)
                },
            };
            this.CustomRegionGroupRepository.UpdateList(foundRegion.CustomRegionEntries, customRegionGroupViewModel.ID);
            var endRegion = this.CustomRegionGroupTempRepo.List().FirstOrDefault(b => b.Id == Guid.Parse(customRegionGroupViewModel.ID));
            endRegion.Id = Guid.Empty;
            this.CustomRegionGroupRepository.AddOrUpdate(endRegion);
            this.CustomRegionGroupTempRepo.DestroySession();
            return PartialView("_Content", contentViewModel);
        }

        [HttpPost]
        public ActionResult EditRegion()
        {
            var customRegionGroupViewModel = new CustomRegionGroupViewModel
            {
                ID = Guid.NewGuid().ToString(),
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
            var customRegion = new CustomRegionGroupModel
            {
                Id = Guid.Parse(customRegionGroupViewModel.ID),
                Name = customRegionGroupViewModel.Name,
                Description = customRegionGroupViewModel.Description,
                CustomRegionEntries = new List<CustomRegionEntryModel>()
            };
            this.CustomRegionGroupTempRepo.Add(customRegion);
            return PartialView("_Content", contentViewModel);
        }

        [HttpPost]
        public ActionResult EditRegionGroup(AddRegionViewModel addRegionViewModel)
        {
            var contentViewModel = new ContentViewModel
            {
                SearchViewModel = new SearchViewModel()
                {
                    IsSearching = true,
                }
            };

            var foundRegion = this.CustomRegionGroupTempRepo.List().FirstOrDefault(a => a.Id == Guid.Parse(addRegionViewModel.RegionId));
            if (foundRegion == null)
            {
                foundRegion = this.CustomRegionGroupRepository.FindById(addRegionViewModel.RegionId);
            }
            foundRegion.CustomRegionEntries = foundRegion.CustomRegionEntries.OrderBy(a => a.Airport?.AirportId)
                .ThenBy(a => a.City?.CityName)
                .ThenBy(a => a.State?.StateName)
                .ThenBy(a => a.Country?.CountryName)
                .ThenBy(a => a.Region?.RegionName).ToList();
            this.CustomRegionGroupRepository.UpdateList(foundRegion.CustomRegionEntries, addRegionViewModel.RegionId);
            contentViewModel = new ContentViewModel
            {
                EditViewModel = new EditViewModel()
                {
                    IsEditing = true,
                    ExistingRegion = true,
                    CustomRegionGroupViewModel = ViewModelConverter.GetView(foundRegion)
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
            return Countries.Where(c => c.Contains(term)).ToList();
        }

        public List<string> GetRegions(string term)
        {
            return Regions.Where(c => c.Contains(term)).ToList();
        }

        public List<string> GetStates(string term)
        {
            return States.Where(c => c.Contains(term)).ToList();
        }

        public List<string> GetCities(string term)
        {
            return Cities.Where(c => c.Contains(term)).ToList();
        }

        public List<string> GetAirports(string term)
        {
            return Airports.Where(c => c.Contains(term)).ToList();
        }

        public ActionResult Index()
        {
            var layoutViewModel = new LayoutViewModel();

            return View(layoutViewModel);
        }
    }
}