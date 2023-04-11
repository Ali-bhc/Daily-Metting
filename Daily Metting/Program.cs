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
    string ConnexionString = "Server=(localdb)\\mssqllocaldb;Database=DailyMeetingDB;" +
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



//Add Anas
//Add Admin
if (!await roleManager.RoleExistsAsync("Admin"))
{
    await roleManager.CreateAsync(new IdentityRole("Admin"));
}


if (await userManager.FindByNameAsync("Anas-Ziat") == null)
{
    var user = new User
    {
        UserName = "Anas-Ziat",
        Email = "Anas.Ziat@prettl.com",
        IsAdmin = true,
        Departement = "Admin Service",
        Name = "Anas Ziat"
    };

    var result = await userManager.CreateAsync(user, "AnasZiat123!");
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


//Add Omar
if (await userManager.FindByNameAsync("Omar-Bouhcain") == null)
{
    var user = new User
    {
        UserName = "Omar-Bouhcain",
        Email = "Omar.Bouhcain@prettl.com",
        IsAdmin = false,
        Departement = "Procurement",
        Name = "Omar Bouhcain"
    };

    var result = await userManager.CreateAsync(user, "OmarBouhcain123!");
    if (result.Succeeded)
    {
        await userManager.AddToRoleAsync(user, "Member");
    }
}

//Add Fatima
if (await userManager.FindByNameAsync("Fatima-Nouhou") == null)
{
    var user = new User
    {
        UserName = "Fatima-Nouhou",
        Email = "Fatima.Nouhou@prettl.com",
        IsAdmin = false,
        Departement = "Procurement",
        Name = "Fatima Nouhou"
    };

    var result = await userManager.CreateAsync(user, "FatimaNouhou123!");
    if (result.Succeeded)
    {
        await userManager.AddToRoleAsync(user, "Member");
    }
}

//Add Mohamed Abakouy
if (await userManager.FindByNameAsync("Mohamed-Abakouy") == null)
{
    var user = new User
    {
        UserName = "Mohamed-Abakouy",
        Email = "Mohamed.Abakouy@prettl.com",
        IsAdmin = false,
        Departement = "Procurement",
        Name = "Mohamed Abakouy"
    };

    var result = await userManager.CreateAsync(user, "MohamedAbakouy123!");
    if (result.Succeeded)
    {
        await userManager.AddToRoleAsync(user, "Member");
    }
}

//Add Ikram Zeroual
if (await userManager.FindByNameAsync("Ikram-Zeroual") == null)
{
    var user = new User
    {
        UserName = "Ikram-Zeroual",
        Email = "Ikram.Zeroual@prettl.com",
        IsAdmin = false,
        Departement = "CS_PP",
        Name = "Ikram Zeroual"
    };

    var result = await userManager.CreateAsync(user, "IkramZeroual123!");
    if (result.Succeeded)
    {
        await userManager.AddToRoleAsync(user, "Member");
    }
}

//Add Ayoub
if (await userManager.FindByNameAsync("Ayoub-Elyemlahi") == null)
{
    var user = new User
    {
        UserName = "Ayoub-Elyemlahi",
        Email = "Ayoub.Elyemlahi@prettl.com",
        IsAdmin = false,
        Departement = "CS_PP",
        Name = "Ayoub Elyemlahi"
    };

    var result = await userManager.CreateAsync(user, "AyoubElyemlahi123!");
    if (result.Succeeded)
    {
        await userManager.AddToRoleAsync(user, "Member");
    }
}

//Add Ghizlane
if (await userManager.FindByNameAsync("Ghizlane-Benhdech") == null)
{
    var user = new User
    {
        UserName = "Ghizlane-Benhdech",
        Email = "Ghizlane.Benhdech@prettl.com",
        IsAdmin = false,
        Departement = "CS_PP",
        Name = "Ghizlane Benhdech"
    };

    var result = await userManager.CreateAsync(user, "GhizlaneBenhdech123!");
    if (result.Succeeded)
    {
        await userManager.AddToRoleAsync(user, "Member");
    }
}

//Add Saloua
if (await userManager.FindByNameAsync("Saloua-Guenoun") == null)
{
    var user = new User
    {
        UserName = "Saloua-Guenoun",
        Email = "Saloua.Guenoun@prettl.com",
        IsAdmin = false,
        Departement = "CS_PP",
        Name = "Saloua Guenoun"
    };

    var result = await userManager.CreateAsync(user, "SalouaGuenoun123!");
    if (result.Succeeded)
    {
        await userManager.AddToRoleAsync(user, "Member");
    }
}
//WareHouse
//Add Mohamed senouni
if (await userManager.FindByNameAsync("Mohamed-Senouni") == null)
{
    var user = new User
    {
        UserName = "Mohamed-Senouni",
        Email = "Mohamed.Senouni@prettl.com",
        IsAdmin = false,
        Departement = "WH",
        Name = "Mohamed Senouni"
    };

    var result = await userManager.CreateAsync(user, "MohamedSenouni123!");
    if (result.Succeeded)
    {
        await userManager.AddToRoleAsync(user, "Member");
    }
}


//Add Bouchta Rharaiba
if (await userManager.FindByNameAsync("Bouchta-Rharaiba") == null)
{
    var user = new User
    {
        UserName = "Bouchta-Rharaiba",
        Email = "Bouchta.Rharaiba@prettl.com",
        IsAdmin = false,
        Departement = "WH",
        Name = "Bouchta Rharaiba"
    };

    var result = await userManager.CreateAsync(user, "BouchtaRharaiba123!");
    if (result.Succeeded)
    {
        await userManager.AddToRoleAsync(user, "Member");
    }
}

//Add Khalid Elhermich
if (await userManager.FindByNameAsync("Khalid-Elhermich") == null)
{
    var user = new User
    {
        UserName = "Khalid-Elhermich",
        Email = "Khalid.Elhermich@prettl.com",
        IsAdmin = false,
        Departement = "WH",
        Name = "Khalid Elhermich"
    };

    var result = await userManager.CreateAsync(user, "KhalidElhermich123!");
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
