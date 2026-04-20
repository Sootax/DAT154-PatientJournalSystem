

// 1. Create the builder
// "Set up the toolbox"

using CaseSetup.Web.Services;

var builder = WebApplication.CreateBuilder(args);

// 2. Register services
// "Tell the DI container what exists and how to build it."
builder.Services.AddControllersWithViews();
builder.Services.AddSession();

var apiBaseUrl = builder.Configuration["CaseSetupApi:BaseUrl"]
    ?? throw new InvalidOperationException("CaseSetupApi:BaseUrl is not configured.");

builder.Services.AddHttpClient<ICaseApiClient, CaseApiClient>(client =>
    client.BaseAddress = new Uri(apiBaseUrl));

builder.Services.AddHttpClient<IAuthApiClient, AuthApiClient>(client =>
    client.BaseAddress = new Uri(apiBaseUrl));


// 3. Build the app
// "Freeze the registrations, produce a runnable app."
var app = builder.Build();


// 4. Configure the app
// "Build the request pipeline: what happens to every request."
app.UseStaticFiles();
app.UseRouting();
app.UseSession();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Account}/{action=Login}/{id?}");

app.Run();


