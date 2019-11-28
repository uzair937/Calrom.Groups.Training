using AutoMapper;
using Calrom.Training.SocialMedia.Database.ORMModels;
using Calrom.Training.SocialMedia.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calrom.Training.SocialMedia.Mapper
{
    public class BorkProfile : Profile
    {
        public override string ProfileName => "BorkProfile";

        public BorkProfile()
        {
            CreateMap<BorkModel, BorkViewModel>()
                .ForMember(d => d.BorkText, m => m.MapFrom(s => s.BorkText))
                .ForMember(d => d.DateBorked, m => m.MapFrom(s => s.DateBorked))
                .ForMember(d => d.UserId, m => m.MapFrom(s => s.UserModel.UserId))
                .ForMember(d => d.UserName, m => m.MapFrom(s => s.UserModel.UserName))
                .ForMember(d => d.UserPP, m => m.MapFrom(s => s.UserModel.UserPP));
                
        }
        //protected override Configure
    }
}
