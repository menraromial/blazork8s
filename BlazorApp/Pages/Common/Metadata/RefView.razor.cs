using System.Threading.Tasks;
using AntDesign;
using BlazorApp.Pages.ClusterRole;
using BlazorApp.Pages.CronJob;
using BlazorApp.Pages.DaemonSet;
using BlazorApp.Pages.Deployment;
using BlazorApp.Pages.Job;
using BlazorApp.Pages.Node;
using BlazorApp.Pages.PersistentVolumeClaim;
using BlazorApp.Pages.Pod;
using BlazorApp.Pages.ReplicaSet;
using BlazorApp.Pages.ReplicationController;
using BlazorApp.Pages.Role;
using BlazorApp.Pages.ServiceAccount;
using BlazorApp.Pages.StatefulSet;
using BlazorApp.Service.k8s;
using BlazorApp.Utils;
using k8s.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;

namespace BlazorApp.Pages.Common.Metadata;

public partial class RefView : ComponentBase
{
    [Parameter]
    public V1ObjectReference Ref { get; set; }

    [Parameter]
    public bool FullView { get; set; } = false;

    [Inject]
    private IMessageService MessageService { get; set; }

    [Inject]
    private IServiceAccountService ServiceAccountService { get; set; }

    [Inject]
    private IPersistentVolumeService PersistentVolumeService { get; set; }

    [Inject]
    private IPersistentVolumeClaimService PersistentVolumeClaimService { get; set; }

    [Inject]
    private IClusterRoleService ClusterRoleService { get; set; }

    [Inject]
    private IRoleService RoleService { get; set; }

    [Inject]
    private IReplicaSetService ReplicaSetService { get; set; }

    [Inject]
    private INodeService NodeService { get; set; }

    [Inject]
    private IJobService JobService { get; set; }

    [Inject]
    private ICronJobService CronJobService { get; set; }

    [Inject]
    private IPodService PodService { get; set; }

    [Inject]
    private IDeploymentService DeploymentService { get; set; }

    [Inject]
    private IDaemonSetService DaemonSetService { get; set; }

    [Inject]
    private IStatefulSetService StatefulSetService { get; set; }

    [Inject]
    private IReplicationControllerService ReplicationControllerService { get; set; }

    [Inject]
    private ILogger<ControllerBy> Logger { get; set; }

    [Inject]
    private IMessageService Message { get; set; }

    [Inject]
    private DrawerService DrawerService { get; set; }

    private async Task OnRsNameClick(string rsName)
    {
        var item = ReplicaSetService.GetByName(rsName);
        await PageDrawerHelper<V1ReplicaSet>.Instance
            .SetDrawerService(DrawerService)
            .ShowDrawerAsync<ReplicaSetDetailView, V1ReplicaSet, bool>(item);
    }

    private async Task OnNodeNameClick(string nodeName)
    {
        var item = NodeService.GetByName(nodeName);
        await PageDrawerHelper<V1Node>.Instance
            .SetDrawerService(DrawerService)
            .ShowDrawerAsync<NodeDetailView, V1Node, bool>(item);
    }

    private async Task OnDeploymentNameClick(string name)
    {
        var item = DeploymentService.GetByName(name);
        await PageDrawerHelper<V1Deployment>.Instance
            .SetDrawerService(DrawerService)
            .ShowDrawerAsync<DeploymentDetailView, V1Deployment, bool>(item);
    }

    private async Task OnDaemonSetNameClick(string name)
    {
        var item = DaemonSetService.GetByName(name);
        await PageDrawerHelper<V1DaemonSet>.Instance
            .SetDrawerService(DrawerService)
            .ShowDrawerAsync<DaemonSetDetailView, V1DaemonSet, bool>(item);
    }

    private async Task OnReplicationControllerNameClick(string name)
    {
        var item = ReplicationControllerService.GetByName(name);
        await PageDrawerHelper<V1ReplicationController>.Instance
            .SetDrawerService(DrawerService)
            .ShowDrawerAsync<ReplicationControllerDetailView, V1ReplicationController, bool>(item);
    }

    private Task OnXClick(string name)
    {
        Logger.LogError($"{name}点击未实现");
        Message.Error($"{name}点击未实现");
        return Task.CompletedTask;
    }


