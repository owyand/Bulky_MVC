using Bulky.DataAccess.Repository.IRepository;
using Bulky.Models;
using Bulky.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BulkyWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public ProductController(IUnitOfWork uow)
        {
            _unitOfWork = uow;
        }
        public IActionResult Index()
        {
            List<Product> objProductList = _unitOfWork.ProductRepo.GetAll().ToList();
            IEnumerable<SelectListItem> CategoryList = _unitOfWork.CategoryRepo
                .GetAll().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                });

            return View(objProductList);
        }

        public IActionResult Upsert(int? id)
        {
            /*1/2
            IEnumerable<SelectListItem> CategoryList = _unitOfWork.CategoryRepo
                .GetAll().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                });

            //1.
            ViewBag.CategoryList = CategoryList;
            //2.
            ViewData["CategoryList"] = CategoryList;
            */
            //3.
            ProductVM productVM = new()
            {
                CategoryList = _unitOfWork.CategoryRepo
                .GetAll().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                }),

                Product = new Product()
            };

            if (id == null || id == 0)
            {
                return View(productVM);
            }
            else
            {
                //update
                productVM.Product = _unitOfWork.ProductRepo.Get(u => u.Id == id);
                return View(productVM);
            }
        }

        [HttpPost]
        public IActionResult Upsert(ProductVM productVM, IFormFile? file)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.ProductRepo.Add(productVM.Product);
                _unitOfWork.Save();
                TempData["success"] = "Product created successfully";

                return RedirectToAction("Index");
            }
            else
            {
                productVM.CategoryList = _unitOfWork.CategoryRepo
                .GetAll().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                });
            }

            return View(productVM);
        }

        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            Product? prodFromDb = _unitOfWork.ProductRepo.Get(u => u.Id == id);

            if (prodFromDb == null)
            {
                return NotFound();
            }

            return View(prodFromDb);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeletePOST(int? id)
        {
            Product? obj = _unitOfWork.ProductRepo.Get(u => u.Id == id);
            if (obj == null)
            {
                return NotFound();
            }
            _unitOfWork.ProductRepo.Remove(obj);
            _unitOfWork.Save();
            TempData["success"] = "Product deleted sucessfully";
            return RedirectToAction("Index");
        }

    }
}
