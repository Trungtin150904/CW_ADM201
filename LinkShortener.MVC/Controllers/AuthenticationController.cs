using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using LinkShortener.MVC.Models;

namespace LinkShortener.MVC.Controllers
{
    public class AuthenticationController : Controller
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IUserStore<IdentityUser> _userStore;
        private readonly IUserEmailStore<IdentityUser> _emailStore;
        private readonly ILogger<AuthenticationController> _logger;

        public AuthenticationController(
            UserManager<IdentityUser> userManager,
            IUserStore<IdentityUser> userStore,
            SignInManager<IdentityUser> signInManager,
            ILogger<AuthenticationController> logger)
        {
            _userManager = userManager;
            _userStore = userStore;
            _emailStore = GetEmailStore();
            _signInManager = signInManager;
            _logger = logger;
        }

        // GET /Authentication/Login
        public IActionResult Login(string? returnUrl = null)
        {
            if (User.Identity?.IsAuthenticated == true)
                return RedirectToAction("Index", "Home");

            return View(new LoginVM { ReturnUrl = returnUrl });
        }

        // POST /Authentication/Login
        [HttpPost]
        public async Task<IActionResult> Login(LoginVM loginVM)
        {
            var returnUrl = string.IsNullOrEmpty(loginVM.ReturnUrl)
                ? Url.Content("~/") : loginVM.ReturnUrl;

            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(
                    loginVM.Email, loginVM.Password,
                    loginVM.RememberMe, lockoutOnFailure: false);

                if (result.Succeeded)
                {
                    _logger.LogInformation("User logged in.");
                    return LocalRedirect(returnUrl);
                }
                if (result.IsLockedOut)
                {
                    ModelState.AddModelError(string.Empty, "Your account is locked. Try again later.");
                    return View(loginVM);
                }

                ModelState.AddModelError(string.Empty, "Incorrect email or password.");
            }
            return View(loginVM);
        }

        // GET /Authentication/Register
        public IActionResult Register(string? returnUrl = null)
        {
            if (User.Identity?.IsAuthenticated == true)
                return RedirectToAction("Index", "Home");

            return View(new RegisterVM { ReturnUrl = returnUrl });
        }

        // POST /Authentication/Register
        [HttpPost]
        public async Task<IActionResult> Register(RegisterVM registerVM)
        {
            var returnUrl = string.IsNullOrEmpty(registerVM.ReturnUrl)
                ? Url.Content("~/") : registerVM.ReturnUrl;

            if (ModelState.IsValid)
            {
                var user = new IdentityUser();
                await _userStore.SetUserNameAsync(user, registerVM.Email, CancellationToken.None);
                await _emailStore.SetEmailAsync(user, registerVM.Email, CancellationToken.None);

                var result = await _userManager.CreateAsync(user, registerVM.Password);
                if (result.Succeeded)
                {
                    // ✅ Mọi user mới đều là role "User"
                    await _userManager.AddToRoleAsync(user, "User");
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    _logger.LogInformation("User created a new account.");
                    return LocalRedirect(returnUrl);
                }
                foreach (var error in result.Errors)
                    ModelState.AddModelError(string.Empty, error.Description);
            }
            return View(registerVM);
        }

        // POST /Authentication/Logout
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login");
        }

        // GET /Authentication/AccessDenied
        public IActionResult AccessDenied()
        {
            return View();
        }

        private IUserEmailStore<IdentityUser> GetEmailStore()
        {
            if (!_userManager.SupportsUserEmail)
                throw new NotSupportedException("Requires email support.");
            return (IUserEmailStore<IdentityUser>)_userStore;
        }
    }
}