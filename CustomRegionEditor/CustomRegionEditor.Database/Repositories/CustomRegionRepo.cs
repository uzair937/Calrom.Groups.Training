using CustomRegionEditor.Database.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomRegionEditor.Database
{
    public class CustomRegionRepo : IRepository<CustomRegionGroupModel>
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

        private List<CustomRegionGroupModel> _customRegionContext;

        public CustomRegionRepo()
        {
            _customRegionContext = new List<CustomRegionGroupModel>();
        }

        public void Add(CustomRegionGroupModel entity)
        {
            using (var dbSession = NHibernateHelper.OpenSession())
            {
                dbSession.SaveOrUpdate(entity);
                dbSession.Flush();
            }
        }

        public List<CustomRegionGroupModel> List()
        {
            using (var dbSession = NHibernateHelper.OpenSession())
            {
                _customRegionContext = dbSession.Query<CustomRegionGroupModel>().ToList();
            }
            return _customRegionContext;
        }

        public List<CustomRegionGroupModel> GetSearchResults(string searchTerm)
        {
            using (var dbSession = NHibernateHelper.OpenSession())
            {
                _customRegionContext = dbSession.Query<CustomRegionGroupModel>().Where(s => s.Abbreviation.Contains(searchTerm) || s.Name.Contains(searchTerm)).ToList();
            }
            return _customRegionContext;
        }
    }
}
