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
            Table("dbo.REF_GDS_global_distribution_system");
            Id(i => i.GdsId).GeneratedBy.Guid();
            Map(i => i.InternalGdsName);
            Map(i => i.ExternalGdsName);
            Map(i => i.GdsDescription);
            Map(i => i.GdsCode);
            Map(i => i.RowVersion);
            Map(i => i.OldGdsId);
            Map(i => i.DisplayOrder);
            Map(i => i.LtoId);

        }
    }
}
