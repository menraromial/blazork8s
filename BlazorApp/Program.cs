using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using BlazorApp.Data;
using k8s;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddSingleton<WeatherForecastService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
}


// Load from the default kubeconfig on the machine.
var config = KubernetesClientConfiguration.BuildConfigFromConfigFile();
// Use the config object to create a client.
var client     = new Kubernetes(config);
var namespaces = client.CoreV1.ListNamespace();
foreach (var ns in namespaces.Items)
{
    Console.WriteLine(ns.Metadata.Name);
    var list = client.CoreV1.ListNamespacedPod(ns.Metadata.Name);
    foreach (var item in list.Items)
    {
        Console.WriteLine(item.Metadata.Name);
    }
}

app.UseStaticFiles();

app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
