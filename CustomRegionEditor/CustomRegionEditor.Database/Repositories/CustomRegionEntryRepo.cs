using CustomRegionEditor.Database.Models;
using CustomRegionEditor.Database.Repositories;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentNHibernate.Utils;
using CustomRegionEditor.Database.NHibLazyLoader;
using CustomRegionEditor.Database.Interfaces;

namespace CustomRegionEditor.Database.Repositories
{
    public class CustomRegionEntryRepo : ICustomRegionEntryRepository
    {
        public CustomRegionEntryRepo(ILazyLoader lazyLoader, ISessionManager iNHibernateHelper)
        {
            this.ILazyLoader = lazyLoader;
            this.INHibernateHelper = iNHibernateHelper;
            _customRegionEntryList = new List<CustomRegionEntryModel>();
        }

        public ISessionManager INHibernateHelper { get; }

        public ILazyLoader ILazyLoader { get; }

        private List<CustomRegionEntryModel> _customRegionEntryList;

        public void AddOrUpdate(CustomRegionEntryModel entity)
        {
            using (var dbSession = INHibernateHelper.OpenSession())
            {
                dbSession.SaveOrUpdate(entity);
                dbSession.Flush();
            }
        }

        public void Delete(List<CustomRegionEntryModel> entities)
        {
            foreach (var entity in entities)
            {
                Delete(entity);
            }
        }

        public void Delete(CustomRegionEntryModel entity)
        {
            if (entity == null) return;

            using (var dbSession = INHibernateHelper.OpenSession())
            {
                entity = dbSession.Get<CustomRegionEntryModel>(entity.CreId);
                dbSession.Delete(entity);
                dbSession.Flush();
            }
        }

        public List<CustomRegionEntryModel> List()
        {
            using (var dbSession = INHibernateHelper.OpenSession())
            {
                _customRegionEntryList = dbSession.Query<CustomRegionEntryModel>().ToList();
            }

            return _customRegionEntryList;
        }

        public void DeleteById(string id)
        {
            var customRegionEntryModel = new CustomRegionEntryModel();
            using (var dbSession = INHibernateHelper.OpenSession())
            {
                customRegionEntryModel = dbSession.Get<CustomRegionEntryModel>(Guid.Parse(id));
            }
            Delete(customRegionEntryModel);
        }

        public CustomRegionEntryModel FindById(string entryId)
        {
            using (var dbSession = INHibernateHelper.OpenSession())
            {
                var customRegionEntryModel = dbSession.Get<CustomRegionEntryModel>(Guid.Parse(entryId));
                return ILazyLoader.LoadEntities(customRegionEntryModel);
            }
        }
    }
}