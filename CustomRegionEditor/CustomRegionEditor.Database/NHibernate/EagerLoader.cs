using CustomRegionEditor.Database.Interfaces;
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
        public CustomRegionEntryModel LoadEntities(CustomRegionEntryModel oldModel)            //FIX LAZY LOADING ERROR
        {
            var newModel = new CustomRegionEntryModel
            {
                Id = oldModel.Id,
                RowVersion = oldModel.RowVersion,
                CustomRegionGroup = oldModel.CustomRegionGroup
            };
            if (oldModel == null) return newModel;
            newModel.Airport = LoadEntities(oldModel.Airport);
            newModel.Country = LoadEntities(oldModel.Country);
            newModel.Region = LoadEntities(oldModel.Region);
            newModel.State = LoadEntities(oldModel.State);
            newModel.City = LoadEntities(oldModel.City);
            return newModel;
        }
        public List<CustomRegionEntryModel> LoadEntities(List<CustomRegionEntryModel> oldModel)            //FIX LAZY LOADING ERROR
        {
            var newModel = new List<CustomRegionEntryModel>();
            if (oldModel == null) return newModel;
            foreach (var entry in oldModel)
            {
                newModel.Add(LoadEntities(entry));
            }
            return newModel;
        }
        public CustomRegionGroupModel LoadEntities(CustomRegionGroupModel oldModel)            //FIX LAZY LOADING ERROR
        {
            var newModel = new CustomRegionGroupModel { 
                Id = oldModel.Id,
                CustomRegionEntries = new List<CustomRegionEntryModel>(),
                Name = oldModel.Name,
                Description = oldModel.Description,
                System = oldModel.System, //LOADER
                RsmId = oldModel.RsmId,
                DisplayOrder = oldModel.DisplayOrder,
                RowVersion = oldModel.RowVersion
            };
            if (oldModel == null) return newModel;
            if (oldModel.CustomRegionEntries != null)
            {
                foreach (var entry in oldModel.CustomRegionEntries)
                {
                    newModel.CustomRegionEntries.Add(LoadEntities(entry));
                }
            }
            return newModel;
        }
        public List<CustomRegionGroupModel> LoadEntities(List<CustomRegionGroupModel> oldModel)            //FIX LAZY LOADING ERROR
        {
            var newModel = new List<CustomRegionGroupModel>();
            if (oldModel == null) return newModel;
            foreach (var group in oldModel)
            {
                newModel.Add(LoadEntities(group));
            }
            return newModel;
        }

        public AirportModel LoadEntities(AirportModel oldModel)            //FIX LAZY LOADING ERROR
        {
            if (oldModel == null) return null;
            var newModel = new AirportModel
            {
                AirportId = oldModel.AirportId,
                AirportName = oldModel.AirportName,
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
        public CityModel LoadEntities(CityModel oldModel)            //FIX LAZY LOADING ERROR
        {
            
            if (oldModel == null) return null;
            var newModel = new CityModel {
                CityId = oldModel.CityId,
                CityName = oldModel.CityName,
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
        public StateModel LoadEntities(StateModel oldModel)            //FIX LAZY LOADING ERROR
        {
            if (oldModel == null) return null;
            var newModel = new StateModel
            {
                StateId = oldModel.StateId,
                StateName = oldModel.StateName,
                Country = LoadEntities(oldModel.Country),
                RowVersion = oldModel.RowVersion,
                DisplayOrder = oldModel.DisplayOrder,
                LtoId = oldModel.LtoId,
                Cities = oldModel.Cities
            };
            return newModel;
        }
        public CountryModel LoadEntities(CountryModel oldModel)            //FIX LAZY LOADING ERROR
        {
            if (oldModel == null) return null;
            var newModel = new CountryModel
            {
                CountryId = oldModel.CountryId,
                CountryName = oldModel.CountryName,
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

        public RegionModel LoadEntities(RegionModel oldModel)            //FIX LAZY LOADING ERROR
        {
            if (oldModel == null) return null;
            var newModel = new RegionModel
            {
                RegionId = oldModel.RegionId,
                RegionName = oldModel.RegionName,
                RowVersion = oldModel.RowVersion,
                LtoId = oldModel.LtoId,
                Countries = oldModel.Countries
            };
            return newModel;
        }
        
        public SystemModel LoadEntities(SystemModel oldModel)            //FIX LAZY LOADING ERROR
        {
            if (oldModel == null) return null;
            var newModel = new SystemModel
            {
                SystemId = oldModel.SystemId,
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
