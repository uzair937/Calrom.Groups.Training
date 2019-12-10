using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CustomRegionEditor.Database.Models;
using FluentNHibernate.Mapping;

namespace CustomRegionEditor.Database.Maps
{
    public class SystemMap : ClassMap<SystemModel>
    {
        public SystemMap()
        {
            Table("dbo.REF_STM_system");
            Id(i => i.SystemId);
            Map(i => i.InternalSystemName);
            Map(i => i.ExternalSystemName);
            Map(i => i.SystemDescription);
            Map(i => i.RowVersion);
            Map(i => i.CompId);
            Map(i => i.LtoId);
        }
    }
}
