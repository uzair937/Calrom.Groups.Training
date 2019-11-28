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
    public class UserProfile : Profile
    {
        public override string ProfileName => "UserProfile";

        public UserProfile()
        {
            CreateMap<UserModel, UserViewModel>()
                .ForMember(d => d.UserId, m => m.MapFrom(s => s.UserId))
                .ForMember(d => d.UserName, m => m.MapFrom(s => s.UserName))
                .ForMember(d => d.Password, m => m.MapFrom(s => s.Password))
                .ForMember(d => d.UserBorks, m => m.Ignore())
                .ForMember(d => d.UserPP, m => m.MapFrom(s => s.UserPP))
                .ForMember(d => d.FollowerId, m => m.MapFrom(s => s.Followers.Select(a => a.FollowerId)))
                .ForMember(d => d.FollowingId, m => m.MapFrom(s => s.Following.Select(a => a.FollowingId)))
                .ForMember(d => d.Notifications, m => m.Ignore());
                
        }
        //protected override Configure
    }
}
