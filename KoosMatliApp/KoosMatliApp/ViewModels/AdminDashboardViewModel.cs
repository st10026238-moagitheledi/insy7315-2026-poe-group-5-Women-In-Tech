using KoosMatliApp.Localization;
using KoosMatliApp.Models;
using KoosMatliApp.Services;

namespace KoosMatliApp.ViewModels;

public class AdminDashboardViewModel : BaseViewModel
{
    private readonly IAdminApiService _api;
    private readonly AdminSession _session;

    public AdminDashboard? Dashboard { get; private set; }

    public AdminDashboardViewModel(
        IAdminApiService api,
        AdminSession session)
    {
        _api = api;
        _session = session;

        Title = L.T("Admin Dashboard");
    }

    public async Task InitializeAsync()
    {
        if (IsBusy)
            return;

        if (!_session.IsConnected)
        {
            Dashboard = null;
            OnPropertyChanged(nameof(Dashboard));

            HasError = true;
            ErrorMessage = L.T(
                "Connect the signed-in user before loading admin data.");

            return;
        }

        IsBusy = true;
        HasError = false;
        ErrorMessage = string.Empty;

        try
        {
            Dashboard = await _api.GetDashboardAsync();
            OnPropertyChanged(nameof(Dashboard));
        }
        catch (Exception ex)
        {
            HasError = true;
            ErrorMessage = ex.Message;
        }
        finally
        {
            IsBusy = false;
        }
    }
}