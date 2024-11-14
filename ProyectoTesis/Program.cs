using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;
using ProyectoTesis.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

#region Cookie Configuration

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Access/Login";
        options.LogoutPath = "/Access/Logout";
        options.AccessDeniedPath = "/Home/Error";
        options.ExpireTimeSpan = TimeSpan.FromMinutes(60);
        options.SlidingExpiration = true;
    });

#endregion

#region DataBase Configuration

builder.Services.AddTransient<IDbConnection>(db =>

    new SqlConnection(builder.Configuration
    .GetConnectionString("Tesis"))
);
builder.Services.AddDbContext<TesisContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("Tesis"));
});

#endregion

var app = builder.Build();

app.UseCors(
     b => b.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin()
);

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Access}/{action=Login}");

app.Run();