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

        public List<CustomRegionGroupModel> GetSearchResults(string searchTerm, string filter)
        {
            var dbList = this.CustomRegionGroupRepository.List();
            var regionList = this.ModelConverter.GetModel(dbList);
            var startsWith = new List<CustomRegionGroupModel>();
            var contains = new List<CustomRegionGroupModel>();
            var ids = new List<CustomRegionGroupModel>();
            if (searchTerm.Equals("-All", StringComparison.OrdinalIgnoreCase))
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

            switch (filter)
            {
                default:
                    startsWith = regionList.Where(s => s.Name.StartsWith(searchTerm, StringComparison.OrdinalIgnoreCase)).ToList();

                    contains = regionList.Where(s =>
                        !s.Name.StartsWith(searchTerm, StringComparison.OrdinalIgnoreCase)
                        && (s.Name.Contains(searchTerm)
                            || s.Description.Contains(searchTerm))
                            || s.CustomRegionEntries.Select(a => a.Country.Name)
                               .Any(w => w.StartsWith(searchTerm, StringComparison.OrdinalIgnoreCase))
                               || s.CustomRegionEntries.Select(a => a.State.Name)
                               .Any(w => w.StartsWith(searchTerm, StringComparison.OrdinalIgnoreCase))
                               || s.CustomRegionEntries.Select(a => a.City.Name)
                               .Any(w => w.StartsWith(searchTerm, StringComparison.OrdinalIgnoreCase))
                               || s.CustomRegionEntries.Select(a => a.Airport.Name)
                               .Any(w => w.StartsWith(searchTerm, StringComparison.OrdinalIgnoreCase))
                               ).ToList();

                    //ids = regionList.Where(s =>
                    //!s.Name.StartsWith(searchTerm, StringComparison.OrdinalIgnoreCase)
                    //    || s.CustomRegionEntries.Select(b => b.Country.Id)
                    //    .Any(w => w.Contains(searchTerm))
                    //    || s.CustomRegionEntries.Select(b => b.State.Id)
                    //    .Any(w => w.Contains(searchTerm))
                    //    || s.CustomRegionEntries.Select(b => b.City.Id)
                    //    .Any(w => w.Contains(searchTerm))
                    //    || s.CustomRegionEntries.Select(b => b.Airport.Id)
                    //    .Any(w => w.Contains(searchTerm))
                    //).ToList();
                    break;
                case ("airport"):
                    startsWith = regionList.Where(s =>
                            s.CustomRegionEntries.Select(a => a.Airport.Name)
                                .Any(w => w.StartsWith(searchTerm, StringComparison.OrdinalIgnoreCase)))
                        .ToList();

                    contains = regionList.Where(s =>
                            !s.CustomRegionEntries.Select(a => a.Airport.Name)
                                .Any(w => w.StartsWith(searchTerm, StringComparison.OrdinalIgnoreCase))
                            && s.CustomRegionEntries.Select(a => a.Airport.Name)
                                .Any(w => w.Contains(searchTerm)))
                        .ToList();
                    break;
                case ("city"):
                    startsWith = regionList.Where(s =>
                            s.CustomRegionEntries.Select(a => a.City.Name)
                            .Any(w => w.StartsWith(searchTerm, StringComparison.OrdinalIgnoreCase)))
                        .ToList();

                    contains = regionList.Where(s =>
                            !s.CustomRegionEntries.Select(a => a.City.Name)
                            .Any(w => w.StartsWith(searchTerm, StringComparison.OrdinalIgnoreCase))
                            && s.CustomRegionEntries.Select(a => a.City.Name)
                            .Any(w => w.Contains(searchTerm)))
                        .ToList();
                    break;
                case ("state"):
                    startsWith = regionList.Where(s =>
                            s.CustomRegionEntries.Select(a => a.State.Name)
                            .Any(w => w.StartsWith(searchTerm, StringComparison.OrdinalIgnoreCase)))
                        .ToList();

                    contains = regionList.Where(s =>
                            !s.CustomRegionEntries.Select(a => a.State.Name)
                            .Any(w => w.StartsWith(searchTerm, StringComparison.OrdinalIgnoreCase))
                            && s.CustomRegionEntries.Select(a => a.State.Name)
                            .Any(w => w.Contains(searchTerm)))
                        .ToList();
                    break;
                case ("country"):
                    startsWith = regionList.Where(s =>
                            s.CustomRegionEntries.Select(a => a.Country.Name)
                                .Any(w => w.StartsWith(searchTerm, StringComparison.OrdinalIgnoreCase)))
                        .ToList();

                    contains = regionList.Where(s =>
                            !s.CustomRegionEntries.Select(a => a.Country.Name)
                                .Any(w => w.StartsWith(searchTerm, StringComparison.OrdinalIgnoreCase))
                            && s.CustomRegionEntries.Select(a => a.Country.Name)
                                .Any(w => w.Contains(searchTerm)))
                        .ToList();
                    break;
                case ("region"):
                    startsWith = regionList.Where(s =>
                            s.CustomRegionEntries.Select(a => a.Region.Name)
                            .Any(w => w.StartsWith(searchTerm, StringComparison.OrdinalIgnoreCase)))
                        .ToList();

                    contains = regionList.Where(s =>
                            !s.CustomRegionEntries.Select(a => a.Region.Name)
                            .Any(w => w.StartsWith(searchTerm, StringComparison.OrdinalIgnoreCase))
                            && s.CustomRegionEntries.Select(a => a.Region.Name)
                                .Any(w => w.Contains(searchTerm)))
                        .ToList();
                    break;
            }
            var returnCustomRegionGroupList = startsWith.Concat(contains).ToList();
            if (ids.Count > 0)
            {
                returnCustomRegionGroupList.Concat(ids);
            }
            return returnCustomRegionGroupList;

        } //looks for any matches containing the search term, orders them by relevance 

    }
}
