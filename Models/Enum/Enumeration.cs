using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreMVC.Models.Enum
{
    public class Enumeration
    {
        public enum Condition
        {
            Equal = 0,
            NotEqual = 2,
            GreaterThan = 3,
            LessThan = 4,
            GreaterThanOrEqual = 5,
            LessThanOrEqual = 6,
            Contain = 7,
            NotContain = 8,
            StartWith = 9,
            EndWith = 10
        }
    }
}
