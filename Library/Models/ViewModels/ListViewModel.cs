﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Library.Models.ViewModels
{
    public class ListViewModel<T>
    {
        public List<T> Results { get; set; }
        public int TotalCount { get; set; }
    }
}
