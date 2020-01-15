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
        public HomeController(ISessionStore sessionStore, IManagerFactory managerFactory, ISessionManager sessionManager, IViewModelConverter viewModelConverter)
        {
            this.ViewModelConverter = viewModelConverter;
            this.SessionStore = sessionStore;
            this.ManagerFactory = managerFactory;
            this.SessionManager = sessionManager;

            SetupAutoCompleteList();
        }

        private void SetupAutoCompleteList()
        {
            using (var session = this.SessionManager.OpenSession())
            {
                var customRegionManager = this.ManagerFactory.CreateCustomRegionManager(session);
                var regionLists = customRegionManager.GetRegionList();
                Airports = this.ViewModelConverter.GetView(regionLists.Airports);
                Cities = this.ViewModelConverter.GetView(regionLists.Cities);
                States = this.ViewModelConverter.GetView(regionLists.States);
                Countries = this.ViewModelConverter.GetView(regionLists.Countries);
                Regions = this.ViewModelConverter.GetView(regionLists.Regions);
            }
        }

        private List<AirportViewModel> Airports = null;
        private List<CityViewModel> Cities = null;
        private List<StateViewModel> States = null;
        private List<CountryViewModel> Countries = null;
        private List<RegionViewModel> Regions = null;

        private readonly IViewModelConverter ViewModelConverter = null;

        private ISessionManager SessionManager;

        public ISessionStore SessionStore { get; private set; }

        public IManagerFactory ManagerFactory { get; private set; }

        [HttpPost]
        public ActionResult Search(SearchBoxViewModel searchForm)
        {
            this.SessionStore.Clear();
            var searchTerm = searchForm.text;

            if (searchTerm.Contains(","))
            {
                var index = searchTerm.IndexOf(",");
                searchTerm = searchTerm.Substring(0, index);
            }


            var contentViewModel = new ContentViewModel
            {
                SearchViewModel = new SearchViewModel()
                {
                    IsSearching = true,
                    InvalidSearchTerm = searchTerm
                }
            };
            using (var session = this.SessionManager.OpenSession())
            {
                var customRegionManager = this.ManagerFactory.CreateCustomRegionManager(session);
                var searchRegionManager = this.ManagerFactory.CreateSearchRegionManager(session);


                this.SessionStore.Clear();
               
                bool validSearch = false;
                var searchResults = searchRegionManager.GetSearchResults(searchTerm);



                if (searchResults.Count > 0) validSearch = true;

                if (validSearch)
                {
                    contentViewModel.SubRegionViewModel.ValidResults = true;
                    contentViewModel.SearchViewModel.ValidResults = true;
                    contentViewModel.SearchViewModel.SearchResults = ViewModelConverter.GetView(searchResults);
                }
            }
            return PartialView("_Content", contentViewModel);
        }

        [HttpPost]
        public ActionResult DeleteRegionGroup(IdViewModel idForm)
        {
            using (var session = this.SessionManager.OpenSession())
            {
                var customRegionManager = this.ManagerFactory.CreateCustomRegionManager(session);
                customRegionManager.DeleteById(idForm.Id);
                return Search(idForm.LastSearch);
            }
        }

        [HttpPost]
        public ActionResult DeleteEntry(DeleteViewModel deleteForm)
        {
            var id = deleteForm.EntryId;
            var name = deleteForm.Name;
            var description = deleteForm.Description;
            var currentRegion = this.SessionStore.Get();
            currentRegion.Name = name;
            currentRegion.Description = description;

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
                case "country":
                    currentViewModel.CustomRegions.Add(new CustomRegionEntryViewModel
                    {
                        Country = new CountryViewModel
                        {
                            Name = entry
                        }
                    });
                    break;
                case "state":
                    currentViewModel.CustomRegions.Add(new CustomRegionEntryViewModel
                    {
                        State = new StateViewModel
                        {
                            Name = entry
                        }
                    });
                    break;
                case "city":
                    currentViewModel.CustomRegions.Add(new CustomRegionEntryViewModel
                    {
                        City = new CityViewModel
                        {
                            Name = entry
                        }
                    });
                    break;
                case "airport":
                    currentViewModel.CustomRegions.Add(new CustomRegionEntryViewModel
                    {
                        Airport = new AirportViewModel
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
            using (var session = this.SessionManager.OpenSession())
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
                    var customRegionManager = this.ManagerFactory.CreateCustomRegionManager(session);
                    var name = saveForm.Name;
                    var description = saveForm.Description;
                    var regionId = saveForm.Id;

                    var storedRegion = this.SessionStore.Get();
                    //var validName = this.SessionRegionGroupRepository.ValidName(name);
                    var validName = true;

                    if (validName)
                    {
                        storedRegion.Id = regionId;
                        storedRegion.Name = name;
                        storedRegion.Description = description;

                        //var customRegionModel = ConvertToModel(storedRegion);

                        var result = customRegionManager.Add(new CustomRegionGroupModel());

                        if (result != null)
                        {
                            storedRegion = ViewModelConverter.GetView(result);
                        }
                        else
                        {

                        }
                    }
                    else
                    {
                        storedRegion.Name = "Enter a valid name";
                    }

                    contentViewModel.EditViewModel.CustomRegionGroupViewModel = storedRegion;
                }
                return PartialView("_Content", contentViewModel);
            }
        }

        [HttpPost]
        public ActionResult NewCustomRegion()
        {
            this.SessionStore.Clear();
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
            using (var session = this.SessionManager.OpenSession())
            {
                var searchRegionManager = this.ManagerFactory.CreateSearchRegionManager(session);
                var searchEntryManager = this.ManagerFactory.CreateSearchEntryManager(session);
                var contentViewModel = new ContentViewModel
                {
                    SearchViewModel = new SearchViewModel()
                    {
                        IsSearching = true,
                    }
                };
                var parseId = new Guid();
                Guid.TryParse(idForm.Id, out parseId);

                var foundRegion = searchRegionManager.FindById(idForm.Id);
                if (foundRegion?.Id == Guid.Empty)
                {
                    var foundEntry = searchEntryManager.FindById(idForm.Id);
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
            using (var session = this.SessionManager.OpenSession())
            {
                var searchRegionManager = this.ManagerFactory.CreateSearchRegionManager(session);

                autoCompleteViewModel.Suggestions = searchRegionManager.SearchCustomRegions(text);
            }

            autoCompleteViewModel.Suggestions = autoCompleteViewModel.Suggestions.Concat(GetRegions(text)).ToList();

            autoCompleteViewModel.Suggestions = autoCompleteViewModel.Suggestions.Concat(GetCountries(text)).ToList();

            autoCompleteViewModel.Suggestions = autoCompleteViewModel.Suggestions.Concat(GetStates(text)).ToList();

            autoCompleteViewModel.Suggestions = autoCompleteViewModel.Suggestions.Concat(GetCities(text)).ToList();

            autoCompleteViewModel.Suggestions = autoCompleteViewModel.Suggestions.Concat(GetAirports(text)).ToList();



            return PartialView("_AutoComplete", autoCompleteViewModel);
        }

        public List<string> GetCountries(string term)
        {
            var results = Countries.Where(c => c.Id.StartsWith(term, StringComparison.OrdinalIgnoreCase)).ToList();
            results = results.Concat(Countries.Where(c => c.Name.StartsWith(term, StringComparison.OrdinalIgnoreCase))).ToList();
            results = results.Distinct().ToList();

            results.ForEach(a => a.Display = a.Id + ", " + a.Name);

            return results.Select(a => a.Display).ToList();
        }

        public List<string> GetRegions(string term)
        {
            var results = Regions.Where(c => c.Id.StartsWith(term, StringComparison.OrdinalIgnoreCase)).ToList();
            results = results.Concat(Regions.Where(c => c.Name.StartsWith(term, StringComparison.OrdinalIgnoreCase))).ToList();
            results = results.Distinct().ToList();

            results.ForEach(a => a.Display = a.Id + ", " + a.Name);

            return results.Select(a => a.Display).ToList();
        }

        public List<string> GetStates(string term)
        {
            var results = States.Where(c => c.Id.StartsWith(term, StringComparison.OrdinalIgnoreCase)).ToList();
            results = results.Concat(States.Where(c => c.Name.StartsWith(term, StringComparison.OrdinalIgnoreCase))).ToList();
            results = results.Distinct().ToList();

            results.ForEach(a => a.Display = a.Id + ", " + a.Name);

            return results.Select(a => a.Display).ToList();
        }

        public List<string> GetCities(string term)
        {
            var results = Cities.Where(c => c.Id.StartsWith(term, StringComparison.OrdinalIgnoreCase)).ToList();
            results = results.Concat(Cities.Where(c => c.Name.StartsWith(term, StringComparison.OrdinalIgnoreCase))).ToList();
            results = results.Distinct().ToList();

            results.ForEach(a => a.Display = a.Id + ", " + a.Name);

            return results.Select(a => a.Display).ToList();
        }

        public List<string> GetAirports(string term)
        {
            var results = Airports.Where(c => c.Id.StartsWith(term, StringComparison.OrdinalIgnoreCase)).ToList();
            results = results.Concat(Airports.Where(c => c.Name.StartsWith(term, StringComparison.OrdinalIgnoreCase))).ToList();
            results = results.Distinct().ToList();

            results.ForEach(a => a.Display = a.Id + ", " + a.Name);

            return results.Select(a => a.Display).ToList();
        }

        public ActionResult Index()
        {
            var layoutViewModel = new LayoutViewModel();
            return View(layoutViewModel);
        }
    }
}