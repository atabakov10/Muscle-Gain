using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MuscleGain.Contracts;
using MuscleGain.Core.Constants;
using MuscleGain.Infrastructure;
using MuscleGain.Infrastructure.Data;
using MuscleGain.Infrastructure.Data.Common;
using MuscleGain.Infrastructure.Data.Models.Account;
using MuscleGain.Services.Proteins;
using MuscleGain.Services.Statistics;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<MuscleGainDbContext>(options =>
    options.UseSqlServer(connectionString, b=> b.MigrationsAssembly("MuscleGain.Infrastructure")));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<ApplicationUser>(options =>
    {
        options.SignIn.RequireConfirmedAccount = true;
        options.Password.RequireNonAlphanumeric = true;
        options.Password.RequiredLength = 6;
        options.User.RequireUniqueEmail = true;
        options.Password.RequireDigit = true;
        options.Password.RequireLowercase = true;
        options.Password.RequiredUniqueChars = 1;

    })
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<MuscleGainDbContext>();
    

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Account/Login";
});

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("CanDeleteProduct", policy =>
        policy.RequireAssertion(context => context.User.IsInRole(RoleConstants.Manager) && context.User.IsInRole(RoleConstants.Supervisor)));
});

builder.Services.AddControllersWithViews();

builder.Services.AddTransient<IStatisticsService, StatisticsService>();

builder.Services.AddTransient<IProteinService, ProteinService>();

builder.Services.AddTransient<IRepository, Repository>();

var app = builder.Build();

app.PrepareDataBase();

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

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();
