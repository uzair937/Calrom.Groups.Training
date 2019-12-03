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
            Table("dbo.REF_STA_state");
            Id(i => i.sta_id);
            References(i => i.cnt).Column("cnt_id").Cascade.All();
            Map(i => i.state_name);
            Map(i => i.display_order);
            Map(i => i.row_version);
            Map(i => i.lto_id);
        }
    }
}
