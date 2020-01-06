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
    public class CustomRegionGroupRepo : ICustomRegionGroupRepository
    {
        public CustomRegionGroupRepo(IEagerLoader eagerLoader, ISessionManager sessionManager)
        {
            this.EagerLoader = eagerLoader;
            this.SessionManager = sessionManager;
            _customRegionGroupList = new List<CustomRegionGroupModel>();
        }

        private ISessionManager SessionManager { get; }
        private IEagerLoader EagerLoader { get; }

        private List<CustomRegionGroupModel> _customRegionGroupList;

        public void AddOrUpdate(CustomRegionGroupModel entity)
        {
            using (var dbSession = SessionManager.OpenSession())
            {
                dbSession.SaveOrUpdate(entity);
                dbSession.Flush();
            }
        }

        public void Delete(CustomRegionGroupModel entity)
        {
            if (entity == null) return;

            using (var dbSession = SessionManager.OpenSession())
            {
                dbSession.Delete(entity);
                dbSession.Flush();
            }
        }

        public List<CustomRegionGroupModel> List()
        {
            using (var dbSession = SessionManager.OpenSession())
            {
                _customRegionGroupList = dbSession.Query<CustomRegionGroupModel>().ToList();
            }

            return _customRegionGroupList;
        }

        public List<CustomRegionGroupModel> GetSearchResults(string searchTerm, string filter)
        {
            using (var dbSession = SessionManager.OpenSession())
            {
                var startsWith = new List<CustomRegionGroupModel>();
                var contains = new List<CustomRegionGroupModel>();
                if (searchTerm.Equals("-All", StringComparison.OrdinalIgnoreCase))
                {
                    return EagerLoader.LoadEntities(dbSession.Query<CustomRegionGroupModel>().ToList());
                }

                if (searchTerm.Equals("-Small", StringComparison.OrdinalIgnoreCase))
                {
                    return EagerLoader.LoadEntities(dbSession.Query<CustomRegionGroupModel>()
                        .Where(a => a.CustomRegionEntries.Count < 25).ToList());
                }

                if (searchTerm.Equals("-Large", StringComparison.OrdinalIgnoreCase))
                {
                    return EagerLoader.LoadEntities(dbSession.Query<CustomRegionGroupModel>()
                        .Where(a => a.CustomRegionEntries.Count >= 25).ToList());
                }

                if (searchTerm.Equals("-Rand", StringComparison.OrdinalIgnoreCase))
                {
                    var returnModels = EagerLoader.LoadEntities(dbSession.Query<CustomRegionGroupModel>().ToList());
                    var rand = new Random();
                    return new List<CustomRegionGroupModel> { returnModels.ElementAt(rand.Next(returnModels.Count)) };
                }

                switch (filter)
                {
                    default:
                        startsWith = EagerLoader.LoadEntities(dbSession.Query<CustomRegionGroupModel>()
                            .Where(s => s.Name.StartsWith(searchTerm)).ToList());

                        contains = EagerLoader.LoadEntities(dbSession.Query<CustomRegionGroupModel>().Where(s =>
                            !s.Name.StartsWith(searchTerm)
                            && (s.Name.Contains(searchTerm)
                                || s.Description.Contains(searchTerm))).ToList());
                        break;
                    case ("airport"):
                        startsWith = EagerLoader.LoadEntities(dbSession.Query<CustomRegionGroupModel>().Where(s =>
                                s.CustomRegionEntries.Select(a => a.Airport.Name)
                                    .Any(w => w.StartsWith(searchTerm)))
                            .ToList());

                        contains = EagerLoader.LoadEntities(dbSession.Query<CustomRegionGroupModel>().Where(s =>
                                !s.CustomRegionEntries.Select(a => a.Airport.Name)
                                    .Any(w => w.StartsWith(searchTerm))
                                && s.CustomRegionEntries.Select(a => a.Airport.Name)
                                    .Any(w => w.Contains(searchTerm)))
                            .ToList());
                        break;
                    case ("city"):
                        startsWith = EagerLoader.LoadEntities(dbSession.Query<CustomRegionGroupModel>().Where(s =>
                                s.CustomRegionEntries.Select(a => a.City.Name).Any(w => w.StartsWith(searchTerm)))
                            .ToList());

                        contains = EagerLoader.LoadEntities(dbSession.Query<CustomRegionGroupModel>().Where(s =>
                                !s.CustomRegionEntries.Select(a => a.City.Name).Any(w => w.StartsWith(searchTerm))
                                && s.CustomRegionEntries.Select(a => a.City.Name).Any(w => w.Contains(searchTerm)))
                            .ToList());
                        break;
                    case ("state"):
                        startsWith = EagerLoader.LoadEntities(dbSession.Query<CustomRegionGroupModel>().Where(s =>
                                s.CustomRegionEntries.Select(a => a.State.Name).Any(w => w.StartsWith(searchTerm)))
                            .ToList());

                        contains = EagerLoader.LoadEntities(dbSession.Query<CustomRegionGroupModel>().Where(s =>
                                !s.CustomRegionEntries.Select(a => a.State.Name).Any(w => w.StartsWith(searchTerm))
                                && s.CustomRegionEntries.Select(a => a.State.Name).Any(w => w.Contains(searchTerm)))
                            .ToList());
                        break;
                    case ("country"):
                        startsWith = EagerLoader.LoadEntities(dbSession.Query<CustomRegionGroupModel>().Where(s =>
                                s.CustomRegionEntries.Select(a => a.Country.Name)
                                    .Any(w => w.StartsWith(searchTerm)))
                            .ToList());

                        contains = EagerLoader.LoadEntities(dbSession.Query<CustomRegionGroupModel>().Where(s =>
                                !s.CustomRegionEntries.Select(a => a.Country.Name)
                                    .Any(w => w.StartsWith(searchTerm))
                                && s.CustomRegionEntries.Select(a => a.Country.Name)
                                    .Any(w => w.Contains(searchTerm)))
                            .ToList());
                        break;
                    case ("region"):
                        startsWith = EagerLoader.LoadEntities(dbSession.Query<CustomRegionGroupModel>().Where(s =>
                                s.CustomRegionEntries.Select(a => a.Region.Name).Any(w => w.StartsWith(searchTerm)))
                            .ToList());

                        contains = EagerLoader.LoadEntities(dbSession.Query<CustomRegionGroupModel>().Where(s =>
                                !s.CustomRegionEntries.Select(a => a.Region.Name).Any(w => w.StartsWith(searchTerm))
                                && s.CustomRegionEntries.Select(a => a.Region.Name)
                                    .Any(w => w.Contains(searchTerm)))
                            .ToList());
                        break;
                }
                var returnCustomRegionGroupList = startsWith.Concat(contains).ToList();
                return returnCustomRegionGroupList;
            }
        } //looks for any matches containing the search term, orders them by relevance 

        public CustomRegionGroupModel FindById(string id)
        {
            var customRegionGroupModel = new CustomRegionGroupModel();
            using (var dbSession = SessionManager.OpenSession())
            {
                customRegionGroupModel = EagerLoader.LoadEntities(dbSession.Get<CustomRegionGroupModel>(Guid.Parse(id)));
            }
            return customRegionGroupModel;
        }

        public void DeleteById(string entryId)
        {
            var customRegionGroupModel = new CustomRegionGroupModel();
            using (var dbSession = SessionManager.OpenSession())
            {
                customRegionGroupModel = dbSession.Get<CustomRegionGroupModel>(Guid.Parse(entryId));
            }
            Delete(customRegionGroupModel);
        }

        public CustomRegionGroupModel AddNewRegion(CustomRegionGroupModel customRegionGroupModel)
        {
            using (var dbSession = SessionManager.OpenSession())
            {
                dbSession.SaveOrUpdate(customRegionGroupModel);
                dbSession.Flush();
                return EagerLoader.LoadEntities(dbSession.Query<CustomRegionGroupModel>().FirstOrDefault(a => a.Name == customRegionGroupModel.Name && a.Description == customRegionGroupModel.Description));
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
                        names = dbSession.Query<AirportModel>().Select(a => a.Name.ToUpper()).ToList();
                        return names;
                    case "city":
                        names = dbSession.Query<CityModel>().Select(a => a.Name.ToUpper()).ToList();
                        return names;
                    case "state":
                        names = dbSession.Query<StateModel>().Select(a => a.Name.ToUpper()).ToList();
                        return names;
                    case "country":
                        names = dbSession.Query<CountryModel>().Select(a => a.Name.ToUpper()).ToList();
                        return names;
                    case "region":
                        names = dbSession.Query<RegionModel>().Select(a => a.Name.ToUpper()).ToList();
                        return names;
                    default:
                        break;
                }
            }
            return null;
        }
    }
}