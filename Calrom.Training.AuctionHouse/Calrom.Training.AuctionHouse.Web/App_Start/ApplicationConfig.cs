using Calrom.Training.AuctionHouse.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Calrom.Training.AuctionHouse.Web.App_Start
{
    public class ApplicationConfig
    {
        public static void Initialise()
        {
            log4net.Config.XmlConfigurator.Configure();
            ////////Configure database context and Load default data if any
            using (var dbSession = NHibernateHelper.OpenSession())
            {
                // Do Nothing - wrapped in using block to dispose session automatically
            }
        }
    }
}