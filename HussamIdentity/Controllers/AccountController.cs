using HussamIdentity.Models.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Data;

namespace HussamIdentity.Controllers
{
    public class AccountController : Controller
    {
        #region Configration

        private UserManager<IdentityUser> userManager;
        private SignInManager<IdentityUser> SignInManager;
        private RoleManager<IdentityRole> RoleManager;

        public AccountController(UserManager<IdentityUser> _userManager, SignInManager<IdentityUser> signInManager, RoleManager<IdentityRole> roleManager)
        {
            userManager = _userManager;
            SignInManager = signInManager;
            RoleManager = roleManager;
        }
        #endregion
        #region User
        [AllowAnonymous]
        public IActionResult Register()
        {
            return View();
        }
        [HttpGet]
        [AllowAnonymous]

        public IActionResult Login()
        {
            return View();
        }
        [AllowAnonymous]

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                IdentityUser user = new IdentityUser
                {
                    UserName = model.Email,
                    Email = model.Email,
                    PhoneNumber = model.Phone
                };
                var result = await userManager.CreateAsync(user, model.Password!);
                if (result.Succeeded)
                {
                    return RedirectToAction("Login", "Account");
                }
                foreach (var err in result.Errors)
                {
                    ModelState.AddModelError(err.Code, err.Description);
                }
                return View(model);
            }
            return View();

        }

        [HttpPost]
        [AllowAnonymous]

        public async Task<IActionResult> Login(LoginModelView model)
        {
            if (!ModelState.IsValid)
            {
                var res = await SignInManager.PasswordSignInAsync(model.Email!, model.Password!, false, false);
                if (res.Succeeded)
                {


                    return RedirectToAction("Index", "Home");

                }
                ModelState.AddModelError("", "in valid user or email");
                return View(model);

            }
            return View();

        }
        public async Task<IActionResult> Logout()
        {
            await SignInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
        #endregion
        #region Role
        [HttpGet]
        //  [Authorize(Roles = "Admin")]
        [AllowAnonymous]

        public IActionResult CreateRole()
        {
            return View();
        }
        [HttpPost]
        [AllowAnonymous]

        public async Task<IActionResult> CreateRole(CreateRoleModelView model)
        {
            if (ModelState.IsValid)
            {
                IdentityRole role = new IdentityRole()
                {
                    Name = model.RoleName
                };
                var result = await RoleManager.CreateAsync(role);

                if (result.Succeeded)
                {
                    return RedirectToAction("RolesList", "Account");
                }
                foreach (var err in result.Errors)
                {
                    ModelState.AddModelError(err.Code, err.Description);
                }
                return View(model);
            }
            return View();

        }
        [AllowAnonymous]


        public IActionResult RolesList()
        {
            return View(RoleManager.Roles);
        }

        public IActionResult AccessDenied()
        {
            return View();
        }

        [HttpGet]
        //  [Authorize(Roles = "Admin")]
        [AllowAnonymous]

        public async Task<IActionResult> EditRole(string Id)
        {
            // التحقق من أن المعرف ليس فارغًا وأنه GUID صالح
            if (string.IsNullOrEmpty(Id) || !Guid.TryParse(Id, out Guid roleId))
            {
                // يمكن هنا إضافة رسالة للمستخدم توضح أن المعرف غير صالح (اختياري)
                TempData["ErrorMessage"] = "Invalid Role ID.";
                return RedirectToAction("RolesList");
            }

            // البحث عن الدور باستخدام المعرف
            var role = await RoleManager.FindByIdAsync(Id);
            if (role == null)
            {
                TempData["ErrorMessage"] = "Role not found.";
                return RedirectToAction("RolesList");
            }

            // إنشاء نموذج (ViewModel) لعرضه في صفحة التحرير
            EditRoleViewModel model = new EditRoleViewModel
            {
                RoleId = role.Id,
                RoleName = role.Name
            };
            foreach (var user in userManager.Users)
            {
                if (await userManager.IsInRoleAsync(user, role.Name!))
                {
                    model.Users!.Add(user.UserName!);
                }
            }

            // عرض النموذج في صفحة التحرير
            return View(model);
        }
        [HttpPost]
        [AllowAnonymous]

        public async Task<IActionResult> EditRole(EditRoleViewModel model)
        {

            if (ModelState.IsValid)
            {
                var role = await RoleManager.FindByIdAsync(model.RoleId!);
                if (role == null)
                {
                    TempData["ErrorMessage"] = "Role not found.";
                    return RedirectToAction("RolesList");
                }
                role.Name = model.RoleName;
                var result = await RoleManager.UpdateAsync(role);
                if (result.Succeeded)
                {
                    return RedirectToAction(nameof(RolesList)); // نفس الاشي الثنتين 

                }
                foreach (var item in result.Errors)
                {
                    ModelState.AddModelError(item.Code, item.Description);
                }
                return View(model);
            }
            return View(model);


        }

        [HttpGet]
        public async Task<IActionResult> ModifyUser(string id)
        {

            if (id == null)
            {
                TempData["ErrorMessage"] = "Role not found.";
                return RedirectToAction("RolesList");
            }
            var role = await RoleManager.FindByIdAsync(id);
            if (role == null)
            {
                TempData["ErrorMessage"] = "Role not found.";
                return RedirectToAction("RolesList");
            }
            List<UserRoleViewModel> models = new List<UserRoleViewModel>();
            foreach (var User in userManager.Users)
            {
                UserRoleViewModel userRole = new UserRoleViewModel
                {
                    UserId = User.Id,
                    UserName = User.UserName


                };
                if (await userManager.IsInRoleAsync(User, role.Name!))
                {
                    userRole.IsSelected = true;
                }
                else
                {
                    userRole.IsSelected = false;

                }
                models.Add(userRole);

            }
            return View(models);


        }
        [HttpPost]
        public async Task<IActionResult> ModifyUser(string id, List<UserRoleViewModel> models)
        {

            if (id == null)
            {
                TempData["ErrorMessage"] = "Role not found.";
                return RedirectToAction("RolesList");
            }
            var role = await RoleManager.FindByIdAsync(id);
            if (role == null)
            {
                TempData["ErrorMessage"] = "Role not found.";
                return RedirectToAction("RolesList");
            }
            IdentityResult result = new IdentityResult();
            for (int i = 0; i < models.Count; i++)
            {

                var user = await userManager.FindByIdAsync(models[i].UserId!);
                if (models[i].IsSelected && (!await userManager.IsInRoleAsync(user!, role.Name!)))
                {
                    result = await userManager.AddToRoleAsync(user!, role.Name!);
                }
                else if (!models[i].IsSelected && (await userManager.IsInRoleAsync(user!, role.Name!)))
                {
                    result = await userManager.RemoveFromRoleAsync(user!, role.Name!);
                }

            }
            if (result.Succeeded)
            {
                return RedirectToAction(nameof(RolesList));
            }
            return View(models);




        }
          
            
   
        #endregion

    }
}