using KerstmanPROG6_Fedor_Kevin.models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Text.RegularExpressions;

namespace KerstmanPROG6_Fedor_Kevin.Controllers
{
    public class SantaController : Controller
    {
        public string[] names;
        public SantaController @object;
        public UserManager<ApplicationUser> _userManager;
        public SignInManager<ApplicationUser> _signInManager;
        public RoleManager<ApplicationUserRole> _roleManager;
        
        public SantaController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            RoleManager<ApplicationUserRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }
        [HttpPost]
        //[Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(ApplicationUser Input)
        {
            if (ModelState.IsValid)
            {
                names = Input.Name.Split(',');

                for (int x = 0; x < names.Length; x++)
                {
                    names[x] = Regex.Replace(names[x], @"\s", "");
                }
                if (names.Length != names.Distinct().Count())
                {
                    TempData["alertMessage"] = "Er staan twee namen in de lijstje.";
                    return Redirect("/");
                }
                if (ModelState.IsValid)
                {
                    for (int x = 0; x < names.Length; x++)
                    {
                        var user = new ApplicationUser { UserName = names[x], Name = names[x] };
                        user.IsBehaved = Input.IsBehaved;
                        user.IsRegistered = false;
                        var role = new ApplicationUserRole { Role = "Child", Name = "Child" };

                        var claim = new Claim("Role", "Child");

                        ViewBag.Persons = Input.Name;
                        ViewBag.Password = Input.Password.ToLower();
                    }
                    return View("/Views/Home/RegisterSuccess.cshtml");
                }
            }
            return Redirect("/");
        }

        [HttpPost]
        public async Task<IActionResult> Login(ApplicationUser Input)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByNameAsync(Input.Name);
                if (user.IsRegistered != true)
                {
                    var result = await _signInManager.PasswordSignInAsync(Input.Name, Input.Password.ToLower(), false, lockoutOnFailure: false);

                    if (result.Succeeded)
                    {
                        return LocalRedirect("/");
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Invalid");
                        return View();
                    }
                }
            }
            TempData["alertMessage"] = "Je hebt je verlanglijstje al ingevuld! De kerstman is al voor je bezig.";
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> LogOut()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction(nameof(Login));
        }
        public IActionResult Register()
        {
            return View();
        }
        public IActionResult Login()
        {
            return View();
        }
    }
}
