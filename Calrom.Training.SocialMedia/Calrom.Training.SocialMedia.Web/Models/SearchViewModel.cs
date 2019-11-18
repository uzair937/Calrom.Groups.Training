using Calrom.Training.SocialMedia.Database.Models;
using Calrom.Training.SocialMedia.Database.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Calrom.Training.SocialMedia.Web.Models
{
    public class SearchViewModel
    {
        public List<BorkViewModel> BorkResults { get; set; }
        public string SearchText { get; set; }
        public bool ValidResults { get; set; }
    }
}