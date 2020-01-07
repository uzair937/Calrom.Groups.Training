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
    public class CustomRegionGroupProfile : Profile
    {
        public override string ProfileName => base.ProfileName;
        public CustomRegionGroupProfile()
        {
            CreateMap<CustomRegionGroupModel, CustomRegionGroupViewModel>();

            CreateMap<CustomRegionGroup, CustomRegionGroupModel>();

            CreateMap<CustomRegionGroupModel, CustomRegionGroup>();
        }
    }
}
