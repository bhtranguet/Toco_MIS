using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreMVC.Models.Entity
{
    public class ServiceResult
    {
        public bool Success { get; set; }
        public int Code { get; set; }
        public object Data { get; set; }
    }
}
