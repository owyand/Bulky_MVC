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
            Category catFromDb = _db.Categories.Find(id);
            //works on any property -- preferred
            Category catFromDb1 = _db.Categories.FirstOrDefaut(u => u.Id == id);
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

                return RedirectToAction("Index");
            }

            return View();
        }
    }
}
