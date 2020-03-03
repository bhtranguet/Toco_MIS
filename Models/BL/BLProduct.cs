using CoreMVC.Models.DL;
using CoreMVC.Models.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreMVC.Models.BL
{
    public class BLProduct : BLBase<Product>
    {
        public BLProduct()
        {
            dl = new DLProduct();
        }
    }
}
