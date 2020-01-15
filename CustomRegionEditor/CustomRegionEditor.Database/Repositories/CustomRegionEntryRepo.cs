using CustomRegionEditor.Database.Interfaces;
using CustomRegionEditor.Database.Models;
using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CustomRegionEditor.Database.Repositories
{
    internal class CustomRegionEntryRepo : ICustomRegionEntryRepository
    {
        internal CustomRegionEntryRepo(ISession session)
        {
            this.Session = session;
            _customRegionEntryList = new List<CustomRegionEntry>();
        }

        private ISession Session { get; }

        private List<CustomRegionEntry> _customRegionEntryList;

        public CustomRegionEntry AddOrUpdate(CustomRegionEntry entity)
        {
            Session.SaveOrUpdate(entity);
            Session.Flush();
            return entity;
        }

        public void Delete(List<CustomRegionEntry> entities)
        {
            foreach (var entity in entities)
            {
                Delete(entity);
            }
        }

        public void Delete(CustomRegionEntry entity)
        {
            if (entity == null) return;

            entity = Session.Get<CustomRegionEntry>(entity.Id);
            Session.Delete(entity);
            Session.Flush();
        }

        public List<CustomRegionEntry> List()
        {
            _customRegionEntryList = Session.Query<CustomRegionEntry>().ToList();

            return _customRegionEntryList;
        }

        public void DeleteById(string id)
        {
            var customRegionEntryModel = Session.Get<CustomRegionEntry>(Guid.Parse(id));

            Delete(customRegionEntryModel);
        }

        public CustomRegionEntry FindById(string entryId)
        {
            var customRegionEntryModel = Session.Get<CustomRegionEntry>(Guid.Parse(entryId));

            return customRegionEntryModel;
        }
    }
}