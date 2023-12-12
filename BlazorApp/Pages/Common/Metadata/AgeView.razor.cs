using System;
using System.Threading.Tasks;
using System.Timers;
using Extension;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;

namespace BlazorApp.Pages.Common.Metadata;

public partial class AgeView : ComponentBase,IDisposable
{
    [Parameter]
    public DateTime? Age { get; set; }

    [Inject]
    private ILogger<AgeView> Logger { get; set; }

    private string _kubeAge;
    private Timer  _timer;

    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();
        _timer         =  new Timer(1000);
        _timer.Elapsed += (sender, eventArgs) => OnTimerCallback();
        _timer.Start();
    }

    private void OnTimerCallback()
    {
         _kubeAge = Age.AgeFromUtc();
         InvokeAsync(StateHasChanged);
    }
    public void Dispose()
    {
        _timer.Dispose();
    }
}