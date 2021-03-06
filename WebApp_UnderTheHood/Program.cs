using Microsoft.AspNetCore.Authorization;
using WebApp_UnderTheHood.Authorization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddAuthentication("MyCookieAuth").AddCookie("MyCookieAuth", opt =>
{
    opt.Cookie.Name = "MyCookieAuth";
    opt.LoginPath = "/Account/Login";
    opt.AccessDeniedPath = "/Account/AccesDenied";
    opt.ExpireTimeSpan = TimeSpan.FromSeconds(30);
});

builder.Services.AddAuthorization(opt =>
{
    opt.AddPolicy("MustBelongToHRDepartment",
        policy => policy.RequireClaim("Department", "HR"));

    opt.AddPolicy("AdminOnly",
        policy => policy.RequireClaim("Admin"));

    opt.AddPolicy("HRManagerOnly",
        policy => policy.RequireClaim("Department", "HR")
        .RequireClaim("Manager")
        .Requirements.Add(new HRManagerProbationRequirement(3)));
});

builder.Services.AddSingleton<IAuthorizationHandler, HRManagerProbationRequirementHandler>();

builder.Services.AddHttpClient("OurWebApi", client =>
{
    client.BaseAddress = new Uri("https://localhost:44325/");
});

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

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();

app.Run();
