using KoosMatliApp.Localization;
using KoosMatliApp.Models;
using KoosMatliApp.Services;

namespace KoosMatliApp.ViewModels;

public class AdminDashboardViewModel : BaseViewModel //(Microsoft Learn, 2026a).
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

        if (!_session.IsConnected) //(Microsoft Learn, 2026a).
        {
            Dashboard = null;
            OnPropertyChanged(nameof(Dashboard));

            HasError = true;
            ErrorMessage = L.T(
                "Connect the signed-in user before loading admin data.");

            return;
        }

        IsBusy = true; //(Microsoft Learn, 2026b).
        HasError = false;
        ErrorMessage = string.Empty;

        try
        {
            Dashboard = await _api.GetDashboardAsync();
            OnPropertyChanged(nameof(Dashboard)); //(Microsoft Learn, 2026b).
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

/*Reference List
 
 Microsoft Corporation, "Model-View-ViewModel (MVVM) in .NET MAUI," Microsoft Learn, 2026. [Online]. Available: https://learn.microsoft.com/en-us/dotnet/maui/xaml/mvvm

 Microsoft Corporation, "Asynchronous Programming Patterns in .NET MAUI Apps," Microsoft Learn, 2026. [Online]. Available: https://learn.microsoft.com/en-us/dotnet/maui/data-cloud/async

 Microsoft Corporation, "INotifyPropertyChanged Interface (System.ComponentModel)," Microsoft Learn, 2026. [Online]. Available: https://learn.microsoft.com/en-us/dotnet/api/system.componentmodel.inotifypropertychanged
 */