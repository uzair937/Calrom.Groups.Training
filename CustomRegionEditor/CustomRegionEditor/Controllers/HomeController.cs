using CustomRegionEditor.Database.Repositories;
using CustomRegionEditor.Database.Interfaces;
using CustomRegionEditor.Handler.Interfaces;
using CustomRegionEditor.Models;
using CustomRegionEditor.ViewModels;
using CustomRegionEditor.Web.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace CustomRegionEditor.Controllers
{
    public class HomeController : Controller
    {
        public HomeController(ISessionRegionGroupRepository sessionRegionGroupRepository, IViewModelConverter viewModelConverter, IRegionHandler regionHandler, ISearchRegion searchRegion, ISearchEntry searchEntry)
        {
            this.SessionRegionGroupRepository = sessionRegionGroupRepository;
            this.ViewModelConverter = viewModelConverter;
            this.RegionHandler = regionHandler;
            this.SearchRegion = searchRegion;
            this.SearchEntry = searchEntry;
            SetupAutoCompleteList();
        }

        private void SetupAutoCompleteList()
        {
            var regionLists = this.RegionHandler.GetRegionList();
            Airports = regionLists[0];
            Cities = regionLists[1];
            States = regionLists[2];
            Countries = regionLists[3];
            Regions = regionLists[4];
        }

        private List<string> Airports = null;
        private List<string> Cities = null;
        private List<string> States = null;
        private List<string> Countries = null;
        private List<string> Regions = null;
        private readonly IViewModelConverter ViewModelConverter = null;

        public ISessionRegionGroupRepository SessionRegionGroupRepository { get; private set; }
        public IRegionHandler RegionHandler { get; private set; }
        public ISearchRegion SearchRegion { get; private set; }
        public ISearchEntry SearchEntry { get; private set; }

        [HttpPost]
        public ActionResult Search(SearchBoxViewModel searchForm)
        {
            this.SessionRegionGroupRepository.ClearSession();
            var searchTerm = searchForm.text;
            var filter = searchForm.filter;
            var SearchResults = new List<CustomRegionGroupModel>();
            bool ValidSearch = false;
            var contentViewModel = new ContentViewModel();
            if (filter.Contains("Filter"))
            {
                var regionList = this.RegionHandler.GetSubRegions(searchTerm, filter);
                var SubSearchResults = this.ViewModelConverter.GetView(regionList);
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
                SearchResults = this.SearchRegion.GetSearchResults(searchTerm, filter);
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
        public ActionResult DeleteRegionGroup(IdViewModel idForm)
        {
            this.RegionHandler.DeleteById(idForm.Id);
            return Search(idForm.LastSearch);
        }

        [HttpPost]
        public ActionResult DeleteEntry(DeleteViewModel deleteForm)
        {
            var id = deleteForm.EntryId;
            var currentRegion = this.SessionRegionGroupRepository.GetSessionRegion();
            var FoundEntry = new CustomRegionEntryModel();


            FoundEntry = currentRegion.CustomRegionEntries.FirstOrDefault(a => a.Airport?.Id == id
                                                                             || a.City?.Id == id
                                                                             || a.State?.Id == id
                                                                             || a.Country?.Id == id
                                                                             || a.Region?.Id == id);
            currentRegion.CustomRegionEntries.Remove(FoundEntry);

            return UpdateRegionGroup();
        }

        [HttpPost]
        public ActionResult AddRegion(RegionFormViewModel regionForm)
        {
            var entry = regionForm.Entry;
            var type = regionForm.Type;
            this.SessionRegionGroupRepository.AddByType(entry, type);
            return UpdateRegionGroup();
        }

        [HttpPost]
        public ActionResult SaveChanges(SaveFormViewModel saveForm)
        {
            var name = saveForm.Name;
            var description = saveForm.Description;
            var regionId = saveForm.Id;
            var parseId = new Guid();
            Guid.TryParse(regionId, out parseId);
            var FoundRegion = new CustomRegionGroupModel();
            if (!(string.IsNullOrEmpty(name) || name == "Enter a valid name"))
            {
                FoundRegion = this.SessionRegionGroupRepository.GetSessionRegion();
                if (parseId != Guid.Empty)
                {
                    FoundRegion.Id = parseId;
                }
                FoundRegion.Name = name;
                FoundRegion.Description = description;
                FoundRegion = this.SessionRegionGroupRepository.SaveToDatabase(FoundRegion);
                FoundRegion.CustomRegionEntries = FoundRegion.CustomRegionEntries.OrderBy(a => a.Airport?.Id)
                                                                                .ThenBy(a => a.City?.Name)
                                                                                .ThenBy(a => a.State?.Name)
                                                                                .ThenBy(a => a.Country?.Name)
                                                                                .ThenBy(a => a.Region?.Name).ToList();
            }
            else
            {
                FoundRegion = this.SessionRegionGroupRepository.GetSessionRegion();
                FoundRegion.Name = "Enter a valid name";
            }
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
        public ActionResult EditRegionGroup(IdViewModel idForm)
        {
            var contentViewModel = new ContentViewModel
            {
                SearchViewModel = new SearchViewModel()
                {
                    IsSearching = true,
                }
            };
            var parseId = new Guid();
            Guid.TryParse(idForm.Id, out parseId);
            var FoundRegion = this.SearchRegion.FindById(idForm.Id);
            if (FoundRegion?.Id == Guid.Empty)
            {
                var FoundEntry = this.SearchEntry.FindById(idForm.Id);
                FoundRegion = FoundEntry.CustomRegionGroup;
                if (FoundRegion?.Id == Guid.Empty)
                {
                    return PartialView("_Content", contentViewModel);
                }
            }
            FoundRegion.CustomRegionEntries = FoundRegion.CustomRegionEntries.OrderBy(a => a.Airport?.Id)
                                                                            .ThenBy(a => a.City?.Name)
                                                                            .ThenBy(a => a.State?.Name)
                                                                            .ThenBy(a => a.Country?.Name)
                                                                            .ThenBy(a => a.Region?.Name).ToList();
            this.SessionRegionGroupRepository.SetSessionRegion(FoundRegion);
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

        public ActionResult UpdateRegionGroup()
        {
            var FoundRegion = this.SessionRegionGroupRepository.GetSessionRegion();
            FoundRegion.CustomRegionEntries = FoundRegion.CustomRegionEntries.OrderBy(a => a.Airport?.Id)
                                                                            .ThenBy(a => a.City?.Name)
                                                                            .ThenBy(a => a.State?.Name)
                                                                            .ThenBy(a => a.Country?.Name)
                                                                            .ThenBy(a => a.Region?.Name).ToList();
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
        public ActionResult AutoComplete(AutoCompleteFormViewModel autoCompleteForm)
        {
            var type = autoCompleteForm.Type;
            var text = autoCompleteForm.Text;

            var autoCompleteViewModel = new AutoCompleteViewModel();
            if (string.IsNullOrEmpty(text))
            {
                return PartialView("_AutoComplete", autoCompleteViewModel);
            }
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

        public ActionResult Index()
        {
            var layoutViewModel = new LayoutViewModel();
            this.SessionRegionGroupRepository.ClearSession();
            return View(layoutViewModel);
        }
    }
}