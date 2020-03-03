using CoreMVC.Models.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreMVC.Models.DL
{
    public class DLDocument : DLBase<Document>
    {
        public List<Document> GetChildNodes(int parentID)
        {
            List<Document> documents = new List<Document>();
            string query = "select * from documents where parent_id = @parent_id";
            var param = new Dictionary<string, object>() { { "@parent_id", parentID } };
            documents = Query(query, param);
            return documents;
        }
    }
}
