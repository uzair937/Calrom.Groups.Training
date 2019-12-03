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
    public class CountryProfile : Profile
    {
        public override string ProfileName => base.ProfileName;
        public CountryProfile()
        {
            CreateMap<CountryModel, CountryViewModel>()
                .ForMember(c => c.ID, m => m.MapFrom(s => s.cnt_id))
                .ForMember(c => c.Name, m => m.MapFrom(s => s.country_name));
        }
    }
}
