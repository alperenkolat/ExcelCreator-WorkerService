using ExcelCreateWebUI.Models;
using ExcelCreatorWorkerService.Hubs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace ExcelCreateWebUI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FilesController : ControllerBase
    {


        private readonly AppDbContext _context;

        private readonly IHubContext<MyHub> _hubContext;
        public FilesController(AppDbContext context,IHubContext<MyHub> hubContext)
        {
            _context = context;
            _hubContext = hubContext;
        }
        [HttpPost]
        public async Task<IActionResult> Upload(IFormFile file,int fileID)
        {


            if (file is not { Length: > 0 }) return BadRequest();

            var userFile = await _context.userFiles.FirstAsync(x => x.Id == fileID);
            if (userFile is null) return BadRequest();

            var filePath=userFile.FileName+Path.GetExtension(file.FileName);

            var path=Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Files");

            using FileStream stream = new FileStream(path, FileMode.Create);
            

            await file.CopyToAsync(stream);

            userFile.CreatedTime = DateTime.Now;
            userFile.FilePath = filePath;

            userFile.FileStatus = FileStatus.Completed;
            await _context.SaveChangesAsync();

            await _hubContext.Clients.User(userFile.UserId).SendAsync("CompletedFile");

            return Ok();
                }
    }
}
