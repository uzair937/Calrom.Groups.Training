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
    public class CustomRegionGroupProfile : Profile
    {
        public override string ProfileName => base.ProfileName;
        public CustomRegionGroupProfile()
        {
            CreateMap<CustomRegionGroupModel, CustomRegionGroupViewModel>()
                .ForMember(c => c.ID, m => m.MapFrom(s => s.CrgId))
                .ForMember(c => c.Name, m => m.MapFrom(s => s.CustomRegionName))
                .ForMember(c => c.Description, m => m.MapFrom(s => s.CustomRegionDescription))
                .ForMember(c => c.CustomRegions, m => m.Ignore());
        }
    }
}
