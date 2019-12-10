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
    public class AirportProfile : Profile
    {
        public override string ProfileName => base.ProfileName;
        public AirportProfile()
        {
            CreateMap<AirportModel, AirportViewModel>()
                .ForMember(c => c.ID, m => m.MapFrom(s => s.AirportId))
                .ForMember(c => c.Name, m => m.MapFrom(s => s.AirportName));
        }
    }
}
