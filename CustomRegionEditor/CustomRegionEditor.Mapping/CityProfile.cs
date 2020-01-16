using AutoMapper;
using CustomRegionEditor.Database.Models;
using CustomRegionEditor.Models;
using CustomRegionEditor.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomRegionEditor.EntityMapper
{
    public class CityProfile : Profile
    {
        public override string ProfileName => base.ProfileName;
        public CityProfile()
        {
            CreateMap<CityModel, City>();

            CreateMap<CityModel, CityViewModel>();

            CreateMap<CityViewModel, CityModel>();

            CreateMap<City, CityModel>()
                .ForMember(c => c.Airports, m => m.Ignore());
        }
    }
}
