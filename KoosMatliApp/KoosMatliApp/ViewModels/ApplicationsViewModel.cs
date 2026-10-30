using System.Collections.ObjectModel;
using System.Windows.Input;
using KoosMatliApp.Models;
using KoosMatliApp.Services;

namespace KoosMatliApp.ViewModels;

public class ApplicationsViewModel : BaseViewModel
{
    private readonly IAdminApiService _api;
    private string _searchText = string.Empty;
    private bool _filterNew;
    private bool _filterPending;
    private bool _filterApproved;
    private bool _filterRejected;

    public ObservableCollection<AdmissionApplication> Applications { get; } = new();

    public string SearchText
    {
        get => _searchText;
        set
        {
            if (SetProperty(ref _searchText, value))
                RefreshFiltered();
        }
    }

    public bool IsFilterNew => _filterNew;
    public bool IsFilterPending => _filterPending;
    public bool IsFilterApproved => _filterApproved;
    public bool IsFilterRejected => _filterRejected;

    public ICommand RefreshCommand { get; }
    public ICommand FilterAllCommand { get; }
    public ICommand FilterNewCommand { get; }
    public ICommand FilterPendingCommand { get; }
    public ICommand FilterApprovedCommand { get; }
    public ICommand FilterRejectedCommand { get; }
    public ICommand DetailCommand { get; }

    public IReadOnlyList<AdmissionApplication> FilteredApplications
    {
        get
        {
            IEnumerable<AdmissionApplication> query = Applications;

            if (_filterNew)
            {
                query = query.Where(a =>
                    a.Status.Equals("New", StringComparison.OrdinalIgnoreCase));
            }
            else if (_filterPending)
            {
                query = query.Where(a =>
                    a.Status.Equals("Pending", StringComparison.OrdinalIgnoreCase) ||
                    a.Status.Equals("New", StringComparison.OrdinalIgnoreCase));
            }
            else if (_filterApproved)
            {
                query = query.Where(a =>
                    a.Status.Equals("Approved", StringComparison.OrdinalIgnoreCase));
            }
            else if (_filterRejected)
            {
                query = query.Where(a =>
                    a.Status.Equals("Rejected", StringComparison.OrdinalIgnoreCase));
            }

            if (!string.IsNullOrWhiteSpace(SearchText))
            {
                query = query.Where(a =>
                    a.ApplicationNumber.Contains(
                        SearchText, StringComparison.OrdinalIgnoreCase) ||
                    a.LearnerFullName.Contains(
                        SearchText, StringComparison.OrdinalIgnoreCase));
            }

            return query.ToList();
        }
    }

    public ApplicationsViewModel(IAdminApiService api)
    {
        _api = api;
        Title = L.T("Applications");

        RefreshCommand = new Command(async () => await LoadAsync());
        FilterAllCommand = new Command(() => SetFilter(""));
        FilterNewCommand = new Command(() => SetFilter("New"));
        FilterPendingCommand = new Command(() => SetFilter("Pending"));
        FilterApprovedCommand = new Command(() => SetFilter("Approved"));
        FilterRejectedCommand = new Command(() => SetFilter("Rejected"));

        DetailCommand = new Command<AdmissionApplication>(
            async app => await NavigateToDetailAsync(app));
    }

    private void SetFilter(string status)
    {
        _filterNew = status == "New";
        _filterPending = status == "Pending";
        _filterApproved = status == "Approved";
        _filterRejected = status == "Rejected";

        OnPropertyChanged(nameof(IsFilterNew));
        OnPropertyChanged(nameof(IsFilterPending));
        OnPropertyChanged(nameof(IsFilterApproved));
        OnPropertyChanged(nameof(IsFilterRejected));

        RefreshFiltered();
    }

    private void RefreshFiltered() =>
        OnPropertyChanged(nameof(FilteredApplications));

    public async Task LoadAsync()
    {
        if (IsBusy)
            return;

        IsBusy = true;
        HasError = false;
        ErrorMessage = string.Empty;

        try
        {
            var list = await _api.GetApplicationsAsync();

            Applications.Clear();

            foreach (var application in list)
                Applications.Add(application);

            RefreshFiltered();
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

    public async Task DecideAsync(
        AdmissionApplication application, string status)
    {
        if (IsBusy || application is null ||
            !(application.Status.Equals(
                  "New", StringComparison.OrdinalIgnoreCase) ||
              application.Status.Equals(
                  "Pending", StringComparison.OrdinalIgnoreCase)))
        {
            return;
        }

        IsBusy = true;
        HasError = false;
        ErrorMessage = "";

        try
        {
            bool confirmed = await Shell.Current.DisplayAlertAsync(
                L.T("Confirm"),
                L.T("Change this application status?"),
                L.T("Continue"),
                L.T("Cancel"));

            if (!confirmed)
                return;

            await _api.UpdateApplicationStatusAsync(application.Id, status);

            application.Status = status;
            RefreshFiltered();
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

    public async Task NavigateToDetailAsync(AdmissionApplication app)
    {
        if (app is null)
            return;

        await Shell.Current.GoToAsync($"applicationdetail?id={app.Id}");
    }
}