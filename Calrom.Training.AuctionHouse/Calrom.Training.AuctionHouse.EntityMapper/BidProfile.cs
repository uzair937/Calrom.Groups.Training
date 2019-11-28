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
    public class BidProfile : Profile
    {
        public override string ProfileName => base.ProfileName;
        public BidProfile()
        {
            CreateMap<ProductModel, BidProductViewModel>()
                .ForMember(p => p.Amount, m => m.MapFrom(s => s.CurrentBid));
        }
    }
}
