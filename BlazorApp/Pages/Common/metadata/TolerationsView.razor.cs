using System.Collections.Generic;
using k8s.Models;
using Microsoft.AspNetCore.Components;

namespace BlazorApp.Pages.Common.metadata
{
    public partial class TolerationsView : ComponentBase
    {
        [Parameter]
        public IList<V1Toleration> Tolerations { get; set; }
    }
}