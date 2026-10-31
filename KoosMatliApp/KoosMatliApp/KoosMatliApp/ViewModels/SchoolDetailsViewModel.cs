using System.Collections.ObjectModel;
using System.Windows.Input;
using KoosMatliApp.Localization;
using KoosMatliApp.Models;
using KoosMatliApp.Services;

namespace KoosMatliApp.ViewModels;

public class SchoolDetailsViewModel : BaseViewModel
{
    private readonly IAdminApiService _api;
    private readonly AdminSession _session;

    private SchoolDetails? _original;
    private bool _loaded;
    private bool _isEditing = true;

    private string _schoolName = "";
    private string _address = "";
    private string _phoneNumber = "";
    private string _email = "";
    private string _principalName = "";

    public ObservableCollection<SchoolDate> Dates { get; } = new();
    public ObservableCollection<CalendarEvent> Events { get; } = new();

    public string SchoolName
    {
        get => _schoolName;
        set => SetProperty(ref _schoolName, value);
    }

    public string Address
    {
        get => _address;
        set => SetProperty(ref _address, value);
    }

    public string PhoneNumber
    {
        get => _phoneNumber;
        set => SetProperty(ref _phoneNumber, value);
    }

    public string Email
    {
        get => _email;
        set => SetProperty(ref _email, value);
    }

    public string PrincipalName
    {
        get => _principalName;
        set => SetProperty(ref _principalName, value);
    }

    public bool IsEditing
    {
        get => _isEditing;
        set => SetProperty(ref _isEditing, value);
    }

    public ICommand EditCommand { get; }
    public ICommand SaveCommand { get; }
    public ICommand CancelCommand { get; }
    public ICommand RefreshCommand { get; }
    public ICommand AddEventCommand { get; }
    public ICommand BackCommand { get; }

    public SchoolDetailsViewModel(
        IAdminApiService api,
        AdminSession session)
    {
        _api = api;
        _session = session;

        Title = L.T("School Details");

        EditCommand = new Command(() => IsEditing = true);

        SaveCommand = new Command(
            async () => await SaveAsync());

        CancelCommand = new Command(() =>
        {
            SetFields(_original);
            IsEditing = true;
            HasError = false;
            ErrorMessage = "";
        });

        RefreshCommand = new Command(async () =>
        {
            _loaded = false;
            await LoadAsync();
        });

        AddEventCommand = new Command(
            async () => await AddEventAsync());

        BackCommand = new Command(
            async () => await AppShell.GoBackAsync());
    }

    private bool CheckSession()
    {
        if (_session.IsConnected)
            return true;

        HasError = true;
        ErrorMessage = L.T(
            "Connect the signed-in user before loading admin data.");

        return false;
    }

    private void SetFields(SchoolDetails? value)
    {
        SchoolName = value?.SchoolName ?? "";
        Address = value?.Address ?? "";
        PhoneNumber = value?.PhoneNumber ?? "";
        Email = value?.Email ?? "";
        PrincipalName = value?.PrincipalName ?? "";
    }

    public async Task LoadAsync()
    {
        IsEditing = true;

        if (IsBusy || !CheckSession() || _loaded)
            return;

        IsBusy = true;
        HasError = false;
        ErrorMessage = "";

        try
        {
            _original = await _api.GetSchoolDetailsAsync();
            SetFields(_original);
            _loaded = true;
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

    public async Task SaveAsync()
    {
        if (IsBusy || !CheckSession())
            return;

        if (string.IsNullOrWhiteSpace(SchoolName))
        {
            HasError = true;
            ErrorMessage = L.T("A school name is required.");
            return;
        }

        IsBusy = true;
        HasError = false;
        ErrorMessage = "";

        try
        {
            var saved = await _api.UpdateSchoolDetailsAsync(
                new SchoolDetails
                {
                    SchoolName = SchoolName.Trim(),
                    Address = Address?.Trim() ?? "",
                    PhoneNumber = PhoneNumber?.Trim() ?? "",
                    Email = Email?.Trim() ?? "",
                    PrincipalName = PrincipalName?.Trim() ?? ""
                });

            if (saved is null)
            {
                HasError = true;
                ErrorMessage = L.T(
                    "The server returned no saved record.");
                return;
            }

            _original = saved;
            SetFields(saved);
            _loaded = true;

            await Shell.Current.DisplayAlertAsync(
                L.T("Success"),
                L.T("School details were saved successfully."),
                L.T("OK"));
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

    public Task AddEventAsync() =>
        Shell.Current.GoToAsync("addcalendarevent");

    public async Task LoadCalendarEventsAsync()
    {
        if (IsBusy || !CheckSession())
            return;

        IsBusy = true;
        HasError = false;
        ErrorMessage = "";

        try
        {
            var values = await _api.GetCalendarEventsAsync();

            Events.Clear();

            foreach (var value in values.OrderBy(x => x.Start))
                Events.Add(value);
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