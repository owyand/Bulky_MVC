using Bulky.DataAccess.Repository.IRepository;
using Bulky.Models;
using Microsoft.AspNetCore.Mvc;

namespace BulkyWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CompanyController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public CompanyController(IUnitOfWork uow)
        {
            _unitOfWork = uow;
        }
        public IActionResult Index()
        {
            List<Company> objCompanyList = _unitOfWork.CompanyRepo.GetAll().ToList();

            return View(objCompanyList);
        }

        public IActionResult Upsert(int? id)
        {
            if (id == null || id == 0)
            {
                return View(new Company());
            }
            else
            {
                Company companyObj = _unitOfWork.CompanyRepo.Get(u => u.Id == id);
                return View(companyObj);
            }
        }

        [HttpPost]
        public IActionResult Upsert(Company companyObj, IFormFile? file)
        {
            if (ModelState.IsValid)
            {

                //save or update
                if (companyObj.Id == 0)
                {
                    _unitOfWork.CompanyRepo.Add(companyObj);
                }
                else
                {
                    _unitOfWork.CompanyRepo.Update(companyObj);
                }

                _unitOfWork.Save();
                TempData["success"] = "Company created successfully";

                return RedirectToAction("Index");
            }
            else
            {

                return View(companyObj);
            }
        }

        #region API CALLS

        [HttpGet]
        public IActionResult GetAll()
        {
            List<Company> objCompanyList = _unitOfWork.CompanyRepo.GetAll().ToList();

            return Json(new { data = objCompanyList });
        }

        public IActionResult Delete(int? id)
        {

            var compToBeDeleted = _unitOfWork.CompanyRepo.Get(u => u.Id == id);
            if (compToBeDeleted == null)
            {
                return Json(new { success = false, message = "Error while deleting" });
            }

            _unitOfWork.CompanyRepo.Remove(compToBeDeleted);
            _unitOfWork.Save();

            return Json(new { success = true, message = "Delete successful" });
        }

        #endregion

    }
}
