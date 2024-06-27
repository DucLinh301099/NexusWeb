using AutoMapper;
using NexusWeb.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using NexusWeb.Repository;
using Microsoft.AspNetCore.Authentication.Cookies;
using NexusWeb.Helpers;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("default");



// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<NexusWebAppContext>(
	option => option.UseSqlServer(connectionString)

	);
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<IConnectionTypeRepository, ConnectionTypeRepository>();
//builder.Services.AddIdentity<User, IdentityRole>(
//        option =>
//        {
//            option.Password.RequiredUniqueChars = 0;
//            option.Password.RequireUppercase = false;
//            option.Password.RequiredLength = 8;
//            option.Password.RequireNonAlphanumeric = false;
//            option.Password.RequireLowercase = false;
//        }
//    ).AddEntityFrameworkStores<NaxusWebAppContext>().AddDefaultTokenProviders();


builder.Services.AddDistributedMemoryCache();

builder.Services.AddSession(options =>
{
	options.IdleTimeout = TimeSpan.MaxValue;
	options.Cookie.HttpOnly = true;
	options.Cookie.IsEssential = true;
});

builder.Services.AddAutoMapper(typeof(AutoMapperProfile));
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(
	options =>
	{
		options.LoginPath = "/Auth/Login";
		options.AccessDeniedPath = "/AccessDenied";
	}
	);

var app = builder.Build();
app.Use(async (ctx, next) =>
{
	await next();

	if (ctx.Response.StatusCode == 404 && !ctx.Response.HasStarted)
	{
		//Re-execute the request so the user gets the error page
		string originalPath = ctx.Request.Path.Value;
		ctx.Items["originalPath"] = originalPath;
		ctx.Request.Path = "/NotFound";
		await next();
	}
});
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
app.UseSession();

app.MapControllerRoute(
	name: "default",
	pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
