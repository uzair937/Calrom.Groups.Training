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
    public class RegionProfile : Profile
    {
        public override string ProfileName => base.ProfileName;
        public RegionProfile()
        {
            CreateMap<Region, RegionModel>()
                .ForMember(c => c.Countries, m => m.Ignore());

            CreateMap<RegionModel, RegionViewModel>();

            CreateMap<RegionModel, Region>();
        }
    }
}
