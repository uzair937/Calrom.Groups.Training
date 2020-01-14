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
using CustomRegionEditor.Handler.Factories;

namespace CustomRegionEditor.Controllers
{
    public class HomeController : Controller
    {
        public HomeController(ISessionStore sessionStore, IManagerFactory managerFactory, IViewModelConverter viewModelConverter, IRegionHandler regionHandler, ISearchRegion searchRegion, ISearchEntry searchEntry)
        {
            this.ViewModelConverter = viewModelConverter;
            this.RegionHandler = regionHandler;
            this.SearchRegion = searchRegion;
            this.SearchEntry = searchEntry;
            this.SessionStore = sessionStore;
            this.ManagerFactory = managerFactory;

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

        public IRegionHandler RegionHandler { get; private set; }
        public ISearchRegion SearchRegion { get; private set; }
        public ISearchEntry SearchEntry { get; private set; }
        public ISessionStore SessionStore { get; private set; }
        public IManagerFactory ManagerFactory { get; private set; }

        [HttpPost]
        public ActionResult Search(SearchBoxViewModel searchForm)
        {
            this.SessionStore.Clear();
            var searchTerm = searchForm.text;
            bool ValidSearch = false;
            var searchResults = this.SearchRegion.GetSearchResults(searchTerm);

            var contentViewModel = new ContentViewModel
            {
                SearchViewModel = new SearchViewModel()
                {
                    IsSearching = true,
                    InvalidSearchTerm = searchTerm
                }
            };
            if (searchResults.Count > 0) ValidSearch = true;

            if (ValidSearch)
            {
                contentViewModel.SubRegionViewModel.ValidResults = true;
                contentViewModel.SearchViewModel.ValidResults = true;
                contentViewModel.SearchViewModel.SearchResults = ViewModelConverter.GetView(searchResults);
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
            var name = deleteForm.Name;
            var description = deleteForm.Description;
            var currentRegion = this.SessionStore.Get();

            currentRegion.CustomRegions = currentRegion.CustomRegions.Where(s => s.Id != id).ToList();

            var errorViewModel = new ErrorViewModel();

            //this.SessionStore.SetDetails(name, description);
            //var foundEntry = currentRegion.CustomRegionEntries.FirstOrDefault(a => a.Airport?.Id == id
            //                                                                 || a.City?.Id == id
            //                                                                 || a.State?.Id == id
            //                                                                 || a.Country?.Id == id
            //                                                                 || a.Region?.Id == id);
            //if (foundEntry == null)
            //{
            //    errorViewModel.FailedToDelete = id;
            //    errorViewModel.DeleteFailed = true;
            //}

            //currentRegion.CustomRegionEntries.Remove(foundEntry);

            this.SessionStore.Save(currentRegion);

            return UpdateRegionGroup(errorViewModel);
        }

        [HttpPost]
        public ActionResult AddRegionEntry(EntryFormViewModel regionForm)
        {
            var entry = regionForm.Entry;
            var type = regionForm.Type;
            var name = regionForm.Name;
            var description = regionForm.Description;

            var currentViewModel = this.SessionStore.Get();
            currentViewModel.Name = name;
            currentViewModel.Description = description;

            if (currentViewModel.CustomRegions == null)
            {
                currentViewModel.CustomRegions = new List<CustomRegionEntryViewModel>();
            }

            switch (type)
            {
                case "region":
                    currentViewModel.CustomRegions.Add(new CustomRegionEntryViewModel
                    {
                        Region = new RegionViewModel
                        {
                            Name = entry
                        }
                    });
                    break;
            }

            //var errorViewModel = this.ViewModelConverter.GetView(errorModel);
            return UpdateRegionGroup(new ErrorViewModel());
        }

        [HttpPost]
        public ActionResult SaveChanges(SaveFormViewModel saveForm)
        {
            var contentViewModel = new ContentViewModel
            {
                EditViewModel = new EditViewModel()
                {
                    IsEditing = true,
                    ExistingRegion = true
                }
            };

            if (ModelState.IsValid)
            {
                var customRegionManager = this.ManagerFactory.CreateCustomRegionManager();
                var name = saveForm.Name;
                var description = saveForm.Description;
                var regionId = saveForm.Id;

                var foundRegion = this.SessionStore.Get();
                //var validName = this.SessionRegionGroupRepository.ValidName(name);
                var validName = true;

                if (validName)
                {
                    foundRegion.Id = regionId;
                    foundRegion.Name = name;
                    foundRegion.Description = description;
                    var result = customRegionManager.Add(new CustomRegionGroupModel
                    {
                        Name = foundRegion.Name,
                        CustomRegionEntries = foundRegion.CustomRegions.Select(s => new CustomRegionEntryModel
                        {
                            Airport = s.Airport != null ? new AirportModel { Id = s.Id } : null,
                            City = s.City != null ? new CityModel { Id = s.Id } : null,
                            Region = s.Region != null ? new RegionModel { Id = s.Id } : null,
                            State = s.State != null ? new StateModel { Id = s.Id } : null,
                            Country = s.Country != null ? new CountryModel { Id = s.Id } : null,
                        }).ToList()
                    });

                    if (result != null)
                    {
                        foundRegion = ViewModelConverter.GetView(result);
                    }
                    else
                    {

                    }
                }
                else
                {
                    foundRegion.Name = "Enter a valid name";
                }

                contentViewModel.EditViewModel.CustomRegionGroupViewModel = foundRegion;
            }

            return PartialView("_Content", contentViewModel);
        }

        [HttpPost]
        public ActionResult NewCustomRegion()
        {
            var customRegionGroupViewModel = this.SessionStore.Get();

            if (customRegionGroupViewModel == null)
            {
                customRegionGroupViewModel = new CustomRegionGroupViewModel
                {
                    Name = "",
                    Description = "",
                    CustomRegions = new List<CustomRegionEntryViewModel>()
                };

                this.SessionStore.Save(customRegionGroupViewModel);
            }

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
            var foundRegion = this.SearchRegion.FindById(idForm.Id);
            if (foundRegion?.Id == Guid.Empty)
            {
                var foundEntry = this.SearchEntry.FindById(idForm.Id);
                foundRegion = foundEntry.CustomRegionGroup;
                if (foundRegion?.Id == Guid.Empty)
                {
                    return PartialView("_Content", contentViewModel);
                }
            }
            foundRegion.CustomRegionEntries = foundRegion.CustomRegionEntries.OrderBy(a => a.Airport?.Id)
                                                                            .ThenBy(a => a.City?.Name)
                                                                            .ThenBy(a => a.State?.Name)
                                                                            .ThenBy(a => a.Country?.Name)
                                                                            .ThenBy(a => a.Region?.Name).ToList();

            var entryView = ViewModelConverter.GetView(foundRegion);
            this.SessionStore.Save(entryView);
            
            contentViewModel = new ContentViewModel
            {
                EditViewModel = new EditViewModel()
                {
                    IsEditing = true,
                    ExistingRegion = true,
                    CustomRegionGroupViewModel = entryView
                },
            };
            return PartialView("_Content", contentViewModel);
        }

        public ActionResult UpdateRegionGroup(ErrorViewModel errorViewModel)
        {
            var contentViewModel = new ContentViewModel
            {
                DbChanges = errorViewModel,
                EditViewModel = new EditViewModel()
                {
                    IsEditing = true,
                    ExistingRegion = true,
                    CustomRegionGroupViewModel = this.SessionStore.Get(),
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

        [HttpPost]
        public ActionResult SearchComplete(AutoCompleteFormViewModel autoCompleteForm)
        {
            var text = autoCompleteForm.Text;

            var autoCompleteViewModel = new AutoCompleteViewModel();
            if (string.IsNullOrEmpty(text))
            {
                return PartialView("_AutoComplete", autoCompleteViewModel);
            }

            autoCompleteViewModel.Suggestions = this.SearchRegion.SearchCustomRegions(text);

            autoCompleteViewModel.Suggestions = autoCompleteViewModel.Suggestions.Concat(GetRegions(text)).ToList();

            autoCompleteViewModel.Suggestions = autoCompleteViewModel.Suggestions.Concat(GetCountries(text)).ToList();

            autoCompleteViewModel.Suggestions = autoCompleteViewModel.Suggestions.Concat(GetStates(text)).ToList();

            autoCompleteViewModel.Suggestions = autoCompleteViewModel.Suggestions.Concat(GetCities(text)).ToList();

            autoCompleteViewModel.Suggestions = autoCompleteViewModel.Suggestions.Concat(GetAirports(text)).ToList();



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
            this.SessionStore.Clear();
            return View(layoutViewModel);
        }
    }
}