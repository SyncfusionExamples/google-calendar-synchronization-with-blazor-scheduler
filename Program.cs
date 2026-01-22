
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Syncfusion.Blazor;

// ADD THIS if your GoogleService is in BlazorScheduler.Data
using BlazorScheduler.Data;

var builder = WebApplication.CreateBuilder(args);

// Services
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();

// If you call Google APIs over HTTP, keep HttpClient available
builder.Services.AddHttpClient();

// ✅ Register your app service (Scoped is recommended for Blazor Server)
builder.Services.AddScoped<GoogleService>();
// If you actually inject an interface, use this instead:

builder.Services.AddSyncfusionBlazor();

var app = builder.Build();

// Pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
