using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SocialNetwork.Business.Services.Abstracts;
using SocialNetwork.Business.Services.Concretes;
using SocialNetwork.DataAccess.Data;
using SocialNetwork.DataAccess.Repositories.Abstracts;
using SocialNetwork.DataAccess.Repositories.Concretes;
using SocialNetwork.Entities.Entities;
using SocialNetwork.WebUI.Hubs;
using SocialNetwork.WebUI.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
//builder.Services.AddHttpContextAccessor();

builder.Services.AddControllersWithViews();

builder.Services.AddScoped<IImageService,ImageService>();
builder.Services.AddScoped<ICustomIdentityUserDAL, CustomIdentityUserDAL>();
builder.Services.AddScoped<ICustomIdentityUserService, CustomIdentityUserService>();

var connection = builder.Configuration.GetConnectionString("Default");

builder.Services.AddDbContext<SocialNetworkDbContext>(options =>
{
    options.UseSqlServer(connection);
});

builder.Services.AddIdentity<CustomIdentityUser, CustomIdentityRole>()
    .AddEntityFrameworkStores<SocialNetworkDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddSignalR();

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

app.UseAuthentication();

app.UseAuthorization();


app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapHub<ChatHub>("/chathub");

app.Run();
