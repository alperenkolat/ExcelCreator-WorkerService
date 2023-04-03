using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;

namespace ExcelCreateWebUI.Controllers
{

    public class AccountController : Controller
    {


        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser>_signManager;

        public AccountController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signManager)
        {
            _userManager = userManager;
            _signManager = signManager;
        }

        public IActionResult Login()
        {


            return View();
        }
        [HttpPost]
        public async Task<IActionResult>Login( string Email ,string Password)
        {
            var hasUser=await  _userManager.FindByEmailAsync(Email);
            if (hasUser == null)
            {

                return View();
                
            }
            var signInResult = await _signManager.PasswordSignInAsync(hasUser,Password,true,false);  
            if(signInResult == null)
            {

                return View();
            }   
            return RedirectToAction(nameof(HomeController.Index),"Home"); 
        }
    }
}
