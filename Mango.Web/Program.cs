using Mango.Web.RestService;
using Mango.Web.RestService.IRestService;
using Mango.Web.Service.IService;
using Mango.Web.Utility;
using System.ComponentModel.Design;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddHttpContextAccessor();


SD.CouponAPIBase = builder.Configuration["ServiceUrls:CouponAPI"]!;
SD.AuthAPIBase = builder.Configuration["ServiceUrls:AuthAPI"]!;


builder.Services.AddScoped<ICouponRestService, CouponRestService>();
builder.Services.AddScoped<IAuthRestService, AuthRestService>();
builder.Services.AddScoped<ITokenProvider, TokenProvider>();


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
