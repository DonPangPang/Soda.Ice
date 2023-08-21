using Soda.AutoMapper;
using Soda.Ice.Common.Helpers;
using Soda.Ice.Domain;
using Soda.Ice.Shared.ViewModels;
using Soda.Ice.WebApi.Filters;
using Soda.Ice.WebApi.Options;
using Soda.Ice.WebApi.Setups;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllersWithViews(setup =>
{
    setup.ReturnHttpNotAcceptable = true;
    setup.Filters.Add<AsyncExceptionFilter>();
}).AddJson();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddRazorPages();

builder.Services.AddIce();

builder.WebHost.UseUrls("http://*:8090/");

var app = builder.Build();

app.InitSodaMapper();

app.UseCors();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseBlazorFrameworkFiles();
app.UseStaticFiles(new StaticFileOptions
{
    ServeUnknownFileTypes = true
    //ContentTypeProvider = new FileExtensionContentTypeProvider(new Dictionary<string, string>
    //        {
    //                { ".apk", "application/vnd.android.package-archive" }
    //        })
});

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

var defaultFilesOptions = new DefaultFilesOptions();
defaultFilesOptions.DefaultFileNames.Clear();
defaultFilesOptions.DefaultFileNames.Add("index.html");
app.UseDefaultFiles(defaultFilesOptions);
app.UseStaticFiles();

app.Run();