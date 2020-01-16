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
    public class CustomRegionEntryProfile : Profile
    {
        public override string ProfileName => base.ProfileName;
        public CustomRegionEntryProfile()
        {
            CreateMap<CustomRegionEntryModel, CustomRegionEntryViewModel>()
                .ForMember(c => c.Region, m => m.Ignore())
                .ForMember(c => c.Country, m => m.Ignore())
                .ForMember(c => c.State, m => m.Ignore())
                .ForMember(c => c.City, m => m.Ignore())
                .ForMember(c => c.Airport, m => m.Ignore());

            CreateMap<CustomRegionEntryViewModel, CustomRegionEntryModel>();

            CreateMap<CustomRegionEntry, CustomRegionEntryModel>()
                .ForMember(c => c.Region, m => m.Ignore())
                .ForMember(c => c.Country, m => m.Ignore())
                .ForMember(c => c.State, m => m.Ignore())
                .ForMember(c => c.City, m => m.Ignore())
                .ForMember(c => c.Airport, m => m.Ignore());

            CreateMap<CustomRegionEntryModel, CustomRegionEntry>()
                .ForMember(c => c.Region, m => m.Ignore())
                .ForMember(c => c.Country, m => m.Ignore())
                .ForMember(c => c.State, m => m.Ignore())
                .ForMember(c => c.City, m => m.Ignore())
                .ForMember(c => c.Airport, m => m.Ignore());
        }
    }
}
