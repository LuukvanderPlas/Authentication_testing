using Authentication_testing.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Authentication_testing.Controllers {
    public class AccountController : Controller {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AccountController(
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            RoleManager<IdentityRole> roleManager) {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }

        public IActionResult Login(string? ReturnUrl) {
            return View(new LoginViewModel { ReturnUrl = ReturnUrl });
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel input) {
            if (ModelState.IsValid) {
                var result = await _signInManager.PasswordSignInAsync(input.Email, input.Password, false, lockoutOnFailure: false);
                if (result.Succeeded) {
                    //_logger.LogInformation("User logged in.");
                    return LocalRedirect(input.ReturnUrl ?? Url.Content("~/"));
                } else {
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                    return View(input);
                }
            }

            // If we got this far, something failed, redisplay form
            return View(input);
        }

        public IActionResult Register(string? ReturnUrl) {
            return View(new RegisterViewModel { ReturnUrl = ReturnUrl });
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel input) {
            if (ModelState.IsValid) {
                var user = new IdentityUser { UserName = input.Email, Email = input.Email };
                var result = await _userManager.CreateAsync(user, input.Password);

                if (result.Succeeded) {
                    //_logger.LogInformation("User created a new account with password.");

                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return LocalRedirect(input.ReturnUrl ?? Url.Content("~/"));
                }

                foreach (var error in result.Errors) {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            // If we got this far, something failed, redisplay form
            return View(input);
        }

        //[HttpPost]
        public async Task<IActionResult> Logout() {
            if (User.Identity.IsAuthenticated) {
                await _signInManager.SignOutAsync();
            }

            //_logger.LogInformation("User logged out.");
            return RedirectToAction("Index", "Home");
        }
    }
}
