using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MuscleGain.Core.Constants;
using MuscleGain.Extensions;
using MuscleGain.Infrastructure;
using MuscleGain.Infrastructure.Data;
using MuscleGain.Infrastructure.Data.Models.Account;
using MuscleGain.ModelBinders;

var builder = WebApplication.CreateBuilder(args);

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


builder.Services.AddAuthentication()
    .AddFacebook(options =>
    {
        options.AppId = "1432800920459702";
        options.AppSecret = "412639861b83dfb0266c0edb515fc6ef";
    });

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Account/Login";
    options.LogoutPath = "/Account/Logout";
    options.AccessDeniedPath = "/Account/AccessDenied";
});

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("CanDeleteProduct", policy =>
        policy.RequireAssertion(context => context.User.IsInRole(RoleConstants.Manager) && 
                                           context.User.IsInRole(RoleConstants.Supervisor) && 
                                           context.User.IsInRole(RoleConstants.Administrator)));
});

builder.Services.AddControllersWithViews()
    .AddMvcOptions(options =>
    {
        options.ModelBinderProviders.Insert(0, new DecimalModelBinderProvider());
    });

builder.Services.AddApplicationServices();

builder.Services.AddResponseCaching();

var app = builder.Build();

app.PrepareDataBase();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");

    app.UseHsts();
}

app.Use(async (context, next) =>
{ 
    context.Request.Scheme = "https";

    await next();
});

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapRazorPages();
app.UseResponseCaching();

app.Run();
