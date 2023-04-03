using ExcelCreateWebUI.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();


builder.Services.AddDbContext<AppDbContext>(options => {
    options.UseSqlServer(builder.Configuration.GetConnectionString("Sqlserver"));
});

builder.Services.AddIdentity<IdentityUser, IdentityRole>(opt =>
{

    opt.User.RequireUniqueEmail = true;

}).AddEntityFrameworkStores<AppDbContext>();    

var app = builder.Build();
using(var scope = app.Services.CreateScope())
{
    var appDbContext =scope.ServiceProvider.GetRequiredService<AppDbContext>();

    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();
    appDbContext.Database.Migrate();

    if (!appDbContext.Users.Any())
    {

        userManager.CreateAsync(new IdentityUser() {
        UserName="admin",Email="admin@alp.com",

        },"Password12*").Wait();
        userManager.CreateAsync(new IdentityUser()
        {
            UserName = "user1",Email = "user1@alp.com",

        },"Password12*").Wait();
    }
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();


app.UseAuthentication();
app.UseAuthorization();


app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
