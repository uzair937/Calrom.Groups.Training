using Calrom.Training.SocialMedia.Mapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Calrom.Training.SocialMedia.Web.App_Start
{
    public class AutoMapperConfig
    {
        public static void Initialise()
        {
            AutoMapperConfiguration.Configure();
        }
    }
}