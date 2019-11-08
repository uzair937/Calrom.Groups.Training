using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Calrom.Training.AuctionHouse.Database;

namespace Calrom.Training.AuctionHouse.Web.Models
{
    public class PaginationViewModel<T> : List<T>
    {
        public int CurrentPage { get; set; }
        public int Count { get; set; }
        public int PageSize { get; set; }
        public int TotalPages { get; set; }
        public List<ProductDatabaseModel> DataList { get; set; }
    }
}