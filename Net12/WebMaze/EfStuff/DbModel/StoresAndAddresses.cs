﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebMaze.EfStuff.DbModel
{
    public class StoresAndAddresses
    {
        public long Id { get; set; }
        public string ShopName { get; set; }
        public string AdressOfShop { get; set; }
    }
}