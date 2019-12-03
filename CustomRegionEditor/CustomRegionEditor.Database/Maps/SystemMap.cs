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
            Table("dbo.REF_ACM_system");
            Id(i => i.stm_id).GeneratedBy.Guid();
            Map(i => i.internal_system_name);
            Map(i => i.external_system_name);
            Map(i => i.system_description);
            Map(i => i.row_version);
            Map(i => i.comp_id);
            References(i => i.lto_id).Cascade.All();
        }
    }
}
