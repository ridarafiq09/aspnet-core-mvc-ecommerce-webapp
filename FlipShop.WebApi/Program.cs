using FlipShop.Core.Abstractions;
using FlipShop.Core.Abstractions.Accounts;
using FlipShop.Core.Abstractions.Order;
using FlipShop.Core.Abstractions.Products;
using FlipShop.Core.Contract.Administration;
using FlipShop.Infrastructure;
using FlipShop.Infrastructure.Repositories;
using FlipShop.Infrastructure.Repositories.Accounts;
using FlipShop.Infrastructure.Repositories.Orders;
using FlipShop.Infrastructure.Repositories.Products;
using FlipShop.WebApi.Security.CustomAuthorization;
using FlipShop.WebApi.Security.DP_Api;
using FlipShop.WebApi.Security.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

ConfigurationManager _config = builder.Configuration;
var connString = _config.GetSection("ConnectionStrings");
var googleClientId = builder.Configuration["googleClientId"];
var googleClientSecret = builder.Configuration["googleClientSecret"];


// Register services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContextPool<FlipShopContext>(options => options
                   .UseSqlServer(connString.GetValue<string>("DefaultConnection")));

builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
                   .AddEntityFrameworkStores<FlipShopContext>().AddDefaultTokenProviders();

builder.Services.AddAutoMapper(typeof(Program));

builder.Services.AddAuthentication().AddGoogle(options =>
{
    options.ClientId = googleClientId;
    options.ClientSecret = googleClientSecret;
});


builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AlterRolePolicy",
        policy => policy.RequireClaim("DeleteRole").RequireClaim("CreateRole").RequireClaim("EditRole"));

    options.AddPolicy("EditProductPolicy",
        policy => policy.RequireClaim("EditProduct", "Edit Product")
                        .RequireClaim("AddProduct", "Add Product")
                        .RequireRole("Admin"));
});

//change access denied default path
builder.Services.ConfigureApplicationCookie(options =>
{
    options.AccessDeniedPath = new PathString("/Administrator/AccessDenied");
    options.LoginPath = new PathString("/SignIn");
});

//Override default passsword validation behaviour
builder.Services.Configure<IdentityOptions>(options =>
{
    options.Password.RequiredLength = 10;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequiredUniqueChars = 3;

    options.SignIn.RequireConfirmedEmail = false;

    options.Lockout.MaxFailedAccessAttempts = 5;
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(15);
});

builder.Services.Configure<DataProtectionTokenProviderOptions>(options =>
{
    options.TokenLifespan = TimeSpan.FromHours(5);
});

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
});



builder.Services.AddScoped<IAccountRepository, AccountRepository>();
builder.Services.AddScoped<IAdministration, Administration>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();

builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IUserClaimsPrincipalFactory<ApplicationUser>, ClaimStore>();

builder.Services.AddSingleton<IAuthorizationHandler, CanEditOnlyOtherAdminRolesAndClaimHandler>();
builder.Services.AddSingleton<IAuthorizationHandler, SuperAdminHandler>();

builder.Services.AddSingleton<DataProtectionPurposeStrings>();
#if DEBUG
builder.Services.AddRazorPages().AddRazorRuntimeCompilation();
#endif 

var app = builder.Build();



// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
else
{

    app.UseExceptionHandler("/Home/Error");
    app.UseStatusCodePagesWithReExecute("/Error/{0}");
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseSession();

app.MapControllerRoute(
  name: "areas",
  pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapControllers();

app.Run();
