using CustomRegionEditor.Database.Interfaces;
using CustomRegionEditor.Handler.Interfaces;
using CustomRegionEditor.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CustomRegionEditor.Handler
{
    public class SearchEntry : ISearchEntry
    {
        public SearchEntry(ICustomRegionEntryRepository customRegionEntryRepo, IModelConverter modelConverter)
        {
            this.CustomRegionEntryRepository = customRegionEntryRepo;
            this.ModelConverter = modelConverter;
        }

        private ICustomRegionEntryRepository CustomRegionEntryRepository { get; }
        private IModelConverter ModelConverter { get; }

        public CustomRegionEntryModel FindById(string id)
        {
            var foundRegion = this.CustomRegionEntryRepository.FindById(id);
            return this.ModelConverter.GetModel(foundRegion);
        }



    }
}
