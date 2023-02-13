using Microsoft.AspNetCore.Mvc;
using NudeContentsLimitInsurance.Controllers;
using NudeContentsLimitInsurance.DataAccess;
using NudeContentsLimitInsurance.Interfaces;
using NudeContentsLimitInsurance.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddTransient<IDataConnection, LocalDbConnector>();

WebApplication app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
}

app.UseStaticFiles();
app.UseRouting();

app.MapControllers();

app.MapFallbackToFile("index.html"); ;

app.Run();