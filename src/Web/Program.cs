using API.Extensions.Bootstrapper;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();

builder.Services.AddApplication();
builder.Services.AddInfrastructure();
builder.Services.AddAuthServices(builder.Configuration);

var app = builder.Build();

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
