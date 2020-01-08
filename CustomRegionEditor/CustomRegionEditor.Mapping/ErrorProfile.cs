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
    public class ErrorProfile : Profile
    {
        public override string ProfileName => base.ProfileName;
        public ErrorProfile()
        {
            CreateMap<ErrorModel, ErrorViewModel>();
        }
    }
}
