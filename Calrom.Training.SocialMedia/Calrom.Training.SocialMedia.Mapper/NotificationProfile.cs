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
    public class NotificationProfile : Profile
    {
        public override string ProfileName => "NotificationProfile";

        public NotificationProfile()
        {
            CreateMap<NotificationModel, NotificationViewModel>()
                .ForMember(d => d.UserId, m => m.MapFrom(s => s.UserId))
                .ForMember(d => d.Username, m => m.MapFrom(s => s.UserModel.UserId))
                .ForMember(d => d.Text, m => m.MapFrom(s => s.Text))
                .ForMember(d => d.Type, m => m.MapFrom(s => (int)s.Type))
                .ForMember(d => d.LikedBork, m => m.Ignore())
                .ForMember(d => d.UserPP, m => m.MapFrom(s => s.UserModel.UserPP))
                .ForMember(d => d.DateCreated, m => m.MapFrom(s => s.DateCreated));
                
        }
        //protected override Configure
    }
}
