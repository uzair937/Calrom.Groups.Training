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
            Id(i => i.StateId);
            References(i => i.Country).Column("CountryId").Cascade.All();
            Map(i => i.StateName);
            Map(i => i.DisplayOrder);
            Map(i => i.RowVersion);
            Map(i => i.LtoId);
        }
    }
}
