using Microsoft.AspNetCore.Authentication.Cookies;
using TodoListApp.WebApp.Services.Implementation;
using TodoListApp.WebApp.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddHttpClient("WebApi", client =>
{
    client.BaseAddress = new Uri(builder.Configuration["WebApiBaseUrl"]!);
});

builder.Services.AddHttpContextAccessor();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/auth/login";
        options.AccessDeniedPath = "/auth/accessDenied";
        options.ExpireTimeSpan = TimeSpan.FromMinutes(60);
    });

builder.Services.AddScoped<ITodoListWebApiService, TodoListWebApiService>();
builder.Services.AddScoped<ITodoTaskWebApiService, TodoTaskWebApiService>();
builder.Services.AddScoped<IAuthServiceWebApiService, AuthServiceWebApiService>();
builder.Services.AddScoped<IClientAuth, ClientAuth>();
builder.Services.AddScoped<IDashboardWebApiService, DashboardWebApiService>();
builder.Services.AddScoped<IAssignedTasksWebApiService, AssignedTasksWebApiService>();

builder.Services.AddRouting(opts => opts.LowercaseUrls = true);

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

await app.RunAsync();
