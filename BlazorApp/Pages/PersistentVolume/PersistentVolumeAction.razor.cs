using System;
using System.Threading.Tasks;
using AntDesign;
using BlazorApp.Pages.Common.Metadata;
using BlazorApp.Pages.Workload;
using BlazorApp.Service;
using BlazorApp.Service.k8s;
using k8s.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;

namespace BlazorApp.Pages.PersistentVolume;

public partial class PersistentVolumeAction : ComponentBase
{
    [Parameter]
    public V1PersistentVolume Item { get; set; }

    [Parameter]
    public MenuMode MenuMode { get; set; }=MenuMode.Vertical;

    [Inject]
    private IPersistentVolumeService PersistentVolumeService { get; set; }


    [Inject]
    private IPageDrawerService PageDrawerService { get; set; }

    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();
    }

    private async Task OnDeleteClick(V1PersistentVolume item)
    {
        await PersistentVolumeService.Delete(item.Namespace(), item.Name());
        StateHasChanged();
    }

 

    private async Task OnYamlClick(V1PersistentVolume item)
    {
        var options = PageDrawerService.DefaultOptions($"Yaml:{item.Name()}", width: 1000);
        await PageDrawerService.ShowDrawerAsync<YamlView<V1PersistentVolume>, V1PersistentVolume, bool>(options, item);
    }

    private async Task OnDocClick(V1PersistentVolume item)
    {
        var options = PageDrawerService.DefaultOptions($"Doc:{item.Name()}", width: 1000);
        await PageDrawerService.ShowDrawerAsync<DocTreeView<V1PersistentVolume>, V1PersistentVolume, bool>(options, item);
    }
}