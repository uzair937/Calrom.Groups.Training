using Calrom.Training.AuctionHouse.EntityMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Calrom.Training.AuctionHouse.Web.App_Start
{
    public class AutoMapperConfig
    {
        public static void Initialise()
        {
            AutoMapperConfiguration.Configure();
        }
    }
}