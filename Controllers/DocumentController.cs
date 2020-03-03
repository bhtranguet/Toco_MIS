using CoreMVC.Models.BL;
using CoreMVC.Models.Entity;
using Microsoft.AspNetCore.Mvc;

namespace CoreMVC.Controllers
{
    public class DocumentController : BaseController<Document>
    {
        public DocumentController()
        {
            bl = new BLDocument();
        }

        public override IActionResult Index()
        {
            return RedirectToAction("list");
        }

        [Route("document/list/{id?}")]
        public IActionResult List(int id = 0)
        {
            var entities = (bl as BLDocument).GetChildNodes(id);
            return View(entities);
        }

        // Thực hiện Xóa
        [HttpGet]
        [Route("document/delete/{id?}")]
        public override IActionResult Delete(int id)
        {
            ServiceResult result = new ServiceResult();
            var success = bl.Delete(id);
            if (success > 0)
            {
                result.Success = true;
            }
            else
            {
                result.Success = false;
            }
            return Ok(result);
        }
    }
}