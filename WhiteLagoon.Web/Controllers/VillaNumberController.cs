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
    public class VillaNumberController : Controller
    {
		private readonly IUnitOfWork _unitofwork;

		public VillaNumberController(IUnitOfWork unitofwork)
		{
			_unitofwork = unitofwork;
		}
		public IActionResult Index()
        {
			var villanumbers = _unitofwork.villaNumber.GetAll(includeproperties: "Villa");
            return View(villanumbers);
        }
        public IActionResult Create()
        {
            VillaNumberVM villNumberVM = new VillaNumberVM()
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
        public IActionResult Create(VillaNumberVM obj)
        {
            bool roomNumberExits = _unitofwork.villaNumber.Any(u => u.Villa_Number == obj.VillaNumber.Villa_Number);
            if (ModelState.IsValid && !roomNumberExits)
            {
				_unitofwork.villaNumber.Add(obj.VillaNumber);
				_unitofwork.Save();
				TempData["success"] = "The villa number has been created successfully.";
                return RedirectToAction("index", "VillaNumber");
            }
            if(roomNumberExits)
                TempData["error"] = "The villa number could not be created.";


			obj.VillaList = _unitofwork.villa.GetAll().Select(u => new SelectListItem
            {
                Text = u.Name,
                Value = u.Id.ToString()
            });
		
			return View(obj);
        }

        public IActionResult Update(int villaNumberId)
        {
			VillaNumberVM villNumberVM = new VillaNumberVM()
            {
                VillaList = _unitofwork.villa.GetAll().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                }),
                VillaNumber = _unitofwork.villaNumber.Get(u => u.Villa_Number == villaNumberId)
            };

			if (villNumberVM.VillaNumber == null)
            {
                return RedirectToAction("Error", "Home");
            }
            return View(villNumberVM);
        }
		[HttpPost]
		public IActionResult Update(VillaNumberVM obj)
		{
			
			if (ModelState.IsValid)
			{
				_unitofwork.villaNumber.Update(obj.VillaNumber);
				_unitofwork.Save();
				TempData["success"] = "The villa number has been update successfully.";
				return RedirectToAction("index", "VillaNumber");
			}
			
			obj.VillaList = _unitofwork.villa.GetAll().Select(u => new SelectListItem
			{
				Text = u.Name,
				Value = u.Id.ToString()
			});

			return View(obj);
		}
		public IActionResult Delete(int villaNumberId)
		{
			VillaNumberVM villNumberVM = new VillaNumberVM()
			{
				VillaList = _unitofwork.villa.GetAll().Select(u => new SelectListItem
				{
					Text = u.Name,
					Value = u.Id.ToString()
				}),
				VillaNumber = _unitofwork.villaNumber.Get(u => u.Villa_Number == villaNumberId)
			};

			if (villNumberVM.VillaNumber == null)
			{
				return RedirectToAction("Error", "Home");
			}
			return View(villNumberVM);
		}

		[HttpPost]
		public IActionResult Delete(VillaNumberVM obj)
        {
			VillaNumber? objFromDb = _unitofwork.villaNumber.Get(x => x.Villa_Number == obj.VillaNumber.Villa_Number);
            if(objFromDb is not null)
            {
                _unitofwork.villaNumber.Remove(objFromDb);
				_unitofwork.Save();
                TempData["success"] = "The villa number has been deleted successfully.";
                return RedirectToAction("index", "VillaNumber");
            }
            TempData["error"] = "The villa number could not be deleted.";
            return View(obj.VillaNumber.Villa_Number);
		}
    }
}
