using ExcelCreateWebUI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Drawing.Printing;

namespace ExcelCreateWebUI.Controllers
{
    
    [Authorize]
    public class ProductController : Controller
    {
    private readonly AppDbContext _context;
    private readonly UserManager<IdentityUser> _userManager;

        public ProductController(AppDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            return View();
        }


        public async Task< IActionResult> CreatePrdocut() {


            var user =await _userManager.FindByNameAsync(User.Identity.Name);
            var fileName = $"product-excel-{Guid.NewGuid().ToString().Substring(1, 10)}";

            UserFile userFile = new UserFile()
            {
                UserId=user.Id,
                FileName=fileName,  
                FileStatus=FileStatus.Creating
            };
            _context.userFiles.AddAsync(userFile);

            TempData["startCreating"] = true;
            return RedirectToAction(nameof(Files));
          
        }

        public async Task<IActionResult> Files()
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            return View();
        }
    }
}
