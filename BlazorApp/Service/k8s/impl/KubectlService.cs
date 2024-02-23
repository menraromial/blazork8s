using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

namespace BlazorApp.Service.k8s.impl;

public class KubectlService : IKubectlService
{
    public async Task<string> Apply(string yaml)
    {
        if (string.IsNullOrWhiteSpace(yaml))
        {
            return string.Empty;
        }

        //yaml 存为文件
        var guid = Guid.NewGuid().ToString();
        var path = $"{guid}.yaml";
        await File.WriteAllTextAsync(path, yaml);


        var output = await Kubectl($"apply -f {path}");
        File.Delete(path);
        return output; // 输出命令输出结果
    }

    public async Task<string> Explain(string filed)
    {
        if (string.IsNullOrWhiteSpace(filed))
        {
            return string.Empty;
        }

        return await Kubectl($" explain {filed}");
    }

    public async Task<string> Describe(string resource, string name)
    {
        return await Kubectl($" describe {resource}  {name}");
    }

    public async Task<string> Delete(string yaml)
    {
        if (string.IsNullOrWhiteSpace(yaml))
        {
            return string.Empty;
        }

        //yaml 存为文件
        var guid = Guid.NewGuid().ToString();
        var path = $"{guid}.yaml";
        Console.WriteLine($"文件名称{path}");
        await File.WriteAllTextAsync(path, yaml);

        var output = await Kubectl($"delete -f {path}");
        Console.WriteLine(output);
        File.Delete(path);
        return output; // 输出命令输出结果
    }

    private static async Task<string> Kubectl(string command)
    {
        Process process = new Process();
        process.StartInfo.FileName               = "kubectl";             // 设置要执行的 kubectl 命令
        process.StartInfo.Arguments              = command;               // 设置命令参数
        process.StartInfo.UseShellExecute        = false;                 // 不使用操作系统的 shell
        process.StartInfo.RedirectStandardOutput = true;                  // 重定向输出
        process.StartInfo.RedirectStandardError  = true;                  // 重定向输出
        process.Start();                                                  // 启动进程
        string output    = await process.StandardOutput.ReadToEndAsync(); // 读取输出
        string errOutput = await process.StandardError.ReadToEndAsync();
        await process.WaitForExitAsync();
        if (!string.IsNullOrEmpty(errOutput))
        {
            output = output + "\r\n" + errOutput;
        }

        return output;
    }
}
