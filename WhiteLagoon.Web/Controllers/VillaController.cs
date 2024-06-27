using Microsoft.AspNetCore.Mvc;
using System.IO;
using WhiteLagoon.Application.Common.Interfaces;
using WhiteLagoon.Domain.Entities;
using WhiteLagoon.Infrastructure.Data;
using WhiteLagoon.Infrastructure.Repository;

namespace WhiteLagoon.Web.Controllers
{
    public class VillaController : Controller
    {
        private readonly IUnitOfWork _unitofwork;

        private readonly IWebHostEnvironment _webHostEnvironment;
        public VillaController(IUnitOfWork unitofwork, IWebHostEnvironment webHostEnvironment)
        {
            _unitofwork = unitofwork;
            _webHostEnvironment = webHostEnvironment;
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
                if(obj.Image !=null)
                {
                    string filename = Guid.NewGuid().ToString()+Path.GetExtension(obj.Image.FileName);
                    string imagepath = Path.Combine(_webHostEnvironment.WebRootPath, @"images\VillaImage");

                    using (var fileStream = new FileStream(Path.Combine(imagepath, filename), FileMode.Create)) 
					{
                        obj.Image.CopyTo(fileStream);
                    }
                    obj.ImageUrl = @"\images\VillaImage\"+filename;
                }
                else
                {
                    obj.ImageUrl = "https://placehold.co/600x400";

				}
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
				if (obj.Image != null)
				{
					string filename = Guid.NewGuid().ToString() + Path.GetExtension(obj.Image.FileName);
					string imagepath = Path.Combine(_webHostEnvironment.WebRootPath, @"images\VillaImage");

                    if(!string.IsNullOrEmpty(obj.ImageUrl))
                    {
                        var oldImagePath = Path.Combine(_webHostEnvironment.WebRootPath, obj.ImageUrl.TrimStart('\\'));

                        if(System.IO.File.Exists(oldImagePath))
                            System.IO.File.Delete(oldImagePath);
                    }

					using (var fileStream = new FileStream(Path.Combine(imagepath, filename), FileMode.Create))
					{
						obj.Image.CopyTo(fileStream);
					}
					obj.ImageUrl = @"\images\VillaImage\" + filename;
				}
				

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
				if (!string.IsNullOrEmpty(objFromDb.ImageUrl))
				{
					var oldImagePath = Path.Combine(_webHostEnvironment.WebRootPath, objFromDb.ImageUrl.TrimStart('\\'));

					if (System.IO.File.Exists(oldImagePath))
						System.IO.File.Delete(oldImagePath);
				}

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
