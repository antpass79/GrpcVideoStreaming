using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using SpawnDev.BlazorJS;
using SpawnDev.BlazorJS.WebWorkers;
using StreamingCore.Actors;
using StreamingViewer;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

builder.Services.AddBlazorJSRuntime();
builder.Services.AddWebWorkerService();

builder.Services.AddSingleton(options => new FrameReceiver());

await builder.Build().BlazorJSRunAsync();
