using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CustomRegionEditor.Database.Models;
using FluentNHibernate.Mapping;

namespace CustomRegionEditor.Database.Maps
{
    public class SystemMap : ClassMap<Models.System>
    {
        public SystemMap()
        {
            Table("dbo.REF_STM_system");
            Id(i => i.Id).Column("stm_id");
            Map(i => i.InternalSystemName).Column("internal_system_name");
            Map(i => i.ExternalSystemName).Column("external_system_name");
            Map(i => i.SystemDescription).Column("system_description");
            Map(i => i.RowVersion).Column("row_version");
            Map(i => i.CompId).Column("comp_id");
            Map(i => i.LtoId).Column("lto_id");
        }
    }
}
