using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CustomRegionEditor.Database.Models;
using FluentNHibernate.Mapping;

namespace CustomRegionEditor.Database.Maps
{
    public class StateMap : ClassMap<StateModel>
    {
        public StateMap()
        {
            Table("dbo.REF_ACM_state");
            Id(i => i.sta_id).GeneratedBy.Guid();
            HasOne(i => i.cnt).Cascade.All();
            Map(i => i.state_name);
            Map(i => i.display_order);
            Map(i => i.row_version);
            References(i => i.lto_id).Cascade.All();
        }
    }
}
