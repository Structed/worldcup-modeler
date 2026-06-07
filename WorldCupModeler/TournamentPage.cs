using Microsoft.AspNetCore.Components;
using WorldCupModeler.Services;

namespace WorldCupModeler;

/// <summary>Base page that loads tournament state once and re-renders on changes.</summary>
public abstract class TournamentPage : ComponentBase, IDisposable
{
    [Inject] protected TournamentService Tournament { get; set; } = default!;

    protected override async Task OnInitializedAsync()
    {
        await Tournament.EnsureLoadedAsync();
        Tournament.OnChange += OnStateChanged;
    }

    private void OnStateChanged() => InvokeAsync(StateHasChanged);

    protected static int? ParseScore(object? value)
    {
        var text = value?.ToString();
        if (string.IsNullOrWhiteSpace(text)) return null;
        return int.TryParse(text, out var n) && n >= 0 ? n : null;
    }

    public void Dispose() => Tournament.OnChange -= OnStateChanged;
}
