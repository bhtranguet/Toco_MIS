using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreMVC.Models.Entity
{
    public class Document
    {
        /// <summary>
        /// ID Document
        /// </summary>
        public int id { get; set; }

        /// <summary>
        /// Tên tài liệu
        /// </summary>
        public string name { get; set; }

        /// <summary>
        /// Parent ID
        /// </summary>
        public int parent_id { get; set; }

        /// <summary>
        /// Tên vật lý
        /// </summary>
        public string physical_name { get; set; }

        /// <summary>
        /// Loại tài liệu: 0: folder, 1: file
        /// </summary>
        public int type { get; set; }

        /// <summary>
        /// Extention của file
        /// </summary>
        public string extension { get; set; }

    }
}
