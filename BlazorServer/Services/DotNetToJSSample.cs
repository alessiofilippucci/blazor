using Microsoft.JSInterop;

namespace BlazorServer.Services;

public class DotNetToJSSample
{
    private string name;

    public string Name { get => name; set => name = value; }

    [JSInvokable]
    public Task<List<int>> GetDataAsync()
    {
        List<int> data = new List<int>();

        var rdm = new Random();

        foreach (var _ in Enumerable.Range(1, 10))
        {
            data.Add(rdm.Next(0, 100));
        }

        return Task.FromResult(data);
    }

    [JSInvokable]
    public string GetWelcomeMessage() => $"Welcome, {name}";
}
