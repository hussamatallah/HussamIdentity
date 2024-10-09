using HussamIdentity.Data;
using HussamIdentity.Models.catagorys;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Elfie.Diagnostics;

namespace HussamIdentity.Controllers
{
    public class CategoriesController : Controller
    {
        private readonly ILogger<CategoriesController> logger;
        private HussamDbContext Db;

        public CategoriesController(ILogger<CategoriesController> _logger, HussamDbContext _Db)
        {
            Db = _Db;
            logger = _logger;
        }

        public IActionResult Index()
        {
            return View(); 
        }
        public IActionResult AllCategories()
        {
            return View(Db.Categorys);
        }
       


        

        [HttpGet]
        public IActionResult Details(int? Id)
        {
            if (Id == null)
            {
                return RedirectToAction("AllCategories");
            }
            var data = Db.Categorys.Find(Id);
            if (data == null)
            {
                return RedirectToAction("AllCategories");
            }
            return View(data);
        }
        [HttpGet]

        [Authorize(Roles = "Admin")]
        public IActionResult Edit(int? Id)
        {
            if (Id == null)
            {
                return RedirectToAction("AllCategories");
            }
            var data = Db.Categorys.Find(Id);
            if (data == null)
            {
                return RedirectToAction("AllCategories");
            }
            return View(data);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult Edit(Category Cat)
        {
            if (!ModelState.IsValid)
            {
                Db.Categorys.Update(Cat);
                Db.SaveChanges();

                return RedirectToAction("AllCategories");
            }
            return View(Cat);
        }


        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult Delete(int? Id)
        {
            if (Id == null)
            {
                return RedirectToAction("AllCategories");
            }
            var data = Db.Categorys.Find(Id);
            if (data == null)
            {
                return RedirectToAction("AllCategories");
            }
            return View(data);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult Delete(Category Cat)
        {
            var data = Db.Categorys.Find((Cat.CategoryId));

            if (data == null)
            {
                return RedirectToAction("AllCategories");
            }
            Db.Categorys.Remove(data);
            Db.SaveChanges();
            return RedirectToAction("AllCategories");
        }

    }
}
