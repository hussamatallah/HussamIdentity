using HussamIdentity.Data;
using HussamIdentity.Models.catagorys;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace HussamIdentity.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ILogger<CategoryController> _logger;
        private HussamDbContext _Db;

        public CategoryController(ILogger<CategoryController> logger, HussamDbContext Db)
        {
            _logger = logger;
            _Db = Db;
        }

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult AllProduct()
        {
            return View(_Db.Products.Include(x => x.Category));


        }
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            ViewBag.CatList = new SelectList(_Db.Categorys, "CategoryId", "Name");
            return View();  

        }
        [HttpPost]
        public IActionResult Create(Product model)
        {
            if (ModelState.IsValid)
            {
                _Db.Products.Add(model);
                _Db.SaveChanges();
                return RedirectToAction("AllProduct");
            }
            return View();

        }

        [HttpGet]
        public IActionResult CreateCategory()
        {
            return View();

        }
        [HttpPost]
       
        public IActionResult CreateCategory(Category cat)
        {
            if (ModelState.IsValid)
            {
                // تحقق مما إذا كان اسم الفئة موجودًا بالفعل
                var existingCategory = _Db.Categorys.FirstOrDefault(c => c.Name == cat.Name);
                if (existingCategory != null)
                {
                    ModelState.AddModelError("Name", "اسم الفئة موجود مسبقًا."); // إضافة خطأ إلى ModelState
                    return View(cat); // إعادة عرض نفس النموذج مع رسالة الخطأ
                }

                _Db.Categorys.Add(cat);
                _Db.SaveChanges();
                return RedirectToAction("AllProduct");
            }

            return View(cat); // إعادة عرض النموذج مع الأخطاء الموجودة فيه
        }


    }
}
