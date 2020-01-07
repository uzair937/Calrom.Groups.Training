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
using System.Diagnostics;

namespace CustomRegionEditor.Database.Repositories
{
    public class CustomRegionGroupRepo : ICustomRegionGroupRepository
    {
        public CustomRegionGroupRepo(IEagerLoader eagerLoader, ISessionManager sessionManager)
        {
            this.EagerLoader = eagerLoader;
            this.SessionManager = sessionManager;
            _customRegionGroupList = new List<CustomRegionGroup>();
        }

        private ISessionManager SessionManager { get; }
        private IEagerLoader EagerLoader { get; }

        private List<CustomRegionGroup> _customRegionGroupList;
        public void Delete(CustomRegionGroup entity)
        {
            if (entity == null) return;

            using (var dbSession = SessionManager.OpenSession())
            {
                dbSession.Delete(entity);
                dbSession.Flush();
            }
        }

        public List<CustomRegionGroup> List()
        {
            using (var dbSession = SessionManager.OpenSession())
            {
                _customRegionGroupList = this.EagerLoader.LoadEntities(dbSession.Query<CustomRegionGroup>().ToList());
            }

            return _customRegionGroupList;
        }

        public CustomRegionGroup FindById(string id)
        {
            var customRegionGroupModel = new CustomRegionGroup();
            using (var dbSession = SessionManager.OpenSession())
            {
                var dbModel = dbSession.Get<CustomRegionGroup>(Guid.Parse(id));
                customRegionGroupModel = EagerLoader.LoadEntities(dbModel);
            }
            return customRegionGroupModel;
        }

        public CustomRegionGroup AddOrUpdate(CustomRegionGroup customRegionGroupModel)
        {
            using (var dbSession = SessionManager.OpenSession())
            {
                dbSession.SaveOrUpdate(customRegionGroupModel);

                dbSession.Flush();

                var savedGroup = EagerLoader.LoadEntities(dbSession.Query<CustomRegionGroup>().FirstOrDefault(a => a.Name == customRegionGroupModel.Name && a.Description == customRegionGroupModel.Description));
                return savedGroup;
            }
        }

        public List<string> GetNames(string type)
        {
            var names = new List<string>();
            using (var dbSession = SessionManager.OpenSession())
            {
                switch (type)
                {
                    case "airport":
                        names = dbSession.Query<Airport>().Select(a => a.Name.ToUpper()).ToList();
                        return names;
                    case "city":
                        names = dbSession.Query<City>().Select(a => a.Name.ToUpper()).ToList();
                        return names;
                    case "state":
                        names = dbSession.Query<State>().Select(a => a.Name.ToUpper()).ToList();
                        return names;
                    case "country":
                        names = dbSession.Query<Country>().Select(a => a.Name.ToUpper()).ToList();
                        return names;
                    case "region":
                        names = dbSession.Query<Region>().Select(a => a.Name.ToUpper()).ToList();
                        return names;
                    default:
                        break;
                }
            }
            return null;
        }
    }
}