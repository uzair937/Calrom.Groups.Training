using AutoMapper;
using CustomRegionEditor.Database.Models;
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
            CreateMap<CityModel, CityViewModel>()
                .ForMember(c => c.ID, m => m.MapFrom(s => s.CityId))
                .ForMember(c => c.Name, m => m.MapFrom(s => s.CityName));
        }
    }
}
