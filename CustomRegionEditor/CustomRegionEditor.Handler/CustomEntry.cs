using CustomRegionEditor.Database.Interfaces;
using CustomRegionEditor.Handler.Interfaces;
using CustomRegionEditor.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomRegionEditor.Handler
{
    public class CustomEntry : IEntryHandler
    {
        public CustomEntry (ICustomRegionEntryRepository customRegionEntryRepository)
        {
            this.CustomRegionEntryRepository = customRegionEntryRepository;
        }

        public ICustomRegionEntryRepository CustomRegionEntryRepository { get; private set; }

        public bool DeleteById(string id)
        {
            try
            {
                var entryList = this.CustomRegionEntryRepository.List();
                var customEntry = entryList.FirstOrDefault(a => a.Id == Guid.Parse(id));
                this.CustomRegionEntryRepository.Delete(customEntry);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
