using System.Windows.Input;
using KoosMatliApp.Models;
using KoosMatliApp.Services;

namespace KoosMatliApp.ViewModels;

public class ApplicationsDetailViewModel : BaseViewModel
{
    private readonly IAdminApiService _api;
    private AdmissionApplication? _application;

    public AdmissionApplication? Application
    {
        get => _application;
        private set
        {
            _application = value;

            OnPropertyChanged(nameof(Application));
            OnPropertyChanged(nameof(CanDecide)); //(Microsoft Learn, 2026a).
            OnPropertyChanged(nameof(LearnerFullName));
            OnPropertyChanged(nameof(Status));
            OnPropertyChanged(nameof(GradeApplyingFor));
            OnPropertyChanged(nameof(DateOfBirth)); //(Microsoft Learn, 2026a).
            OnPropertyChanged(nameof(GuardianName));
            OnPropertyChanged(nameof(GuardianEmail));
            OnPropertyChanged(nameof(GuardianPhone));
        }
    }

    public bool CanDecide => Application is not null &&
        (Status.Equals("New", StringComparison.OrdinalIgnoreCase) ||
         Status.Equals("Pending", StringComparison.OrdinalIgnoreCase));

    public string LearnerFullName =>
        Application?.LearnerFullName ?? string.Empty;

    public string Status =>
        Application?.Status ?? string.Empty;

    public string GradeApplyingFor =>
        Application?.GradeApplyingFor ?? string.Empty;

    public DateTime DateOfBirth =>
        Application?.DateOfBirth ?? DateTime.MinValue;

    public string GuardianName =>
        Application?.GuardianName ?? string.Empty;

    public string GuardianEmail =>
        Application?.GuardianEmail ?? string.Empty;

    public string GuardianPhone =>
        Application?.GuardianPhone ?? string.Empty;

    public ICommand ApproveCommand { get; }
    public ICommand RejectCommand { get; }
    public ICommand PendingCommand { get; } //(Microsoft Learn, 2026b).
    public ICommand BackCommand { get; }

    public ApplicationsDetailViewModel(IAdminApiService api)
    {
        _api = api;
        Title = L.T("Application Details");

        ApproveCommand =
            new Command(async () => await UpdateStatusAsync("Approved"));

        RejectCommand =
            new Command(async () => await UpdateStatusAsync("Rejected"));

        PendingCommand =
            new Command(async () => await UpdateStatusAsync("Pending"));

        BackCommand =
            new Command(async () => await GoBackAsync());
    }

    public async Task LoadAsync(int id)
    {
        if (IsBusy)
            return;

        IsBusy = true;
        HasError = false;
        ErrorMessage = string.Empty;

        try
        {
            Application = await _api.GetApplicationAsync(id);

            if (Application is null)
            {
                HasError = true;
                ErrorMessage = L.T("Application not found.");
            }
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

    public async Task UpdateStatusAsync(string status)
    {
        if (Application is null || IsBusy || !CanDecide)
            return;

        IsBusy = true;
        HasError = false;
        ErrorMessage = string.Empty;

        try
        {
            bool confirmed = await Shell.Current.DisplayAlertAsync( //(Microsoft Learn, 2026b).
                L.T("Confirm"),
                L.T("Change this application status?"),
                L.T("Continue"),
                L.T("Cancel"));

            if (!confirmed)
                return;

            await _api.UpdateApplicationStatusAsync(Application.Id, status);

            Application.Status = status;

            OnPropertyChanged(nameof(Status));
            OnPropertyChanged(nameof(Application));
            OnPropertyChanged(nameof(CanDecide));
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

    private async Task GoBackAsync() =>
        await Shell.Current.GoToAsync(".."); //(Microsoft Learn, 2026c).
}

/*Reference List
 
Microsoft Corporation, "Model-View-ViewModel (MVVM) in .NET MAUI: Property Change Notifications," Microsoft Learn, 2026. [Online]. Available: https://learn.microsoft.com/en-us/dotnet/maui/xaml/mvvm

Microsoft Corporation, "Commanding in .NET MAUI," Microsoft Learn, 2026. [Online]. Available: https://learn.microsoft.com/en-us/dotnet/maui/xaml/fundamentals/commanding

Microsoft Corporation, "Shell Navigation in .NET MAUI," Microsoft Learn, 2026. [Online]. Available: https://learn.microsoft.com/en-us/dotnet/maui/fundamentals/shell/navigation
 */