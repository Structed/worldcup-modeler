using System.Text.Json;
using Microsoft.JSInterop;

namespace WorldCupModeler.Services;

/// <summary>Thin wrapper over the browser's localStorage via JS interop.</summary>
public class LocalStorageService(IJSRuntime js)
{
    private static readonly JsonSerializerOptions Options = new(JsonSerializerDefaults.Web);

    public async Task<T?> GetAsync<T>(string key)
    {
        var json = await js.InvokeAsync<string?>("localStorage.getItem", key);
        if (string.IsNullOrEmpty(json)) return default;
        try
        {
            return JsonSerializer.Deserialize<T>(json, Options);
        }
        catch (JsonException)
        {
            return default;
        }
    }

    public async Task SetAsync<T>(string key, T value)
    {
        var json = JsonSerializer.Serialize(value, Options);
        await js.InvokeVoidAsync("localStorage.setItem", key, json);
    }

    public async Task RemoveAsync(string key) =>
        await js.InvokeVoidAsync("localStorage.removeItem", key);
}
