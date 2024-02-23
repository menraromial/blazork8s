using System.Threading.Tasks;

namespace BlazorApp.Service.k8s;

public interface IKubectlService
{
    //调用kubectl执行命令
    public Task<string> Apply(string    yaml);
    public Task<string> Delete(string   yaml);
    public Task<string> Explain(string  filed);
    public Task<string> Describe(string resource, string name);
    public Task<string> Describe(string resourceAndName);
}
