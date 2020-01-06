using CustomRegionEditor.Database.Models;
using CustomRegionEditor.Database.Repositories;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentNHibernate.Utils;
using CustomRegionEditor.Database.NHibernate;
using CustomRegionEditor.Database.Interfaces;

namespace CustomRegionEditor.Database.Repositories
{
    public class CustomRegionEntryRepo : ICustomRegionEntryRepository
    {
        public CustomRegionEntryRepo(IEagerLoader eagerLoader, ISessionManager iNHibernateHelper)
        {
            this.IEagerLoader = eagerLoader;
            this.INHibernateHelper = iNHibernateHelper;
            _customRegionEntryList = new List<CustomRegionEntry>();
        }

        public ISessionManager INHibernateHelper { get; }

        public IEagerLoader IEagerLoader { get; }

        private List<CustomRegionEntry> _customRegionEntryList;

        public CustomRegionEntry AddOrUpdate(CustomRegionEntry entity)
        {
            using (var dbSession = INHibernateHelper.OpenSession())
            {
                dbSession.SaveOrUpdate(entity);
                dbSession.Flush();
                return entity;
            }
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

            using (var dbSession = INHibernateHelper.OpenSession())
            {
                entity = dbSession.Get<CustomRegionEntry>(entity.Id);
                dbSession.Delete(entity);
                dbSession.Flush();
            }
        }

        public List<CustomRegionEntry> List()
        {
            using (var dbSession = INHibernateHelper.OpenSession())
            {
                _customRegionEntryList = dbSession.Query<CustomRegionEntry>().ToList();
            }

            return _customRegionEntryList;
        }

        public void DeleteById(string id)
        {
            var customRegionEntryModel = new CustomRegionEntry();
            using (var dbSession = INHibernateHelper.OpenSession())
            {
                customRegionEntryModel = dbSession.Get<CustomRegionEntry>(Guid.Parse(id));
            }
            Delete(customRegionEntryModel);
        }

        public CustomRegionEntry FindById(string entryId)
        {
            using (var dbSession = INHibernateHelper.OpenSession())
            {
                var customRegionEntryModel = dbSession.Get<CustomRegionEntry>(Guid.Parse(entryId));
                return IEagerLoader.LoadEntities(customRegionEntryModel);
            }
        }
    }
}