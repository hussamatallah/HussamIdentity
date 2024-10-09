using HussamIdentity.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.CodeAnalysis.Options;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<HussamDbContext>(options => //option is obj of nextwoDbcontext 
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("constr"));
});
builder.Services.AddIdentity<IdentityUser,IdentityRole>( option => {
    option.Password.RequiredUniqueChars = 0;
    option.Password.RequireUppercase = false;
    option.Password.RequireLowercase = false;
    option.Password.RequiredLength = 5;
    option.Password.RequireDigit = false;



}).AddEntityFrameworkStores<HussamDbContext>();
var app = builder.Build();

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

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
