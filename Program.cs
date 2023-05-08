using CC.Areas.Identity;
using CC.Authorization;
using CC.Data;
using CC.Helpers;
using CC.Hubs;
using CC.Identity;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using MudBlazor.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<DataContext>(options => options.UseSqlServer(connectionString), ServiceLifetime.Singleton);
builder.Services.AddDbContext<IdentityContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("IdentityConnection")));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();
builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true).AddRoles<IdentityRole>().AddEntityFrameworkStores<IdentityContext>();
builder.Services.AddDbContextFactory<DataContext>(opt => opt.UseSqlServer(connectionString));
//builder.Services.AddAuthentication()
//   .AddGoogle(options =>
//   {
//       IConfigurationSection googleAuthNSection = builder.Configuration.GetSection("Authentication:Google");
//       options.ClientId = googleAuthNSection["ClientId"];
//       options.ClientSecret = googleAuthNSection["ClientSecret"];
//   })
//   .AddMicrosoftAccount(microsoftOptions =>
//   {
//       microsoftOptions.ClientId = builder.Configuration["Authentication:Microsoft:ClientId"];
//       microsoftOptions.ClientSecret = builder.Configuration["Authentication:Microsoft:ClientSecret"];
//   });
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddScoped<AuthenticationStateProvider, RevalidatingIdentityAuthenticationStateProvider<IdentityUser>>();
builder.Services.AddScoped<ConnectedUser>();
builder.Services.AddScoped<ConnectedUserList>();
builder.Services.AddScoped<HubConnectionCollection>();
builder.Services.AddMudServices();
builder.Services.AddSingleton<WeatherForecastService>();
//builder.Services.AddSingleton<IAuthorizationHandler, ProfileOwnerHandler>();
//builder.Services.AddAuthorization(config => config.AddPolicy("ProfileOwner", policy => policy.Requirements.Add(new ProfileOwnerRequirement())));
builder.Services.AddHttpContextAccessor();
//builder.Services.AddScoped<XCookieAuthEvents>();

//// optional: customize cookie expiration time
//builder.Services.ConfigureApplicationCookie(ops =>
//{
//    ops.EventsType = typeof(XCookieAuthEvents);
//    ops.ExpireTimeSpan = TimeSpan.FromMinutes(30);
//    ops.SlidingExpiration = true;
//});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapBlazorHub();
app.MapFallbackToPage("/Host");
app.MapHub<ChatHub>(ChatHub.HubUrl);

app.Run();
