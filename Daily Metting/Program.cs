using Daily_Metting.DAO;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Daily_Metting.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using Microsoft.AspNetCore.Authorization;
using Daily_Metting.Repositories;
using Microsoft.AspNetCore.Mvc.Razor;
using Daily_Metting.Services;
using Microsoft.AspNetCore.Http.Features;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<DailyMeetingDbContext>(op => {
    string ConnexionString = "Server=(localdb)\\mssqllocaldb;Database=DailyMeetingDB1999;" +
    "Trusted_Connection=True;MultipleActiveResultSets=true";
    op.UseSqlServer(ConnexionString);
    });

////Background services
//builder.Services.AddScoped<SampleService>();
//builder.Services.AddSingleton<PeriodicHostedService>();
//builder.Services.AddHostedService(provider => provider.GetRequiredService<PeriodicHostedService>());

builder.Services.AddHostedService<DailyMissedSubmissionService>();



//Authentification

builder.Services.AddIdentity<User, IdentityRole>()
    .AddEntityFrameworkStores<DailyMeetingDbContext>()
    .AddDefaultUI()
    .AddDefaultTokenProviders();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Account/Login";
    options.LogoutPath = "/Account/Logout";
});
builder.Services.AddScoped<IPointRepository, PointRepository>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<ISubmissionRepository, SubmissionRepository>();
builder.Services.AddScoped<IAbsencesRepository, AbsenceRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IValueRepository, ValueRepository>();
builder.Services.AddScoped<IAttainementRepository, AttaienementRepository>();
builder.Services.AddScoped<IAPURepository, APURepository>();



//builder.Services.AddAuthentication(options =>
//{
//    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
//    options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
//    options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
//})
//    .AddCookie();


builder.Services.AddControllersWithViews();

//please for adding migration you should Comment this block of code(Add-Admin);
//Add Admin
var roleManager = builder.Services.BuildServiceProvider().GetRequiredService<RoleManager<IdentityRole>>();
var userManager = builder.Services.BuildServiceProvider().GetRequiredService<UserManager<User>>();

if (!await roleManager.RoleExistsAsync("Admin"))
{
    await roleManager.CreateAsync(new IdentityRole("Admin"));
}


if (await userManager.FindByNameAsync("admin") == null)
{
    var user = new User
    {
        UserName = "admin",
        Email = "admin@example.com",
        IsAdmin = true,
        Departement = "Admin Service",
        Name = "admin test"
    };

    var result = await userManager.CreateAsync(user, "Admin123!");
    if (result.Succeeded)
    {
        await userManager.AddToRoleAsync(user, "Admin");
    }
}

//Add Member
if (!await roleManager.RoleExistsAsync("Member"))
{
    await roleManager.CreateAsync(new IdentityRole("Member"));
}


if (await userManager.FindByNameAsync("CS_PP") == null)
{
    var user = new User
    {
        UserName = "CS_PP",
        Email = "CS_PP@example.com",
        IsAdmin = true,
        Departement = "CS_PP",
        Name = "CS_PP test"
    };

    var result = await userManager.CreateAsync(user, "Admin123!");
    if (result.Succeeded)
    {
        await userManager.AddToRoleAsync(user, "Member");
    }
}


builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminPolicy", policy => policy.RequireRole("Admin"));
});
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("MemberPolicy", policy => policy.RequireRole("Member"));
});

//Upload
builder.Services.Configure<IISServerOptions>(options =>
{
    options.MaxRequestBodySize = int.MaxValue;
});
builder.Services.Configure<FormOptions>(options =>
{
    options.ValueLengthLimit = int.MaxValue;
    options.MultipartBodyLengthLimit = int.MaxValue;
    options.MemoryBufferThreshold = int.MaxValue;
});


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
app.UseAuthentication();;

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}");



DBInitializer.Seed(app);

app.Run();
