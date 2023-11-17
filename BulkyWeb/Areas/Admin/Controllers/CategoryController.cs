using Bulky.DataAccess.Repository.IRepository;
using Bulky.Models;
using Bulky.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BulkyWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]
    public class CategoryController : Controller
    {

        private readonly IUnitOfWork _unitOfWork;
        public CategoryController(IUnitOfWork uow)
        {
            _unitOfWork = uow;
        }
        public IActionResult Index()
        {
            List<Category> objCategoryList = _unitOfWork.CategoryRepo.GetAll().ToList();
            return View(objCategoryList);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Category obj)
        {

            //custom validation - name and display order cannot be same value
            if (obj.Name == obj.DisplayOrder.ToString())
            {
                ModelState.AddModelError("name", "The Display Order and Name cannot be the same");
            }
            //custom validation - name cannot be test
            if (obj.Name == "test")
            {
                ModelState.AddModelError("", "Test is an invalid value");
            }

            if (ModelState.IsValid)
            {
                _unitOfWork.CategoryRepo.Add(obj);
                _unitOfWork.Save();
                TempData["success"] = "Category created successfully";

                return RedirectToAction("Index");
            }

            return View();
        }

        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            //works on primary key (which ID is)
            //Category? catFromDb = _categoryRepo.Categories.Find(id);
            //works on any property -- preferred
            Category? catFromDb = _unitOfWork.CategoryRepo.Get(u => u.Id == id);
            //also works for any property
            //Category catFromDb2 = _categoryRepo.Categories.Where(u => u.Id == id).FirstOrDefault();

            if (catFromDb == null)
            {
                return NotFound();
            }

            return View(catFromDb);
        }

        [HttpPost]
        public IActionResult Edit(Category obj)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.CategoryRepo.Update(obj);
                _unitOfWork.Save();
                TempData["success"] = "Category updated successfully";


                return RedirectToAction("Index");
            }

            return View();
        }

        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            //works on primary key (which ID is)
            //Category? catFromDb = _categoryRepo.Categories.Find(id);
            //works on any property -- preferred
            Category? catFromDb = _unitOfWork.CategoryRepo.Get(u => u.Id == id);
            //also works for any property
            //Category catFromDb2 = _categoryRepo.Categories.Where(u => u.Id == id).FirstOrDefault();

            if (catFromDb == null)
            {
                return NotFound();
            }

            return View(catFromDb);
        }

        //customizes the action URL
        [HttpPost, ActionName("Delete")]
        public IActionResult DeletePOST(int? id)
        {

            Category obj = _unitOfWork.CategoryRepo.Get(u => u.Id == id);

            if (obj == null)
            {
                return NotFound();
            }

            _unitOfWork.CategoryRepo.Remove(obj);
            _unitOfWork.Save();
            TempData["success"] = "Category deleted successfully";


            return RedirectToAction("Index");
        }
    }
}
