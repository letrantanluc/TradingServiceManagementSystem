using Do_An_Chuyen_Nganh.Data;
using Do_An_Chuyen_Nganh.Service.Payment;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Configuration;

var modelbuilder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = modelbuilder.Configuration.GetConnectionString("DefaultConnection");
modelbuilder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
modelbuilder.Services.AddDatabaseDeveloperPageExceptionFilter();

modelbuilder.Services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = false)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();

modelbuilder.Services.AddControllersWithViews();

//momo
var momoSettings = modelbuilder.Configuration.GetSection("MomoSettings").Get<MomoSettings>();
modelbuilder.Services.AddSingleton(momoSettings);
//---
//VNPAY
var vnpaySettings = modelbuilder.Configuration.GetSection("VNPaySettings").Get<VNPaySettings>();
modelbuilder.Services.AddSingleton(vnpaySettings);
//--
modelbuilder.Services.AddDistributedMemoryCache();
modelbuilder.Services.AddSession();
//modelbuilder.Services.AddScoped<RoleManager<IdentityRole>>();

var app = modelbuilder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseSession();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();
