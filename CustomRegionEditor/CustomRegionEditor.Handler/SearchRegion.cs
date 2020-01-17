using CustomRegionEditor.Database.Factories;
using CustomRegionEditor.Database.Interfaces;
using CustomRegionEditor.Database.Models;
using CustomRegionEditor.Handler.Factories;
using CustomRegionEditor.Handler.Interfaces;
using CustomRegionEditor.Models;
using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CustomRegionEditor.Handler
{
    public class SearchRegion : ISearchRegion
    {
        public SearchRegion(ISession session, IModelConverter modelConverter, IRepositoryFactory repositoryFactory)
        {
            this.Session = session;
            this.ModelConverter = modelConverter;
            this.RepositoryFactory = repositoryFactory;
            this.CustomRegionGroupRepository = this.RepositoryFactory.CreateCustomRegionGroupRepository(Session);
        }

        private ICustomRegionGroupRepository CustomRegionGroupRepository { get; }
        private IRepositoryFactory RepositoryFactory { get; }
        private IModelConverter ModelConverter { get; }
        private ISession Session { get; }

        public bool CheckValidName(string name)
        {
            var validName = true;
            var regionList = this.CustomRegionGroupRepository.List();
            if (regionList.Select(a => a.Name).Contains(name))
            {
                validName = false;
            }
            return validName;
        }

        public CustomRegionGroupModel FindById(string id)
        {
            var parsedId = Guid.Parse(id);
            var foundRegion = this.CustomRegionGroupRepository.FindById(parsedId);
            var modelRegion = this.ModelConverter.GetModel(foundRegion);
            return modelRegion;
        }

        public List<CustomRegionGroupModel> GetSearchResults(string searchTerm)
        {
            var searchId = "";
            var setSearchType = "";
            var dbList = this.CustomRegionGroupRepository.List();
            var regionList = this.ModelConverter.GetModel(dbList);

            if (searchTerm.Equals("-All", StringComparison.OrdinalIgnoreCase)
                    || searchTerm.Equals("none", StringComparison.OrdinalIgnoreCase))
            {
                return regionList;
            }

            var regionRepo = this.RepositoryFactory.CreateRegionRepository(Session);
            var countryRepo = this.RepositoryFactory.CreateCountryRepository(Session);
            var stateRepo = this.RepositoryFactory.CreateStateRepository(Session);
            var cityRepo = this.RepositoryFactory.CreateCityRepository(Session);
            var airportRepo = this.RepositoryFactory.CreateAirportRepository(Session);

            if (searchTerm.Contains(",")) //gets id
            {
                var index = searchTerm.IndexOf(",");
                searchId = searchTerm.Substring(0, index);
            }
            if (searchTerm.Contains("(") && searchTerm.Contains(")")) //gets type
            {
                var index = searchTerm.IndexOf("(") + 1;
                var endIndex = searchTerm.IndexOf(")");
                setSearchType = searchTerm.Substring(index, (endIndex - index));
            }

            //Search through custom region names

            var startsWith = regionList.Where(s => s.Name.StartsWith(searchTerm, StringComparison.OrdinalIgnoreCase)).ToList();

            var contains = regionList.Where(s =>
                !s.Name.StartsWith(searchTerm, StringComparison.OrdinalIgnoreCase)
                && (s.Name.Contains(searchTerm)
                    || (!string.IsNullOrEmpty(s.Description) && s.Description.Contains(searchTerm)))).ToList();

            var returnCustomRegionGroupList = startsWith.Concat(contains).ToList();

            var searchTypes = new List<string>();
            var searchIds = new List<string>();

            if (string.IsNullOrEmpty(setSearchType))
            {
                var foundRegion = regionRepo.Find(searchTerm);
                var foundCountry = countryRepo.Find(searchTerm);
                var foundState = stateRepo.Find(searchTerm);
                var foundCity = cityRepo.Find(searchTerm);
                var foundAirport = airportRepo.Find(searchTerm);

                if (foundRegion != null)
                {
                    searchTypes.Add("Region");
                    searchIds.Add(foundRegion.Id);
                }
                if (foundCountry != null)
                {
                    searchTypes.Add("Country");
                    searchIds.Add(foundCountry.Id);
                }
                if (foundState != null)
                {
                    searchTypes.Add("State");
                    searchIds.Add(foundState.Id);
                }
                if (foundCity != null)
                {
                    searchTypes.Add("City");
                    searchIds.Add(foundCity.Id);
                }
                if (foundAirport != null)
                {
                    searchTypes.Add("Airport");
                    searchIds.Add(foundAirport.Id);
                }
                //FIND IF IT HAS A LOCATION MATCH
                //THEN ASSIGN SEARCH TYPE
            }
            else
            {
                searchTypes.Add(setSearchType);
            }

            //Search through locations

            if (searchTypes.Count > 0)
            {
                foreach (var searchType in searchTypes)
                {
                    var regionIdModel = new LocationIdModel
                    {
                        Type = "Region"
                    };
                    var countryIdModel = new LocationIdModel
                    {
                        Type = "Country"
                    };
                    var stateIdModel = new LocationIdModel
                    {
                        Type = "State"
                    };
                    var cityIdModel = new LocationIdModel
                    {
                        Type = "City"
                    };
                    var airportIdModel = new LocationIdModel
                    {
                        Type = "Airport"
                    };
                    if (searchType.Equals("Region")) //REGION
                    {
                        var foundRegion = new Region();
                        if (string.IsNullOrEmpty(searchId))
                        {
                            foundRegion = regionRepo.Find(searchTerm);
                        }
                        else
                        {
                            foundRegion = regionRepo.Find(searchId);
                        }
                        if (foundRegion == null) break;

                        //Get superRegions
                        regionIdModel.SearchId.Add(foundRegion.Id);
                        regionIdModel.HighlightModel.Match = true;

                        //Get subRegions
                        //countryIdModel.SearchId.AddRange(foundRegion.Countries.Select(a => a.Id));

                        //var countries = foundRegion.Countries;

                        //var cities = new List<City>();
                        //var airports = new List<Airport>();

                        //foreach (var country in countries)
                        //{
                        //    cityIdModel.SearchId.AddRange(country.Cities.Select(a => a.Id));
                        //    cities.AddRange(country.Cities);

                        //    if (country.States != null)
                        //    {
                        //        stateIdModel.SearchId.AddRange(country.States.Select(a => a.Id));
                        //    }
                        //}
                        //foreach (var city in cities)
                        //{
                        //    airportIdModel.SearchId.AddRange(city.Airports.Select(a => a.Id));
                        //}
                    }
                    else if (searchType.Equals("Country"))  //COUNTRY
                    {
                        var foundCountry = new Country();
                        if (string.IsNullOrEmpty(searchId))
                        {
                            foundCountry = countryRepo.Find(searchTerm);
                        }
                        else
                        {
                            foundCountry = countryRepo.Find(searchId);
                        }
                        if (foundCountry == null) break;

                        //Get superRegions
                        countryIdModel.SearchId.Add(foundCountry.Id);
                        countryIdModel.HighlightModel.Match = true;

                        regionIdModel.SearchId.Add(foundCountry.Region.Id);

                        //Get subRegions
                        //cityIdModel.SearchId.AddRange(foundCountry.Cities.Select(a => a.Id));
                        //var cities = foundCountry.Cities;

                        //if (foundCountry.States != null)
                        //{
                        //    stateIdModel.SearchId.AddRange(foundCountry.States.Select(a => a.Id));
                        //}

                        //foreach (var city in cities)
                        //{
                        //    airportIdModel.SearchId.AddRange(city.Airports.Select(a => a.Id));
                        //}
                    }
                    else if (searchType.Equals("State")) //STATE
                    {
                        var foundState = new State();
                        if (string.IsNullOrEmpty(searchId))
                        {
                            foundState = stateRepo.Find(searchTerm);
                        }
                        else
                        {
                            foundState = stateRepo.Find(searchId);
                        }
                        if (foundState == null) break;
                        //Get superRegions
                        stateIdModel.SearchId.Add(foundState.Id);
                        stateIdModel.HighlightModel.Match = true;

                        countryIdModel.SearchId.Add(foundState.Country.Id);
                        regionIdModel.SearchId.Add(foundState.Country.Region.Id);

                        //Get subRegions
                        //cityIdModel.SearchId.AddRange(foundState.Cities.Select(a => a.Id));
                        //var cities = foundState.Cities;

                        //foreach (var city in cities)
                        //{
                        //    airportIdModel.SearchId.AddRange(city.Airports.Select(a => a.Id));
                        //}
                    }
                    else if (searchType.Equals("City")) //CITY
                    {
                        var foundCity = new City();
                        if (string.IsNullOrEmpty(searchId))
                        {
                            foundCity = cityRepo.Find(searchTerm);
                        }
                        else
                        {
                            foundCity = cityRepo.Find(searchId);
                        }
                        if (foundCity == null) break;
                        //Get superRegions
                        cityIdModel.SearchId.Add(foundCity.Id);
                        cityIdModel.HighlightModel.Match = true;

                        countryIdModel.SearchId.Add(foundCity.Country.Id);
                        regionIdModel.SearchId.Add(foundCity.Country.Region.Id);
                        if (foundCity.State != null)
                        {
                            stateIdModel.SearchId.Add(foundCity.State.Id);
                        }

                        //Get subRegions
                        //airportIdModel.SearchId.AddRange(foundCity.Airports.Select(a => a.Id));
                    }
                    else if (searchType.Equals("Airport")) //AIRPORT
                    {
                        var foundAirports = new Airport();
                        if (string.IsNullOrEmpty(searchId))
                        {
                            foundAirports = airportRepo.Find(searchTerm);
                        }
                        else
                        {
                            foundAirports = airportRepo.Find(searchId);
                        }
                        if (foundAirports == null) break;
                        //Get superRegions
                        airportIdModel.SearchId.Add(foundAirports.Id);
                        airportIdModel.HighlightModel.Match = true;

                        cityIdModel.SearchId.Add(foundAirports.City.Id);
                        countryIdModel.SearchId.Add(foundAirports.City.Country.Id);
                        regionIdModel.SearchId.Add(foundAirports.City.Country.Region.Id);
                        if (foundAirports.City.State != null)
                        {
                            stateIdModel.SearchId.Add(foundAirports.City.State.Id);
                        }
                    }


                    var searchModel = new SearchModel
                    {
                        RegionList = regionList
                    };

                    searchModel.SearchTerm = regionIdModel;
                    returnCustomRegionGroupList.AddRange(SearchByLocation(searchModel));

                    searchModel.SearchTerm = countryIdModel;
                    returnCustomRegionGroupList.AddRange(SearchByLocation(searchModel));

                    searchModel.SearchTerm = stateIdModel;
                    returnCustomRegionGroupList.AddRange(SearchByLocation(searchModel));

                    searchModel.SearchTerm = cityIdModel;
                    returnCustomRegionGroupList.AddRange(SearchByLocation(searchModel));

                    searchModel.SearchTerm = airportIdModel;
                    returnCustomRegionGroupList.AddRange(SearchByLocation(searchModel));
                }
            }

            returnCustomRegionGroupList = returnCustomRegionGroupList.GroupBy(a => a.Id).Select(b => b.First()).ToList();

            returnCustomRegionGroupList.Where(s => s.Name.Equals(searchTerm, StringComparison.OrdinalIgnoreCase)).ToList().ForEach(a => a.HighlightModel.Match = true);

            return returnCustomRegionGroupList;
        }

        public List<CustomRegionGroupModel> SearchByLocation(SearchModel searchModel)
        {
            var foundRegions = new List<CustomRegionGroupModel>();

            var searchType = searchModel.SearchTerm.Type;
            var searchIds = searchModel.SearchTerm.SearchId;
            var regionList = searchModel.RegionList;

            foreach (var searchId in searchIds)
            {
                if (!string.IsNullOrEmpty(searchType))
                {
                    if (searchType.Equals("Region"))
                    {
                        foundRegions.AddRange(regionList.Where(a => a.CustomRegionEntries.Select(b => b.Region.Id).Any(w => w == searchId)).ToList());
                        if (foundRegions.Count > 0 && searchModel.SearchTerm.HighlightModel.Match)
                        {
                            foreach (var region in foundRegions)
                            {
                                region.CustomRegionEntries.First(a => a.Region.Id == searchId).HighlightModel.Match = true;
                            }
                        }
                    }
                    else if (searchType.Equals("Country"))
                    {
                        foundRegions.AddRange(regionList.Where(a => a.CustomRegionEntries.Select(b => b.Country.Id).Any(w => w == searchId)).ToList());
                        if (foundRegions.Count > 0 && searchModel.SearchTerm.HighlightModel.Match)
                        {
                            foreach (var region in foundRegions)
                            {
                                region.CustomRegionEntries.First(a => a.Country.Id == searchId).HighlightModel.Match = true;
                            }
                        }
                    }
                    else if (searchType.Equals("State"))
                    {
                        foundRegions.AddRange(regionList.Where(a => a.CustomRegionEntries.Select(b => b.State.Id).Any(w => w == searchId)).ToList());
                        if (foundRegions.Count > 0 && searchModel.SearchTerm.HighlightModel.Match)
                        {
                            foreach (var region in foundRegions)
                            {
                                region.CustomRegionEntries.First(a => a.State.Id == searchId).HighlightModel.Match = true;
                            }
                        }
                    }
                    else if (searchType.Equals("City"))
                    {
                        foundRegions.AddRange(regionList.Where(a => a.CustomRegionEntries.Select(b => b.City.Id).Any(w => w == searchId)).ToList());
                        if (foundRegions.Count > 0 && searchModel.SearchTerm.HighlightModel.Match)
                        {
                            foreach (var region in foundRegions)
                            {
                                region.CustomRegionEntries.First(a => a.City.Id == searchId).HighlightModel.Match = true;
                            }
                        }
                    }
                    else if (searchType.Equals("Airport"))
                    {
                        foundRegions.AddRange(regionList.Where(a => a.CustomRegionEntries.Select(b => b.Airport.Id).Any(w => w == searchId)).ToList());
                        if (foundRegions.Count > 0 && searchModel.SearchTerm.HighlightModel.Match)
                        {
                            foreach (var region in foundRegions)
                            {
                                region.CustomRegionEntries.First(a => a.Airport.Id == searchId).HighlightModel.Match = true;
                            }
                        }
                    }
                }
            }



            return foundRegions;
        }

        public List<string> SearchCustomRegions(string text)
        {
            if (string.IsNullOrEmpty(text)) return null;

            var regionListNames = GetSearchResults(text).Where(a => a.Name.StartsWith(text, StringComparison.OrdinalIgnoreCase)).Select(b => b.Name).ToList();

            return regionListNames;
        }
    }
}
