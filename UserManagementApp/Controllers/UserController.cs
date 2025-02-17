using Microsoft.AspNetCore.Mvc;
using UserManagementApp.Business;
using UserManagementApp.Entities;
using UserManagementApp.Entities.DTO;

namespace UserManagementApp.Web.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        public IActionResult Index()
        {
            var users = _userService.GetAll();
            return View(users);
        }

        public IActionResult Create()
        {
            return View();
        } 
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Login(LoginRequest model)
        {
            var token = _userService.Authenticate(model.email, model.password);
            if (token != null)
            {
                Response.Cookies.Append("AuthToken", token);
                return RedirectToAction("Index", "User");
            }

            ViewBag.Error = "Geçersiz e-posta veya şifre. Lütfen tekrar deneyin.";
            return View(model);
        }


        [HttpPost]
        public IActionResult Create(User user)
        {
            _userService.Add(user);
            return RedirectToAction("Index");
        } 
     


    }
}
