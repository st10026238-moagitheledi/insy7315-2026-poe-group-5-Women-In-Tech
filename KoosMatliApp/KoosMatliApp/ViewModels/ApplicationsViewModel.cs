using System.Windows.Input;
using KoosMatliApp.Models;
using KoosMatliApp.Services;
using System.Collections.ObjectModel;

namespace KoosMatliApp.ViewModels;

public class ApplicationsViewModel : BaseViewModel
{
    private readonly IAdminApiService _api;

    public ObservableCollection<AdmissionApplication> Applications { get; } = new();

    public ICommand RefreshCommand { get; }
    public ICommand UpdateStatusCommand { get; }

    public ApplicationsViewModel(IAdminApiService api)
    {
        _api = api;
        Title = "Applications";
        RefreshCommand = new Microsoft.Maui.Controls.Command(async () => await LoadAsync());
        UpdateStatusCommand = new Microsoft.Maui.Controls.Command(async (param) => await UpdateStatusAsync(param));
    }

    public async Task LoadAsync()
    {
        if (IsBusy) return;
        IsBusy = true;
        var list = await _api.GetApplicationsAsync();
        Applications.Clear();
        foreach (var a in list) Applications.Add(a);
        IsBusy = false;
    }

    private async Task UpdateStatusAsync(object? param)
    {
        if (param is not System.Text.Json.JsonElement je)
        {
            // handle AdmissionApplication passed directly
            if (param is AdmissionApplication app)
            {
                // default toggle to Approved for demo; real UI passes desired status
                await _api.UpdateApplicationStatusAsync(app.Id, "Approved");
                await LoadAsync();
            }
            return;
        }

        // If param is provided as a tuple via Binding (we won't rely on this path normally)
    }
}
