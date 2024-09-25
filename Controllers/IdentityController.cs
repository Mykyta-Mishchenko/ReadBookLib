using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Identity.Client;
using Microsoft.IdentityModel.Tokens;
using ReadBookLib.Models;
using ReadBookLib.Models.ViewModels;
using System.Security.Claims;

namespace ReadBookLib.Controllers
{
    public class IdentityController : Controller
    {
        private UserManager<User> _userManager;
        private SignInManager<User> _signInManager;
        private RoleManager<IdentityRole> _roleManager;
        public IdentityController(UserManager<User> userManager, 
               SignInManager<User> signInManager, RoleManager<IdentityRole> roleManager) 
        { 
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }
        [HttpGet]
        public IActionResult SignUp()
        {
            return View(new SignUpViewModel() { Role = "Member"});
        }
        [HttpPost]
        public async Task<IActionResult> SignUp(SignUpViewModel model)
        {
            if (model.Role.IsNullOrEmpty())
            {
                ModelState.Remove("Role");
				model.Role = "Member";
			}
            if (!(await _roleManager.RoleExistsAsync(model.Role)))
            {
                var role = new IdentityRole { Name = model.Role };
                var result = await _roleManager.CreateAsync(role);
                if (!result.Succeeded)
                {
                    var errors = result.Errors.Select(x => x.Description);
                    ModelState.AddModelError("Role", string.Join(", ", errors));
                    return View(model);
                }
            }

            if (ModelState.IsValid)
            {
                if (await _userManager.FindByEmailAsync(model.Email) == null)
                {
                    var user = new User
                    {
                        FirstName = model.FirstName,
                        LastName = model.LastName,
						UserName = model.Email,
						Email = model.Email,
                    };
                    var result = await _userManager.CreateAsync(user, model.Password);
                    if (result.Succeeded)
                    {
                        user = await _userManager.FindByEmailAsync(model.Email);
                        await _userManager.AddToRoleAsync(user, model.Role);
                        return Redirect("SignIn");
                    }
                    if (result.Errors.Any(e => e.Description.Contains("Password")))
                    {
						ModelState.AddModelError("Password", result.Errors.Where(x => x.Description.Contains("Password")).First().Description);
					}
					return View(model);
                }
            }
            return View(model);
        }
        [HttpGet]
        public IActionResult SignIn()
        {
            return View(new SignInViewModel());  
        }
        [HttpPost]
        public async Task<IActionResult> SignIn(SignInViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, false);
                if(!result.Succeeded)
                {
                    ModelState.AddModelError("Login", "Cannot login.");
                }
                else
                {
                    return RedirectToAction("Index","Home");
                }
            }
            return View(model);
        }
        public async Task<IActionResult> SignOut()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("SignIn");
        }

        [HttpPost]
        public async Task<IActionResult> ExternalLogin(string provider, string returnUrl = null)
        {
            var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, returnUrl);
            var callback = Url.Action("ExternalLoginCallback");
            properties.RedirectUri = callback;

            return Challenge(properties,provider);
        }

        public async Task<IActionResult> ExternalLoginCallback()
        {
            var info = await _signInManager.GetExternalLoginInfoAsync();
            var email = info.Principal.Claims.FirstOrDefault(x=>x.Type==ClaimTypes.Email);
            var user = new User { UserName = email.Value, Email = email.Value };
            await _userManager.CreateAsync(user);
            await _userManager.AddLoginAsync(user, info);
            await _signInManager.SignInAsync(user, false);

            return RedirectToAction("Index", "Home");
        }

        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
