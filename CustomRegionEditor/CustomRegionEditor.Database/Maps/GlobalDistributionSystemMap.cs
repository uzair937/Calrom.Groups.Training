using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CustomRegionEditor.Database.Models;
using FluentNHibernate.Mapping;

namespace CustomRegionEditor.Database.Maps
{
    public class GlobalDistributionSystemMap : ClassMap<GlobalDistributionSystem>
    {
        public GlobalDistributionSystemMap()
        {
            Table("dbo.REF_GDS_global_distribution_system");
            Id(i => i.Id).Column("gds_id").GeneratedBy.Guid();
            Map(i => i.InternalGdsName).Column("internal_gds_nane");
            Map(i => i.ExternalGdsName).Column("external_gds_name");
            Map(i => i.GdsDescription).Column("gds_description");
            Map(i => i.GdsCode).Column("gds_code");
            Map(i => i.RowVersion).Column("row_version");
            Map(i => i.OldGdsId).Column("old_gds_id");
            Map(i => i.DisplayOrder).Column("display_order");
            Map(i => i.LtoId).Column("lto_id");

        }
    }
}
