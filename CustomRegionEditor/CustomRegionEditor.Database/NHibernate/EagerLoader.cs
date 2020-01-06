﻿using CustomRegionEditor.Database.Interfaces;
using CustomRegionEditor.Database.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomRegionEditor.Database.NHibernate
{
    public class EagerLoader : IEagerLoader
    {
        public CustomRegionEntry LoadEntities(CustomRegionEntry oldModel)            //FIX LAZY LOADING ERROR
        {
            var newModel = new CustomRegionEntry();
            if (oldModel == null) return newModel;
            newModel = new CustomRegionEntry
            {
                Id = oldModel.Id,
                RowVersion = oldModel.RowVersion,
                CustomRegionGroup = oldModel.CustomRegionGroup
            };
            
            newModel.Airport = LoadEntities(oldModel.Airport);

            newModel.Country = LoadEntities(oldModel.Country);
            newModel.Country.Cities = LoadEntities(oldModel.Country.Cities);
            newModel.Country.States = LoadEntities(oldModel.Country.States);

            newModel.Region = LoadEntities(oldModel.Region);
            newModel.Region.Countries = LoadEntities(oldModel.Region.Countries);

            newModel.State = LoadEntities(oldModel.State);
            newModel.State.Cities = LoadEntities(oldModel.State.Cities);

            newModel.City = LoadEntities(oldModel.City);
            newModel.City.Airports = LoadEntities(oldModel.City.Airports);

            return newModel;
        }
        public List<CustomRegionEntry> LoadEntities(List<CustomRegionEntry> oldModel)            //FIX LAZY LOADING ERROR
        {
            var newModel = new List<CustomRegionEntry>();
            if (oldModel == null) return newModel;
            foreach (var entry in oldModel)
            {
                newModel.Add(LoadEntities(entry));
            }
            return newModel;
        }
        public CustomRegionGroup LoadEntities(CustomRegionGroup oldModel)            //FIX LAZY LOADING ERROR
        {
            var newModel = new CustomRegionGroup();
            if (oldModel == null) return newModel;
            newModel = new CustomRegionGroup { 
                Id = oldModel.Id,
                CustomRegionEntries = new List<CustomRegionEntry>(),
                Name = oldModel.Name,
                Description = oldModel.Description,
                System = oldModel.System, //LOADER
                RsmId = oldModel.RsmId,
                DisplayOrder = oldModel.DisplayOrder,
                RowVersion = oldModel.RowVersion
            };
            
            if (oldModel.CustomRegionEntries != null)
            {
                foreach (var entry in oldModel.CustomRegionEntries)
                {
                    newModel.CustomRegionEntries.Add(LoadEntities(entry));
                }
            }
            return newModel;
        }
        public List<CustomRegionGroup> LoadEntities(List<CustomRegionGroup> oldModel)            //FIX LAZY LOADING ERROR
        {
            var newModel = new List<CustomRegionGroup>();
            if (oldModel == null) return newModel;
            foreach (var group in oldModel)
            {
                newModel.Add(LoadEntities(group));
            }
            return newModel;
        }

        public IList<Airport> LoadEntities(IList<Airport> airports)
        {
            if (airports == null) return null;
            var newList = new List<Airport>();
            foreach (var airport in airports)
            {
                newList.Add(LoadEntities(airport));
            }
            return newList;
        }

        public Airport LoadEntities(Airport oldModel)            //FIX LAZY LOADING ERROR
        {
            if (oldModel == null) return null;
            var newModel = new Airport
            {
                Id = oldModel.Id,
                Name = oldModel.Name,
                City = LoadEntities(oldModel.City),
                RowVersion = oldModel.RowVersion,
                IsMainAirport = oldModel.IsMainAirport,
                IsGatewayAirport = oldModel.IsGatewayAirport,
                GmaEmailAddress = oldModel.GmaEmailAddress,
                IsGmaAllowed = oldModel.IsGmaAllowed,
                IsGroupCheckinAllowed = oldModel.IsGroupCheckinAllowed,
                LtoId = oldModel.LtoId
            };
            return newModel;
        }

        public IList<City> LoadEntities(IList<City> cities)
        {
            if (cities == null) return null;
            var newList = new List<City>();
            foreach (var city in cities)
            {
                newList.Add(LoadEntities(city));
            }
            return newList;
        }

        public City LoadEntities(City oldModel)            //FIX LAZY LOADING ERROR
        {
            
            if (oldModel == null) return null;
            var newModel = new City {
                Id = oldModel.Id,
                Name = oldModel.Name,
                Country = LoadEntities(oldModel.Country),
                RowVersion = oldModel.RowVersion,
                State = LoadEntities(oldModel.State),
                TimeZone = oldModel.TimeZone,
                UtcOffset = oldModel.UtcOffset,
                LtoId = oldModel.LtoId,
                Airports = oldModel.Airports
            };
            return newModel;
        }

        public IList<State> LoadEntities(IList<State> states)
        {
            if (states == null) return null;
            var newList = new List<State>();
            foreach (var state in states)
            {
                newList.Add(LoadEntities(state));
            }
            return newList;
        }

        public State LoadEntities(State oldModel)            //FIX LAZY LOADING ERROR
        {
            if (oldModel == null) return null;
            var newModel = new State
            {
                Id = oldModel.Id,
                Name = oldModel.Name,
                Country = LoadEntities(oldModel.Country),
                RowVersion = oldModel.RowVersion,
                DisplayOrder = oldModel.DisplayOrder,
                LtoId = oldModel.LtoId,
                Cities = oldModel.Cities
            };
            return newModel;
        }

        public IList<Country> LoadEntities(IList<Country> countries)
        {
            if (countries == null) return null;
            var newList = new List<Country>();
            foreach (var country in countries)
            {
                newList.Add(LoadEntities(country));
            }
            return newList;
        }

        public Country LoadEntities(Country oldModel)            //FIX LAZY LOADING ERROR
        {
            if (oldModel == null) return null;
            var newModel = new Country
            {
                Id = oldModel.Id,
                Name = oldModel.Name,
                Region = LoadEntities(oldModel.Region),
                IsoCode = oldModel.IsoCode,
                IsoNumber = oldModel.IsoNumber,
                RowVersion = oldModel.RowVersion,
                DialingCode = oldModel.DialingCode,
                LtoId = oldModel.LtoId,
                Cities = oldModel.Cities,
                States = oldModel.States
            };
            return newModel;
        }

        public IList<Region> LoadEntities(IList<Region> regions)
        {
            if (regions == null) return null;
            var newList = new List<Region>();
            foreach (var region in regions)
            {
                newList.Add(LoadEntities(region));
            }
            return newList;
        }

        public Region LoadEntities(Region oldModel)            //FIX LAZY LOADING ERROR
        {
            if (oldModel == null) return null;
            var newModel = new Region
            {
                Id = oldModel.Id,
                Name = oldModel.Name,
                RowVersion = oldModel.RowVersion,
                LtoId = oldModel.LtoId,
                Countries = oldModel.Countries
            };
            return newModel;
        }
        
        public Models.System LoadEntities(Models.System oldModel)            //FIX LAZY LOADING ERROR
        {
            if (oldModel == null) return null;
            var newModel = new Models.System
            {
                Id = oldModel.Id,
                InternalSystemName = oldModel.InternalSystemName,
                ExternalSystemName = oldModel.ExternalSystemName,
                SystemDescription = oldModel.SystemDescription,
                RowVersion = oldModel.RowVersion,
                CompId = oldModel.CompId,
                LtoId = oldModel.LtoId
            };
            return newModel;
        }
    }
}
