using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using App;

var builder = WebApplication.CreateBuilder(args);
Util.configuration = builder.Configuration;
Util.rootPath = builder.Environment.WebRootPath;
builder.Services.AddControllersWithViews(options => {
    options.ModelBinderProviders.Insert(0, new DateTimeModelBinderProvider());
});
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession();
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(options => {
    options.LoginPath = "/Login";
    options.LogoutPath = "/Logout";
    options.AccessDeniedPath = "/AccessDenied";
});
builder.Services.AddDbContext<App.Models.DataContext>(options => options.UseMySQL(builder.Configuration.GetConnectionString("Database")));
var app = builder.Build();
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseSession();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=System}/{action=Index}/{id?}"
);
app.Run();