    private Task OnObjClick()
    {
        var name = Ref.Name;
        var kind = Ref.Kind;

        var task = kind switch
        {
            "Deployment"            => OnDeploymentNameClick(name),
            "DaemonSet"             => OnDaemonSetNameClick(name),
            "StatefulSet"           => OnStatefulSetNameClick(name),
            "ReplicationController" => OnReplicationControllerNameClick(name),
            "ReplicaSet"            => OnRsNameClick(name),
            "Node"                  => OnNodeNameClick(name),
            "Job"                   => OnJobNameClick(name),
            "CronJob"               => OnCronJobNameClick(name),
            "Pod"                   => OnPodNameClick(name),
            "Group"                 => OnGroupNameClick(name),
            "User"                  => OnUserNameClick(name),
            "ServiceAccount"        => OnServiceAccountNameClick(name),
            "ClusterRole"           => OnClusterRoleNameClick(name),
            "Role"                  => OnRoleNameClick(name),
            "PersistentVolumeClaim" => OnPersistentVolumeClaimNameClick(name),
            _                       => OnXClick(name)
        };

        return task;
    }

    private async Task OnPersistentVolumeClaimNameClick(string name)
    {
        var item = PersistentVolumeClaimService.GetByName(name);
        await PageDrawerHelper<V1PersistentVolumeClaim>.Instance
            .SetDrawerService(DrawerService)
            .ShowDrawerAsync<PersistentVolumeClaimDetailView, V1PersistentVolumeClaim, bool>(item);
    }

    private async Task OnUserNameClick(string name)
    {
        await MessageService.Info(name);
    }

    private async Task OnGroupNameClick(string name)
    {
        await MessageService.Info(name);
    }

    private async Task OnServiceAccountNameClick(string name)
    {
        var item = ServiceAccountService.GetByName(name);
        if (item == null)
        {
            await MessageService.Error($"ServiceAccount {name} Not Found");
            return;
        }

        await PageDrawerHelper<V1ServiceAccount>.Instance
            .SetDrawerService(DrawerService)
            .ShowDrawerAsync<ServiceAccountDetailView, V1ServiceAccount, bool>(item);
    }

    private async Task OnClusterRoleNameClick(string name)
    {
        var item = ClusterRoleService.GetByName(name);
        await PageDrawerHelper<V1ClusterRole>.Instance
            .SetDrawerService(DrawerService)
            .ShowDrawerAsync<ClusterRoleDetailView, V1ClusterRole, bool>(item);
    }

    private async Task OnRoleNameClick(string name)
    {
        var item = RoleService.GetByName(name);
        await PageDrawerHelper<V1Role>.Instance
            .SetDrawerService(DrawerService)
            .ShowDrawerAsync<RoleDetailView, V1Role, bool>(item);
    }

    private async Task OnPodNameClick(string name)
    {
        var item = PodService.GetByName(name);
        await PageDrawerHelper<V1Pod>.Instance
            .SetDrawerService(DrawerService)
            .ShowDrawerAsync<PodDetailView, V1Pod, bool>(item);
    }

    private async Task OnCronJobNameClick(string name)
    {
        var item = CronJobService.GetByName(name);

        await PageDrawerHelper<V1CronJob>.Instance
            .SetDrawerService(DrawerService)
            .ShowDrawerAsync<CronJobDetailView, V1CronJob, bool>(item);
    }

    private async Task OnJobNameClick(string name)
    {
        var item = JobService.GetByName(name);
        if (item == null)
        {
            await Message.Error($"Job {name} 已被删除");
            return;
        }

        await PageDrawerHelper<V1Job>.Instance
            .SetDrawerService(DrawerService)
            .ShowDrawerAsync<JobDetailView, V1Job, bool>(item);
    }

    private async Task OnStatefulSetNameClick(string name)
    {
        var item = StatefulSetService.GetByName(name);
        await PageDrawerHelper<V1StatefulSet>.Instance
            .SetDrawerService(DrawerService)
            .ShowDrawerAsync<StatefulSetDetailView, V1StatefulSet, bool>(item);
    }
}
