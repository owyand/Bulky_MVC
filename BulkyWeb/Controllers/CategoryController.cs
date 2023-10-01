using BulkyWeb.Data;
using BulkyWeb.Models;
using Microsoft.AspNetCore.Mvc;

namespace BulkyWeb.Controllers
{
    public class CategoryController : Controller
    {

        private readonly ApplicationDbContext _db;
        public CategoryController(ApplicationDbContext db)
        {
            _db = db;
        }
        public IActionResult Index()
        {
            List<Category> objCategoryList = _db.Categories.ToList();
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
                _db.Categories.Add(obj);
                _db.SaveChanges();
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
            Category? catFromDb = _db.Categories.Find(id);
            //works on any property -- preferred
            Category catFromDb1 = _db.Categories.FirstOrDefault(u => u.Id == id);
            //also works for any property
            Category catFromDb2 = _db.Categories.Where(u => u.Id == id).FirstOrDefault();

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
                _db.Categories.Update(obj);
                _db.SaveChanges();
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
            Category? catFromDb = _db.Categories.Find(id);
            //works on any property -- preferred
            Category catFromDb1 = _db.Categories.FirstOrDefault(u => u.Id == id);
            //also works for any property
            Category catFromDb2 = _db.Categories.Where(u => u.Id == id).FirstOrDefault();

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

            Category obj = _db.Categories.Find(id);

            if (obj == null)
            {
                return NotFound();
            }

            _db.Categories.Remove(obj);
            _db.SaveChanges();
            TempData["success"] = "Category deleted successfully";


            return RedirectToAction("Index");
        }
    }
}
