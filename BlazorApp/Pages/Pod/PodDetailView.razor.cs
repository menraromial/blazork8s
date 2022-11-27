using System.Threading.Tasks;
using AntDesign;
using BlazorApp.Pages.Node;
using BlazorApp.Service;
using Entity;
using k8s.Models;
using Microsoft.AspNetCore.Components;

namespace BlazorApp.Pages.Pod
{
    public partial class PodDetailView : FeedbackComponent<V1Pod, bool>
    {
        [Inject]
        private INodeService NodeService { get; set; }

        [Inject]
        private IPageDrawerService PageDrawerService { get; set; }

        public V1Pod PodItem;

        protected override void OnInitialized()
        {
            PodItem = base.Options;
            base.OnInitialized();
        }

        private async Task OnNodeNameClick(string nodeName)
        {
            var nodeVo  = await NodeService.GetNodeVOWithPodListByNodeName(nodeName);
            var options = PageDrawerService.DefaultOptions($"Node:{nodeName}");
            await PageDrawerService.ShowDrawerAsync<NodeDetailView, NodeVO, bool>(options, nodeVo);
        }
    }
}
