using CoreMVC.Models.DL;
using CoreMVC.Models.Entity;
using System.Collections.Generic;

namespace CoreMVC.Models.BL
{
    public class BLDocument : BLBase<Document>
    {
        public BLDocument()
        {
            dl = new DLDocument();
        }

        public List<Document> GetChildNodes(int parentID)
        {
            return (dl as DLDocument).GetChildNodes(parentID);
        }
    }
}
