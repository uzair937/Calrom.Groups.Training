using CustomRegionEditor.Database.Interfaces;
using CustomRegionEditor.Database.Models;
using System;
using NHibernate;
using System.Collections.Generic;
using System.Linq;

namespace CustomRegionEditor.Database.Repositories
{
    internal class CustomRegionGroupRepo : ICustomRegionGroupRepository
    {
        internal CustomRegionGroupRepo(ISession session)
        {
            this.Session = session;
            _customRegionGroupList = new List<CustomRegionGroup>();
        }

        private ISession Session { get; }

        private List<CustomRegionGroup> _customRegionGroupList;
        public void Delete(CustomRegionGroup entity)
        {
            if (entity == null) return;

            this.Session.Delete(entity);
            this.Session.Flush();
        }

        public List<CustomRegionGroup> List()
        {
            _customRegionGroupList = Session.Query<CustomRegionGroup>().ToList();

            return _customRegionGroupList;
        }

        public CustomRegionGroup FindById(string id)
        {
            var dbModel = Session.Get<CustomRegionGroup>(Guid.Parse(id));

            var customRegionGroupModel = dbModel;

            return customRegionGroupModel;
        }

        public CustomRegionGroup AddOrUpdate(CustomRegionGroup customRegionGroupModel)
        {
            Session.SaveOrUpdate(customRegionGroupModel);

            Session.Flush();

            var savedGroup = Session.Query<CustomRegionGroup>().FirstOrDefault(a => a.Name == customRegionGroupModel.Name && a.Description == customRegionGroupModel.Description);
            return savedGroup;
        }
    }
}