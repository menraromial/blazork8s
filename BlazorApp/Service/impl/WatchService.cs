using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using BlazorApp.Chat;
using k8s;
using k8s.Models;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Hosting;

namespace BlazorApp.Service.impl;

public class WatchService : IWatchService
{
    private readonly IBaseService _baseService;

    private readonly IHubContext<ChatHub> _ctx;
    private          ResourceCache<V1Pod> _cache = ResourceCache<V1Pod>.Instance();

    public WatchService(IBaseService baseService, IHubContext<ChatHub> ctx)
    {
        Console.WriteLine("WatchService 初始化" + DateTime.Now);
        _baseService = baseService;
        _ctx         = ctx;
        WatchAllPod();
    }


    public async Task WatchAllPod()
    {
        var podListResp = _baseService.Client().CoreV1
            .ListPodForAllNamespacesWithHttpMessagesAsync(watch: true);

        await foreach (var (type, item) in podListResp.WatchAsync<V1Pod, V1PodList>())
        {
            Console.WriteLine("==on watch event start ==");
            Console.WriteLine($"{type}:{item.Kind}:{item.Metadata.Name}");
            _cache.Update(type, item);
            Console.WriteLine("==on watch event end ==");
            Console.WriteLine("==on SendAsync event start ==");
            await _ctx.Clients.All.SendAsync("ReceiveMessage", $"{type}:{item.Metadata.Name}");
            Console.WriteLine("==on SendAsync event end ==");
        }
    }
}
