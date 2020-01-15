using CustomRegionEditor.Database.Interfaces;
using CustomRegionEditor.Handler.Interfaces;
using CustomRegionEditor.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CustomRegionEditor.Handler
{
    public class SearchRegion : ISearchRegion
    {
        public SearchRegion(ICustomRegionGroupRepository customRegionGroupRepo, IModelConverter modelConverter)
        {
            this.CustomRegionGroupRepository = customRegionGroupRepo;
            this.ModelConverter = modelConverter;
        }

        private ICustomRegionGroupRepository CustomRegionGroupRepository { get; }
        private IModelConverter ModelConverter { get; }

        public bool CheckValidName(string name)
        {
            var validName = true;
            var regionList = this.CustomRegionGroupRepository.List();
            if (regionList.Select(a => a.Name).Contains(name))
            {
                validName = false;
            }
            return validName;
        }

        public CustomRegionGroupModel FindById(string id)
        {
            var foundRegion = this.CustomRegionGroupRepository.FindById(id);
            var modelRegion = this.ModelConverter.GetModel(foundRegion);
            return modelRegion;
        }

        public List<CustomRegionGroupModel> GetSearchResults(string searchTerm)
        {
            var dbList = this.CustomRegionGroupRepository.List();
            var regionList = this.ModelConverter.GetModel(dbList);
            var startsWith = new List<CustomRegionGroupModel>();
            var contains = new List<CustomRegionGroupModel>();
            var ids = new List<CustomRegionGroupModel>();
            if (searchTerm.Equals("-All", StringComparison.OrdinalIgnoreCase)
                || searchTerm.Equals("none", StringComparison.OrdinalIgnoreCase))
            {
                return regionList;
            }

            if (searchTerm.Equals("-Small", StringComparison.OrdinalIgnoreCase))
            {
                return regionList.Where(a => a.CustomRegionEntries.Count < 25).ToList();
            }

            if (searchTerm.Equals("-Large", StringComparison.OrdinalIgnoreCase))
            {
                return regionList.Where(a => a.CustomRegionEntries.Count >= 25).ToList();
            }


            startsWith = regionList.Where(s => s.Name.StartsWith(searchTerm, StringComparison.OrdinalIgnoreCase)).ToList();

            contains = regionList.Where(s =>
                !s.Name.StartsWith(searchTerm, StringComparison.OrdinalIgnoreCase)
                && (s.Name.Contains(searchTerm)
                    || (!string.IsNullOrEmpty(s.Description) && s.Description.Contains(searchTerm)))
                    || s.CustomRegionEntries.Select(a => a.Name)
                       .Any(w => w.StartsWith(searchTerm, StringComparison.OrdinalIgnoreCase))).ToList();


            searchTerm = searchTerm.ToUpper();

            ids = regionList.Where(s => s.CustomRegionEntries.Select(b => b.Value)
                .Any(w => w == searchTerm)).ToList();

            ids = ids.Where(w => !contains.Select(a => a.CustomRegionEntries).Contains(w.CustomRegionEntries)
            && !startsWith.Select(a => a.CustomRegionEntries).Contains(w.CustomRegionEntries)).ToList();

            //var subResults = SearchSubRegions(searchTerm);

            var returnCustomRegionGroupList = startsWith.Concat(contains).ToList();
            returnCustomRegionGroupList = returnCustomRegionGroupList.Concat(ids).ToList();

            returnCustomRegionGroupList = returnCustomRegionGroupList.GroupBy(a => a.Id).Select(b => b.First()).ToList();

            return returnCustomRegionGroupList;

        } //looks for any matches containing the search term, orders them by relevance 

        private List<CustomRegionGroupModel> SearchSubRegions(string searchTerm, List<CustomRegionGroupModel> regionList)
        {
            var results = regionList.Where(s => s.CustomRegionEntries.Select(b => b.Value)
                .Any(w => w == searchTerm)).ToList();

            return null;
            //results = results.Where(w => !contains.Select(a => a.CustomRegionEntries).Contains(w.CustomRegionEntries)
            //&& !startsWith.Select(a => a.CustomRegionEntries).Contains(w.CustomRegionEntries)).ToList();
        }

        public List<string> SearchCustomRegions(string text)
        {
            if (string.IsNullOrEmpty(text)) return null;

            var regionListNames = GetSearchResults(text).Where(a => a.Name.StartsWith(text, StringComparison.OrdinalIgnoreCase)).Select(b => b.Name).ToList();

            return regionListNames;
        }
    }
}
