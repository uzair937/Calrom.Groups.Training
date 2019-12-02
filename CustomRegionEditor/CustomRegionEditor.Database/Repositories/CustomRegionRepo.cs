using CustomRegionEditor.Database.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomRegionEditor.Database
{
    public class CustomRegionRepo : IRepository<CustomRegionModel>
    {
        private static CustomRegionRepo Instance = null;

        private static readonly object padlock = new object();

        public static CustomRegionRepo GetInstance
        {
            get
            {
                if (Instance == null)
                {
                    lock (padlock)
                    {
                        if (Instance == null)
                        {
                            Instance = new CustomRegionRepo();
                        }
                    }
                }
                return Instance;
            }
        }

        private List<CustomRegionModel> _customRegionContext;

        public CustomRegionRepo()
        {
            _customRegionContext = new List<CustomRegionModel>();
        }

        public void Add(CustomRegionModel entity)
        {
            using (var dbSession = NHibernateHelper.OpenSession())
            {
                dbSession.SaveOrUpdate(entity);
                dbSession.Flush();
            }
        }

        public List<CustomRegionModel> List()
        {
            using (var dbSession = NHibernateHelper.OpenSession())
            {
                _customRegionContext = dbSession.Query<CustomRegionModel>().ToList();
            }
            return _customRegionContext;
        }

        public List<CustomRegionModel> GetSearchResults(string searchTerm)
        {
            using (var dbSession = NHibernateHelper.OpenSession())
            {
                _customRegionContext = dbSession.Query<CustomRegionModel>().Where(s => s.Abbreviation.Contains(searchTerm) || s.Name.Contains(searchTerm)).ToList();
            }
            return _customRegionContext;
        }
    }
}
