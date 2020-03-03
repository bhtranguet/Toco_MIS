using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static CoreMVC.Models.Enum.Enumeration;

namespace CoreMVC.Models.Entity
{
    public class Filter
    {
        public string FieldName { get; set; }
        public Condition Condition { get; set; }
        public object Value { get; set; }

    }
}
