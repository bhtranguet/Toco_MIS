using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using CoreMVC.Models.BL;
using CoreMVC.Models.Entity;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;

namespace CoreMVC.Controllers
{
    public class FileHandleController : Controller
    {
        private IHostingEnvironment _hostingEnvironment;
        public FileHandleController(IHostingEnvironment environment)
        {
            _hostingEnvironment = environment;
        }
        public IActionResult UploadFile()
        {
            ServiceResult result = new ServiceResult();
            var files = Request.Form.Files;
            var parentID = Int32.Parse(Request.Form["parentID"]);
            var count = files.Count;
            foreach (var file in files)
            {
                var fileExtension = file.FileName.Split(".").Last();
                var fileName = file.FileName.Substring(0, file.FileName.Length - fileExtension.Length - 1);
                var physicalName = Guid.NewGuid();
                var filePath = Path.Combine(_hostingEnvironment.WebRootPath, "upload" ,$"{physicalName}.{fileExtension}");
                Document document = new Document()
                {
                    name = file.FileName,
                    parent_id = parentID,
                    physical_name = physicalName.ToString(),
                    extension = fileExtension,
                    type = 1
                };

                try
                {
                    using (var stream = System.IO.File.Create(filePath))
                    {
                        file.CopyTo(stream);
                        var blDocument = new BLDocument();
                        blDocument.Insert(document);
                    }
                }
                catch (Exception)
                {
                    count--;
                }
                
            }
            // Trả về số file upload thành công
            return Ok(count);
        }
        public IActionResult Dowload(int id)
        {
            var blDocument = new BLDocument();
            var document = blDocument.GetByID(id);
            var filePath = Path.Combine(_hostingEnvironment.WebRootPath, "upload", $"{document.physical_name}.{document.extension}");
            string contentType;
            new FileExtensionContentTypeProvider().TryGetContentType(document.name, out contentType);
            return PhysicalFile(filePath, contentType, document.name);
        }
    }
}