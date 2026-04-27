using TeacherAssessment.Components;
using TeacherAssessment.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

var apiBaseUrl = builder.Configuration["BackendApi:BaseUrl"]
    ?? throw new InvalidOperationException("BackendApi:BaseUrl is not configured.");

builder.Services.AddHttpClient<IAuthApiClient, AuthApiClient>(client =>
    client.BaseAddress = new Uri(apiBaseUrl));

builder.Services.AddHttpClient<ISessionApiClient, SessionApiClient>(client =>
    client.BaseAddress = new Uri(apiBaseUrl));

builder.Services.AddHttpClient<IObservationApiClient, ObservationApiClient>(client =>
    client.BaseAddress = new Uri(apiBaseUrl));

builder.Services.AddScoped<TeacherSessionState>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);
app.UseHttpsRedirection();

app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();