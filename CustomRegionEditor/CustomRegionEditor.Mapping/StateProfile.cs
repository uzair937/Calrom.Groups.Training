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
    public class StateProfile : Profile
    {
        public override string ProfileName => base.ProfileName;
        public StateProfile()
        {
            CreateMap<StateModel, StateViewModel>()
                .ForMember(c => c.ID, m => m.MapFrom(s => s.Id))
                .ForMember(c => c.Name, m => m.MapFrom(s => s.Name));
        }
    }
}
