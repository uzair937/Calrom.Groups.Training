using CustomRegionEditor.Database.Interfaces;
using CustomRegionEditor.Handler.Factories;
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
        public HomeController(ISessionStore sessionStore, IManagerFactory managerFactory, ISessionManager sessionManager, IViewModelConverter viewModelConverter, IValidatorFactory validatorFactory)
        {
            this.ViewModelConverter = viewModelConverter;
            this.ValidatorFactory = validatorFactory;
            this.SessionStore = sessionStore;
            this.ManagerFactory = managerFactory;
            this.SessionManager = sessionManager;

            SetupAutoCompleteList();
        }

        private List<AirportViewModel> Airports = null;
        private List<CityViewModel> Cities = null;
        private List<StateViewModel> States = null;
        private List<CountryViewModel> Countries = null;
        private List<RegionViewModel> Regions = null;

        private readonly IViewModelConverter ViewModelConverter;

        private readonly IValidatorFactory ValidatorFactory;

        private readonly ISessionManager SessionManager;

        public readonly ISessionStore SessionStore;

        public readonly IManagerFactory ManagerFactory;

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

        [HttpPost]
        public ActionResult Search(SearchBoxViewModel searchForm)
        {
            this.SessionStore.Clear();
            var searchTerm = searchForm.text;

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

            if (string.IsNullOrEmpty(id))
            {
                currentRegion.CustomRegions = currentRegion.CustomRegions.Where(s => s.LocationName != name).ToList();
            }
            else
            {
                currentRegion.CustomRegions = currentRegion.CustomRegions.Where(s => s.LocationId != id).ToList();
            }

            var errorModels = new List<ErrorViewModel>();

            this.SessionStore.Save(currentRegion);


            return UpdateRegionGroup(errorModels);
        }

        [HttpPost]
        public ActionResult AddRegionEntry(EntryFormViewModel regionForm)
        {
            using (var session = this.SessionManager.OpenSession())
            {
                var entryValidator = this.ValidatorFactory.CreateCustomRegionValidator(session);

                var currentViewModel = this.SessionStore.Get();
                currentViewModel.Name = regionForm.Name;
                currentViewModel.Description = regionForm.Description;

                if (currentViewModel.CustomRegions == null)
                {
                    currentViewModel.CustomRegions = new List<CustomRegionEntryViewModel>();
                }

                var entryViewModel = this.CreateEntryViewModel(regionForm.Type, regionForm.Entry);
                currentViewModel.CustomRegions.Add(entryViewModel);

                var customRegionGroupModel = this.ViewModelConverter.GetModel(currentViewModel);

                var validationResult = entryValidator.IsValid(customRegionGroupModel);

                if (validationResult.Errors.Any())
                {
                    var errorViewModel = this.ViewModelConverter.GetView(validationResult.Errors);
                    if (errorViewModel != null)
                    {
                        return UpdateRegionGroup(errorViewModel);
                    }
                }

                this.SessionStore.Save(currentViewModel);

                return UpdateRegionGroup(new List<ErrorViewModel>());
            }
        }

        private CustomRegionEntryViewModel CreateEntryViewModel(string type, string entry)
        {
            var entryId = entry;
            if (entryId.Contains(","))
            {
                var index = entryId.IndexOf(",");
                entryId = entryId.Substring(0, index);

                var idEntry = GetEntry(type, entryId);
                if (idEntry != null)
                {
                    return idEntry;
                }
            }
            return GetEntry(type, entry);
        }
        
        private CustomRegionEntryViewModel GetEntry(string type, string entry)
        {
            switch (type)
            {
                case "airport":
                    var airport = this.Airports.FirstOrDefault(s => s.Id == entry || s.Name == entry);
                    return new CustomRegionEntryViewModel { Airport = airport, LocationId = airport.Id, LocationName = airport.Name };
                case "state":
                    var state = this.States.FirstOrDefault(s => s.Id == entry || s.Name == entry);
                    return new CustomRegionEntryViewModel { State = state, LocationId = state.Id, LocationName = state.Name };
                case "city":
                    var city = this.Cities.FirstOrDefault(s => s.Id == entry || s.Name == entry);
                    return new CustomRegionEntryViewModel { City = city, LocationId = city.Id, LocationName = city.Name };
                case "country":
                    var country = this.Countries.FirstOrDefault(s => s.Id == entry || s.Name == entry);
                    return new CustomRegionEntryViewModel { Country = country, LocationId = country.Id, LocationName = country.Name };
                case "region":
                    var region = this.Regions.FirstOrDefault(s => s.Id == entry || s.Name == entry);
                    return new CustomRegionEntryViewModel { Region = region, LocationId = region.Id, LocationName = region.Name };
                default:
                    return null;
            }
        }

        [HttpPost]
        public ActionResult SaveChanges(SaveFormViewModel saveForm)
        {
            using (var session = this.SessionManager.OpenSession())
            {
                var validationModel = new ValidationModel();
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

                        var customRegionModel = this.ViewModelConverter.GetModel(storedRegion);

                        var managerResult = customRegionManager.Add(customRegionModel);
                        validationModel.Merge(managerResult.ValidationResult);

                        if (managerResult.Object != null)
                        {
                            storedRegion = ViewModelConverter.GetView(managerResult.Object);
                        }
                        else
                        {
                            this.SessionStore.Clear();
                        }
                    }
                    else
                    {
                        storedRegion.Name = "Enter a valid name";
                    }

                    contentViewModel.EditViewModel.CustomRegionGroupViewModel = storedRegion;
                    contentViewModel.ErrorModels = ViewModelConverter.GetView(validationModel.Errors);
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

        public ActionResult UpdateRegionGroup(List<ErrorViewModel> errorViewModels)
        {
            var contentViewModel = new ContentViewModel
            {
                ErrorModels = errorViewModels,
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

            results.ForEach(a => a.Display = a.Id + ", " + a.Name + " (" + a.Type + ")");

            return results.Select(a => a.Display).ToList();
        }

        public List<string> GetRegions(string term)
        {
            var results = Regions.Where(c => c.Id.StartsWith(term, StringComparison.OrdinalIgnoreCase)).ToList();
            results = results.Concat(Regions.Where(c => c.Name.StartsWith(term, StringComparison.OrdinalIgnoreCase))).ToList();
            results = results.Distinct().ToList();

            results.ForEach(a => a.Display = a.Id + ", " + a.Name + " (" + a.Type + ")");

            return results.Select(a => a.Display).ToList();
        }

        public List<string> GetStates(string term)
        {
            var results = States.Where(c => c.Id.StartsWith(term, StringComparison.OrdinalIgnoreCase)).ToList();
            results = results.Concat(States.Where(c => c.Name.StartsWith(term, StringComparison.OrdinalIgnoreCase))).ToList();
            results = results.Distinct().ToList();

            results.ForEach(a => a.Display = a.Id + ", " + a.Name + " (" + a.Type + ")");

            return results.Select(a => a.Display).ToList();
        }

        public List<string> GetCities(string term)
        {
            var results = Cities.Where(c => c.Id.StartsWith(term, StringComparison.OrdinalIgnoreCase)).ToList();
            results = results.Concat(Cities.Where(c => c.Name.StartsWith(term, StringComparison.OrdinalIgnoreCase))).ToList();
            results = results.Distinct().ToList();

            results.ForEach(a => a.Display = a.Id + ", " + a.Name + " (" + a.Type + ")");

            return results.Select(a => a.Display).ToList();
        }

        public List<string> GetAirports(string term)
        {
            var results = Airports.Where(c => c.Id.StartsWith(term, StringComparison.OrdinalIgnoreCase)).ToList();
            results = results.Concat(Airports.Where(c => c.Name.StartsWith(term, StringComparison.OrdinalIgnoreCase))).ToList();
            results = results.Distinct().ToList();

            results.ForEach(a => a.Display = a.Id + ", " + a.Name + " (" + a.Type + ")");

            return results.Select(a => a.Display).ToList();
        }

        public ActionResult Index()
        {
            var layoutViewModel = new LayoutViewModel();
            return View(layoutViewModel);
        }
    }
}