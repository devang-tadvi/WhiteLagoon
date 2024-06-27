using Microsoft.AspNetCore.Mvc;
using WhiteLagoon.Application.Common.Interfaces;
using WhiteLagoon.Domain.Entities;
using WhiteLagoon.Infrastructure.Data;
using WhiteLagoon.Infrastructure.Repository;

namespace WhiteLagoon.Web.Controllers
{
    public class VillaController : Controller
    {
        private readonly IUnitOfWork _unitofwork;

        public VillaController(IUnitOfWork unitofwork)
        {
            _unitofwork = unitofwork;
        }
        public IActionResult Index()
        {
            var villas = _unitofwork.villa.GetAll();
            return View(villas);
        }
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Villa obj)
        {
            if (ModelState.IsValid)
            {
				_unitofwork.villa.Add(obj);
				_unitofwork.Save();
                TempData["success"] = "The villa has been created successfully.";
                return RedirectToAction("index", "Villa");
            }
            TempData["error"] = "The villa could not be created.";
            return View(obj);
        }

        public IActionResult Update(int villaId)
        {
            //Villa? obj= _db.Villas.FirstOrDefault(x => x.Id == villaId);
            Villa? obj = _unitofwork.villa.Get(x => x.Id == villaId);
            if(obj == null)
            {
                return RedirectToAction("Error", "Home");
            }
            return View(obj);
        }
		[HttpPost]
		public IActionResult Update(Villa obj)
		{
			if (ModelState.IsValid && obj.Id > 0)
			{
				_unitofwork.villa.Update(obj);
                _unitofwork.Save();
				TempData["success"] = "The villa has been update successfully.";
                return RedirectToAction("index", "Villa");
			}
            TempData["error"] = "The villa could not be updated.";
            return View(obj);
        }
        public IActionResult Delete(int villaId) {
			//Villa? obj= _db.Villas.FirstOrDefault(x => x.Id == villaId);
			Villa? obj = _unitofwork.villa.Get(x=>x.Id == villaId);
			if (obj == null)
			{
				return RedirectToAction("Error", "Home");
			}
			return View(obj);
		}

        [HttpPost]
		public IActionResult Delete(Villa obj)
        {
			Villa? objFromDb = _unitofwork.villa.Get(x => x.Id == obj.Id);
            if(objFromDb is not null)
            {
				_unitofwork.villa.Remove(objFromDb);
				_unitofwork.Save();
                TempData["success"] = "The villa has been deleted successfully.";
                return RedirectToAction("index", "Villa");
            }
            TempData["error"] = "The villa could not be deleted.";
            return View("index");
		}
    }
}
