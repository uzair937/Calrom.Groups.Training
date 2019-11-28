using AutoMapper;
using Calrom.Training.AuctionHouse.Database;
using Calrom.Training.AuctionHouse.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calrom.Training.AuctionHouse.EntityMapper
{
    public class UserProfile : Profile
    {
        public override string ProfileName => base.ProfileName;
        public UserProfile()
        {
            CreateMap<UserModel, AccountViewModel>();
        }
    }
}
