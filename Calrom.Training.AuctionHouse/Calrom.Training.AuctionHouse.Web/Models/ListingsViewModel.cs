﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Calrom.Training.AuctionHouse.Web.Models
{
    public class ListingsViewModel
    {
        public List<ProductViewModel> ProductList { get; set; }
        public bool IsAuthenticated { get; set; }
    }
}