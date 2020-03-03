using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreMVC.Models.Entity
{
    public class Paging
    {
        public int PageSize { get; set; }
        public int CurrentPage { get; set; }
        public List<Filter> Filters { get; set; }
    }
}
