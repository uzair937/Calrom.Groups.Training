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
            Id(i => i.StateId).Column("sta_id");
            References(i => i.Country).Column("cnt_id").Cascade.All();
            Map(i => i.StateName).Column("state_name");
            Map(i => i.DisplayOrder).Column("display_order");
            Map(i => i.RowVersion).Column("row_version");
            Map(i => i.LtoId).Column("lto_id");

            HasMany(i => i.Cities).Cascade.All().Inverse();
        }
    }
}
