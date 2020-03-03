using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreMVC.Models.DL
{
    public class Mapping
    {
        public static Dictionary<string, string> TableName = new Dictionary<string, string>
        {
            { "Customer", "customers" },
            { "Employee", "employees" },
            { "Office", "office" },
            { "OrderDetail", "orderdetails" },
            { "Order", "orders" },
            { "Payments", "payments" },
            { "ProductLine", "productlines" },
            { "Product", "products" },
            { "Document", "documents" }
        };
    }
}
