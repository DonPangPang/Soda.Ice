using Masa.Blazor;
using Masa.Blazor.Presets;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Soda.Ice.Client;
using Soda.Ice.Client.Extensions;
using Soda.Ice.Common;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

//builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

builder.Services.AddHttpClient(GlobalVars.ApiBase, x =>
{
    x.BaseAddress = new Uri("http://127.0.0.1:8090");
    //x.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress);
});

builder.Services.AddIce();

builder.Services.AddMasaBlazor(options =>
{
    options.Defaults = new Dictionary<string, IDictionary<string, object?>?>()
    {
        {
            PopupComponents.SNACKBAR, new Dictionary<string, object?>()
            {
                { nameof(PEnqueuedSnackbars.Closeable), true },
                { nameof(PEnqueuedSnackbars.Position), SnackPosition.TopRight }
            }
        }
    };
});

await builder.Build().RunAsync();