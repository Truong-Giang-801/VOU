using Vou.Web.Service.IService;
using Vou.Web.Ultility;
using Vou.Web.Service;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddHttpContextAccessor();
builder.Services.AddHttpClient();
builder.Services.AddHttpClient<IBrandService, BrandService>();
builder.Services.AddHttpClient<IAuthService, AuthService>();
SD.BrandAPIBase = builder.Configuration["ServiceUrl:BrandAPI"];
SD.AuthAPIBase = builder.Configuration["ServiceUrl:AuthAPI"];

builder.Services.AddScoped<IBrandService, BrandService>();
builder.Services.AddScoped<IBaseService, BaseService>();
builder.Services.AddScoped<IAuthService, AuthService>();

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
