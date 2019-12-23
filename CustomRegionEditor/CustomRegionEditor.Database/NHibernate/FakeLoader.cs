using CustomRegionEditor.Database.Interfaces;
using CustomRegionEditor.Database.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomRegionEditor.Database.NHibernate
{
    public class FakeLoader : IEagerLoader
    {
        public CustomRegionEntryModel LoadEntities(CustomRegionEntryModel oldModel)            //FIX LAZY LOADING ERROR
        {
            return oldModel;
        }
        public List<CustomRegionEntryModel> LoadEntities(List<CustomRegionEntryModel> oldModel)            //FIX LAZY LOADING ERROR
        {
            return oldModel;
        }
        public CustomRegionGroupModel LoadEntities(CustomRegionGroupModel oldModel)            //FIX LAZY LOADING ERROR
        {
            return oldModel;
        }
        public List<CustomRegionGroupModel> LoadEntities(List<CustomRegionGroupModel> oldModel)            //FIX LAZY LOADING ERROR
        {
            return oldModel;
        }

        public AirportModel LoadEntities(AirportModel oldModel)            //FIX LAZY LOADING ERROR
        {
            return oldModel;
        }
        public CityModel LoadEntities(CityModel oldModel)            //FIX LAZY LOADING ERROR
        {
            return oldModel;
        }
        public StateModel LoadEntities(StateModel oldModel)            //FIX LAZY LOADING ERROR
        {
            return oldModel;
        }
        public CountryModel LoadEntities(CountryModel oldModel)            //FIX LAZY LOADING ERROR
        {
            return oldModel;
        }

        public RegionModel LoadEntities(RegionModel oldModel)            //FIX LAZY LOADING ERROR
        {
            return oldModel;
        }
        
        public SystemModel LoadEntities(SystemModel oldModel)            //FIX LAZY LOADING ERROR
        {
            return oldModel;
        }
    }
}
