using Microsoft.AspNetCore.Mvc;

namespace VehicleRentalManagementSystem.Controllers
{
    public class AccountController : Controller
    {
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(string username, string password)
        {
            if (string.IsNullOrWhiteSpace(username))
            {
                ModelState.AddModelError(
                    "username",
                    "Username is required.");
            }

            if (string.IsNullOrWhiteSpace(password))
            {
                ModelState.AddModelError(
                    "password",
                    "Password is required.");
            }

            if (!ModelState.IsValid)
            {
                return View();
            }

            if (username == "admin" && password == "admin123")
            {
                HttpContext.Session.SetString("AdminLoggedIn", "true");

                return RedirectToAction(
                    "Index",
                    "Home");
            }

            ModelState.AddModelError(
                "",
                "Invalid username or password.");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();

            return RedirectToAction(
                "Login",
                "Account");
        }
    }
}