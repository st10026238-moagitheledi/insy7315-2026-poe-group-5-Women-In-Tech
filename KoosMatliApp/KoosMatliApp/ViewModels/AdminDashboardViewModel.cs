using KoosMatliApp.Models;
using KoosMatliApp.Services;

namespace KoosMatliApp.ViewModels;

public class AdminDashboardViewModel : BaseViewModel
{
    private readonly IAdminApiService _api;

    public AdminDashboard? Dashboard { get; private set; }

    public AdminDashboardViewModel(IAdminApiService api)
    {
        _api = api;
        Title = "Admin Dashboard";
    }

    public async Task InitializeAsync()
    {
        if (IsBusy) return;
        IsBusy = true;
        Dashboard = await _api.GetDashboardAsync();
        OnPropertyChanged(nameof(Dashboard));
        IsBusy = false;
    }
}
