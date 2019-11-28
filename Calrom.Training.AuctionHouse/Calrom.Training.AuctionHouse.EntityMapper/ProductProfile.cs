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
    public class ProductProfile : Profile
    {
        public override string ProfileName => base.ProfileName;
        public ProductProfile()
        {
            CreateMap<ProductModel, ProductViewModel>()
                .ForMember(p => p.ItemPrice, m => m.MapFrom(s => s.CurrentBid));

            CreateMap<ProductViewModel, ProductModel>()
                .ForMember(p => p.CurrentBid, m => m.MapFrom(s => s.ItemPrice));

            CreateMap<ProductModel, IndividualProductViewModel>()
                .ForMember(p => p.ItemPrice, m => m.MapFrom(s => s.CurrentBid));
        }
    }
}
