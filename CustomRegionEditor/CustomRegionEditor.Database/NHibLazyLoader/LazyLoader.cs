using CustomRegionEditor.Database.Interfaces;
using CustomRegionEditor.Database.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomRegionEditor.Database.NHibLazyLoader
{
    public class LazyLoader : ILazyLoader
    {
        public CustomRegionEntryModel LoadEntities(CustomRegionEntryModel oldModel)            //FIX LAZY LOADING ERROR
        {
            var newModel = new CustomRegionEntryModel
            {
                cre_id = oldModel.cre_id,
                row_version = oldModel.row_version,
                crg = oldModel.crg
            };
            if (oldModel == null) return newModel;
            newModel.apt = LoadEntities(oldModel.apt);
            newModel.cnt = LoadEntities(oldModel.cnt);
            newModel.reg = LoadEntities(oldModel.reg);
            newModel.sta = LoadEntities(oldModel.sta);
            newModel.cty = LoadEntities(oldModel.cty);
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
                crg_id = oldModel.crg_id,
                CustomRegionEntries = new List<CustomRegionEntryModel>(),
                custom_region_name = oldModel.custom_region_name,
                custom_region_description = oldModel.custom_region_description,
                stm = oldModel.stm, //LOADER
                rsm_id = oldModel.rsm_id,
                display_order = oldModel.display_order,
                row_version = oldModel.row_version
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
                apt_id = oldModel.apt_id,
                airport_name = oldModel.airport_name,
                cty = LoadEntities(oldModel.cty),
                row_version = oldModel.row_version,
                is_main_airport = oldModel.is_main_airport,
                is_gateway_airport = oldModel.is_gateway_airport,
                gma_email_address = oldModel.gma_email_address,
                is_gma_allowed = oldModel.is_gma_allowed,
                is_group_checkin_allowed = oldModel.is_group_checkin_allowed,
                lto_id = oldModel.lto_id
            };
            return newModel;
        }
        public CityModel LoadEntities(CityModel oldModel)            //FIX LAZY LOADING ERROR
        {
            
            if (oldModel == null) return null;
            var newModel = new CityModel {
                cty_id = oldModel.cty_id,
                city_name = oldModel.city_name,
                cnt = LoadEntities(oldModel.cnt),
                row_version = oldModel.row_version,
                sta = LoadEntities(oldModel.sta),
                timezone = oldModel.timezone,
                utc_offset = oldModel.utc_offset,
                lto_id = oldModel.lto_id
            };
            return newModel;
        }
        public StateModel LoadEntities(StateModel oldModel)            //FIX LAZY LOADING ERROR
        {
            if (oldModel == null) return null;
            var newModel = new StateModel
            {
                sta_id = oldModel.sta_id,
                state_name = oldModel.state_name,
                cnt = LoadEntities(oldModel.cnt),
                row_version = oldModel.row_version,
                display_order = oldModel.display_order,
                lto_id = oldModel.lto_id
            };
            return newModel;
        }
        public CountryModel LoadEntities(CountryModel oldModel)            //FIX LAZY LOADING ERROR
        {
            if (oldModel == null) return null;
            var newModel = new CountryModel
            {
                cnt_id = oldModel.cnt_id,
                country_name = oldModel.country_name,
                reg = LoadEntities(oldModel.reg),
                iso_code = oldModel.iso_code,
                iso_number = oldModel.iso_number,
                row_version = oldModel.row_version,
                dialing_code = oldModel.dialing_code,
                lto_id = oldModel.lto_id
            };
            return newModel;
        }
        public RegionModel LoadEntities(RegionModel oldModel)            //FIX LAZY LOADING ERROR
        {
            if (oldModel == null) return null;
            var newModel = new RegionModel
            {
                reg_id = oldModel.reg_id,
                region_name = oldModel.region_name,
                row_version = oldModel.row_version,
                lto_id = oldModel.lto_id
            };
            return newModel;
        }
        
        public SystemModel LoadEntities(SystemModel oldModel)            //FIX LAZY LOADING ERROR
        {
            if (oldModel == null) return null;
            var newModel = new SystemModel
            {
                stm_id = oldModel.stm_id,
                internal_system_name = oldModel.internal_system_name,
                external_system_name = oldModel.external_system_name,
                system_description = oldModel.system_description,
                row_version = oldModel.row_version,
                comp_id = oldModel.comp_id,
                lto_id = oldModel.lto_id
            };
            return newModel;
        }
    }
}
