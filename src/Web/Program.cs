using API.Extensions.Bootstrapper;
using Infrastructure.Services.Init;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();

builder.Services.AddApplication();
builder.Services.AddInfrastructure();
builder.Services.AddAuthServices(builder.Configuration);

var app = builder.Build();

try
{
    using var scope = app.Services.CreateScope();
    var dbInit = scope.ServiceProvider.GetRequiredService<DataBaseInitializer>();
    await dbInit.InitializeAsync();
}
catch (Exception ex)
{
    var logger = app.Services.GetRequiredService<ILogger<Program>>();
    logger.LogCritical(ex, "Проблема с инициализацией базы данных");
    throw;
}

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseRouting();
app.UseCustomMiddleware();

app.UseAuthorization();
app.MapEndpoints();

app.MapStaticAssets();
app.MapRazorPages()
    .WithStaticAssets();

app.Run();
