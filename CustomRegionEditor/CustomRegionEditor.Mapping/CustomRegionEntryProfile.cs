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
    public class CustomRegionEntryProfile : Profile
    {
        public override string ProfileName => base.ProfileName;
        public CustomRegionEntryProfile()
        {
            CreateMap<CustomRegionEntryModel, CustomRegionViewModel>()
                .ForMember(c => c.ID, m => m.MapFrom(s => s.CreId))
                .ForMember(c => c.Region, m => m.Ignore())
                .ForMember(c => c.Country, m => m.Ignore())
                .ForMember(c => c.State, m => m.Ignore())
                .ForMember(c => c.City, m => m.Ignore())
                .ForMember(c => c.Airport, m => m.Ignore());
        }
    }
}
