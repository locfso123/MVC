using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Routing.Constraints;
using mvc01.ExtendMethods;
using mvc01.Models;
using mvc01.Services;
using System.Net;
using System.Reflection.Metadata;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options =>
{
    string connectionString = builder.Configuration.GetConnectionString("AppDbContext");
    options.UseSqlServer(connectionString);
});

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

builder.Services.Configure<RazorViewEngineOptions>(options =>
{
    options.ViewLocationFormats.Add("/MyView/{1}/{0}" + RazorViewEngine.ViewExtension);
});

builder.Services.AddSingleton(typeof(ProductService), typeof(ProductService));
builder.Services.AddSingleton<PlanetService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.AddStatusCodePage();

app.UseEndpoints(endpoints =>
{
    endpoints.MapGet("/sayhi", async (context) =>
    {
        await context.Response.WriteAsync($"Hello ASP.NET MVC {DateTime.Now}");
    });

    endpoints.MapRazorPages();

    endpoints.MapControllers();

    endpoints.MapControllerRoute(
        name: "first",
        pattern: "{url:regex(^((xemsanpham)|(ViewProduct))$)}/{id:range(2,4)}",
        defaults: new
        {
            controller = "first",
            action = "ViewProduct"
        });

    endpoints.MapAreaControllerRoute(
            name: "product",
            pattern: "/{controller}/{action=Index}/{id?}",
            areaName: "ProductManage"
        );

    endpoints.MapControllerRoute(
        name: "default",
        pattern: "/{controller=Home}/{action=Index}/{id?}"
       /* defaults: new {
            controller = "first",
            action = "ViewProduct",
            id = 3*/
       );

});

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

 ContentRootPath = builder.Environment.ContentRootPath;

app.Run();

public partial class Program
{
    public static string ContentRootPath { get; set; } = string.Empty;
}
