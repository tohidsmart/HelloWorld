using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.Identity;
using HelloWorld.Models;
using HelloWorld.ViewModels;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace HelloWorld.Controllers.Web
{
	public class AuthController : Controller
	{
		private SignInManager<WorldUser> _singInManager;

		public AuthController(SignInManager<WorldUser> singInManager)
		{
			_singInManager = singInManager;
		}

		// GET: /<controller>/
		public IActionResult Login()
		{
			if (User.Identity.IsAuthenticated)
			{
				return RedirectToAction("Trip", "App");
			}
			return View();
		}

		[HttpPost]
		public async Task<ActionResult> Login(LoginViewModel vm,string returnUrl)
		{
			if (ModelState.IsValid)
			{
				var signInResult = await _singInManager.PasswordSignInAsync(vm.UserName, vm.Password, true, false);
                if (signInResult.Succeeded) {
					if (String.IsNullOrEmpty(returnUrl))
						RedirectToAction("Trips", "App");
					else
						Redirect(returnUrl);
				}
				else
				{
					ModelState.AddModelError("", "UserName or Password incorrect");
				}
				
			}
			return View();
        }

        public async Task<ActionResult> Logout()
        {
            if (User.Identity.IsAuthenticated)
            {
                await _singInManager.SignOutAsync();
            }

            return RedirectToAction( "Index","App");

        }
	}
}
