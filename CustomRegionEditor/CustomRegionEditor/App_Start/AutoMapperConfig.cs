using CustomRegionEditor.EntityMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CustomRegionEditor.Web.App_Start
{
    public class AutoMapperConfig
    {
        public static void Initialise()
        {
            AutoMapperConfiguration.Configure();
        }
    }
}