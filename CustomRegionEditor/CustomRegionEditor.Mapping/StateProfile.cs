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
    public class StateProfile : Profile
    {
        public override string ProfileName => base.ProfileName;
        public StateProfile()
        {
            CreateMap<State, StateModel>()
                .ForMember(c => c.Cities, m => m.Ignore());

            CreateMap<StateModel, StateViewModel>();

            CreateMap<StateModel, State>();
        }
    }
}
