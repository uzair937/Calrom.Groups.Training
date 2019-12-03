using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CustomRegionEditor.Database.Models;
using FluentNHibernate.Mapping;

namespace CustomRegionEditor.Database.Maps
{
    public class GlobalDistributionSystemMap : ClassMap<GlobalDistributionSystemModel>
    {
        public GlobalDistributionSystemMap()
        {
            Table("dbo.REF_ACM_global_distribution_system");
            Id(i => i.gds_id).GeneratedBy.Guid();
            Map(i => i.internal_gds_name);
            Map(i => i.external_gds_name);
            Map(i => i.gds_description);
            Map(i => i.gds_code);
            Map(i => i.row_version);
            Map(i => i.old_gds_id);
            Map(i => i.display_order);
            References(i => i.lto_id).Cascade.All();

        }
    }
}
