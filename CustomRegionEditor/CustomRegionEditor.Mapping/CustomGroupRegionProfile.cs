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
                .ForMember(c => c.Id, m => m.MapFrom(s => s.Id))
                .ForMember(c => c.Name, m => m.MapFrom(s => s.Name))
                .ForMember(c => c.Description, m => m.MapFrom(s => s.Description))
                .ForMember(c => c.CustomRegions, m => m.Ignore());
        }
    }
}
