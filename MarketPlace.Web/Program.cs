using MarketPlace.Application.DependencyInjection;
using MarketPlace.Infrastructure.DependencyInjection;
using MarketPlace.Infrastructure.Entities;
using MarketPlace.Web.Services;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

// Add Infrastructure Services (DbContext, Identity, Repositories)
builder.Services.AddInfrastructureServices(builder.Configuration);

// Add Application Services
builder.Services.AddApplicationServices();

// Web-specific services
builder.Services.AddScoped<IEmailSender, EmailSender>();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Identity/Account/Login";
    options.AccessDeniedPath = "/Identity/Account/AccessDenied";
});

builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<CartService>();

builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});


// Add services to the container.
builder.Services.AddRazorPages();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseSession();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
// Ensure database is created and migrated
await app.Services.EnsureDatabaseCreatedAsync();

app.MapRazorPages();

//app.Run("http//:0.0.0.0:5000");
app.Run();
