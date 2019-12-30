using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CustomRegionEditor.Database.Interfaces;
using CustomRegionEditor.Database.Models;

namespace CustomRegionEditor.Database.Repositories
{
    public class CustomRegionGroupTempRepo : ICustomRegionGroupTempRepo
    {
        private static IList<CustomRegionGroupModel> _customRegionGroupModels;

        public CustomRegionGroupTempRepo()
        {
            _customRegionGroupModels = new List<CustomRegionGroupModel>();
        }

        public void Add(CustomRegionGroupModel model)
        {
            _customRegionGroupModels.Add(model);
        }

        public void Delete(CustomRegionGroupModel model)
        {
            _customRegionGroupModels.Remove(model);
        }

        public void DestroySession()
        {
            throw new NotImplementedException();
        }

        public IList<CustomRegionGroupModel> List()
        {
            var updatedList = new List<CustomRegionGroupModel>();

            foreach (var child in _customRegionGroupModels)
            {
                updatedList.Add(child);
            }
            return updatedList;
        }

        public void Update(IList<CustomRegionEntryModel> list, string regionId)
        {
            var model = _customRegionGroupModels.FirstOrDefault(a => a.Id == Guid.Parse(regionId));
            model.CustomRegionEntries = list;
        }
    }
}
