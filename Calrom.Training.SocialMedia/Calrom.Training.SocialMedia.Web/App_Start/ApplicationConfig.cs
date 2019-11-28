using Calrom.Training.SocialMedia.Database.NHibernateTools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Calrom.Training.SocialMedia.Web.App_Start
{
    public class ApplicationConfig
    {
        public static void Initialise()
        {
            log4net.Config.XmlConfigurator.Configure();
            using (var session = NHibernateHelper.OpenSession())
            {
                // Do Nothing - wrapped in using block to dispose session automatically
            }
        }
    }
}