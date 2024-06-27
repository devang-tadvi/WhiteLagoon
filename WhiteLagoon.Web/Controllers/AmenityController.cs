using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using WhiteLagoon.Domain.Entities;
using WhiteLagoon.Infrastructure.Data;
using WhiteLagoon.Web.ViewModels;
using Microsoft.EntityFrameworkCore;
using WhiteLagoon.Application.Common.Interfaces;
using WhiteLagoon.Infrastructure.Repository;
namespace WhiteLagoon.Web.Controllers
{
    public class AmenityController : Controller
    {
		private readonly IUnitOfWork _unitofwork;

		public AmenityController(IUnitOfWork unitofwork)
		{
			_unitofwork = unitofwork;
		}
		public IActionResult Index()
        {
			var amenities = _unitofwork.amenity.GetAll(includeproperties: "Villa");
            return View(amenities);
        }
        public IActionResult Create()
        {
            AmenityVM villNumberVM = new AmenityVM()
            {
				VillaList= _unitofwork.villa.GetAll().Select(u => new SelectListItem
				{
					Text = u.Name,
					Value = u.Id.ToString()
				})
			};
             
            return View(villNumberVM);
        }

        [HttpPost]
        public IActionResult Create(AmenityVM obj)
        {
            
            if (ModelState.IsValid )
            {
				_unitofwork.amenity.Add(obj.Amenity);
				_unitofwork.Save();
				TempData["success"] = "The amenity has been created successfully.";
                return RedirectToAction("index", "Amenity");
            }
            
			obj.VillaList = _unitofwork.villa.GetAll().Select(u => new SelectListItem
            {
                Text = u.Name,
                Value = u.Id.ToString()
            });
		
			return View(obj);
        }

        public IActionResult Update(int amenityId)
        {
			AmenityVM amenityVM = new AmenityVM()
            {
                VillaList = _unitofwork.villa.GetAll().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                }),
                Amenity = _unitofwork.amenity.Get(u => u.Id == amenityId)
            };

			if (amenityVM.Amenity == null)
            {
                return RedirectToAction("Error", "Home");
            }
            return View(amenityVM);
        }
		[HttpPost]
		public IActionResult Update(AmenityVM obj)
		{
			
			if (ModelState.IsValid)
			{
				_unitofwork.amenity.Update(obj.Amenity);
				_unitofwork.Save();
				TempData["success"] = "The amenity has been update successfully.";
				return RedirectToAction("index", "Amenity");
			}
			
			obj.VillaList = _unitofwork.villa.GetAll().Select(u => new SelectListItem
			{
				Text = u.Name,
				Value = u.Id.ToString()
			});

			return View(obj);
		}
		public IActionResult Delete(int amenityId)
		{
			AmenityVM amenityVM = new AmenityVM()
			{
				VillaList = _unitofwork.villa.GetAll().Select(u => new SelectListItem
				{
					Text = u.Name,
					Value = u.Id.ToString()
				}),
				Amenity = _unitofwork.amenity.Get(u => u.Id == amenityId)
			};

			if (amenityVM.Amenity == null)
			{
				return RedirectToAction("Error", "Home");
			}
			return View(amenityVM);
		}

		[HttpPost]
		public IActionResult Delete(AmenityVM obj)
        {
			Amenity? objFromDb = _unitofwork.amenity.Get(x => x.Id == obj.Amenity.Id);
            if(objFromDb is not null)
            {
                _unitofwork.amenity.Remove(objFromDb);
				_unitofwork.Save();
                TempData["success"] = "The amenity has been deleted successfully.";
                return RedirectToAction("index", "Amenity");
            }
            TempData["error"] = "The amenity could not be deleted.";
            return View();
		}
    }
}
