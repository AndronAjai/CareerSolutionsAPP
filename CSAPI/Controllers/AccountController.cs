using Microsoft.AspNetCore.Mvc;

namespace CSAPI.Controllers
{
    public class AccountController : ControllerBase
    {

        private readonly ILogin _loginService;

        public AccountController(ILogin loginService)
        {
            _loginService = loginService;
        }

        public IActionResult Login()

        {
            return View();
        }


        [HttpPost]
        public IActionResult Login(Login model)
        {
            if (ModelState.IsValid)
            {
                if (_loginService.ValidateUser(model.UserName, model.Password, model.Role))
                {
                    return Content("User validated");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                    return Content("Failure");
                }
            }
            return View(model);
        }
    }
}
