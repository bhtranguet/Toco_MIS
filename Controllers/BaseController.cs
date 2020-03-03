using CoreMVC.Models.BL;
using CoreMVC.Models.Entity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreMVC.Controllers
{
    public class BaseController<T> : Controller
    {
        protected BLBase<T> bl;
        public BaseController()
        {
        }

        #region Views
        public virtual IActionResult Index()
        {
            return RedirectToAction("List");
        }

        public virtual IActionResult List()
        {
            var entities = bl.GetAll();
            return View(entities);
        }

        public virtual IActionResult View(int id)
        {
            var entity = bl.GetByID(id);
            return View(entity);
        }

        public virtual IActionResult Edit(int id)
        {
            var entity = bl.GetByID(id);
            return View(entity);
        }
        public virtual IActionResult GetView(string viewName)
        {
            return View(viewName);
        }

        #endregion

        #region Functions
        // Thực hiện sửa
        [HttpPost]
        public virtual IActionResult Edit(int id, T entity)
        {
            var success = bl.Update(id, entity);
            if (success)
            {
                return RedirectToAction("List");
            }
            return View("Edit", entity);
        }

        // Thực hiện Xóa
        [HttpGet]
        public virtual IActionResult Delete(int id)
        {
            ServiceResult result = new ServiceResult();
            var success = bl.Delete(id);
            if (success > 0)
            {
                result.Success = true;
            } else
            {
                result.Success = false;
            }
            return Ok(result);
        }
        // Thực hiện thêm
        [HttpPost]
        public virtual IActionResult Insert(T entity)
        {
            var success = bl.Insert(entity);
            ServiceResult result = new ServiceResult();
            result.Success = success;
            return Json(result);
        }
        #endregion


    }
}

