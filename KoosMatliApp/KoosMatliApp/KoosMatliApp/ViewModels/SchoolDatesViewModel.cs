using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using KoosMatliApp.Models;
using KoosMatliApp.Services;

namespace KoosMatliApp.ViewModels;

public class SchoolDatesViewModel : BaseViewModel
{
    private readonly IAdminApiService _api;

    private string _newTitle = string.Empty;
    private string _newDescription = string.Empty;
    private DateTime _newDate = DateTime.Today;
    private bool _isAddFormVisible;

    public ObservableCollection<SchoolDate> SchoolDates { get; } = new(); //(Microsoft Learn, 2026b).

    public string NewTitle
    {
        get => _newTitle;
        set => SetProperty(ref _newTitle, value);
    }

    public string NewDescription
    {
        get => _newDescription;
        set => SetProperty(ref _newDescription, value);
    }

    public DateTime NewDate
    {
        get => _newDate;
        set => SetProperty(ref _newDate, value);
    }

    public bool IsAddFormVisible
    {
        get => _isAddFormVisible;
        set => SetProperty(ref _isAddFormVisible, value);
    }

    public ICommand ShowAddFormCommand { get; }
    public ICommand CancelAddCommand { get; }
    public ICommand SaveDateCommand { get; }
    public ICommand RefreshCommand { get; }

    public SchoolDatesViewModel(IAdminApiService api)
    {
        _api = api;
        Title = L.T("School Dates");

        ShowAddFormCommand = new Command(() =>
        {
            NewTitle = string.Empty;
            NewDescription = string.Empty;
            NewDate = DateTime.Today;

            HasError = false;
            ErrorMessage = string.Empty;
            IsAddFormVisible = true;
        });

        CancelAddCommand = new Command(() =>
        {
            IsAddFormVisible = false;
            HasError = false;
            ErrorMessage = string.Empty;
        });

        SaveDateCommand = new Command(
          async () => await SaveDateAsync(),
          () => !IsBusy);

        RefreshCommand = new Command(
          async () => await LoadDatesAsync(),
          () => !IsBusy);
    }

    public Task InitializeAsync() => LoadDatesAsync();

    private async Task LoadDatesAsync()
    {
        if (IsBusy)
            return;

        try
        {
            SetBusy(true);
            HasError = false;
            ErrorMessage = string.Empty;

            var dates = await _api.GetSchoolDatesAsync();

            SchoolDates.Clear();

            foreach (var item in dates.OrderBy(x => x.Date))
                SchoolDates.Add(item);
        }
        catch (Exception ex)
        {
            HasError = true;
            ErrorMessage = ex.Message;

            System.Diagnostics.Debug.WriteLine(
              $"SCHOOL DATES LOAD ERROR: {ex}");
        }
        finally
        {
            SetBusy(false);
        }
    }

    private async Task SaveDateAsync()
    {
        if (IsBusy)
            return;

        if (string.IsNullOrWhiteSpace(NewTitle))
        {
            HasError = true;
            ErrorMessage = L.T("Please enter a title for the school date.");
            return;
        }

        try
        {
            SetBusy(true);
            HasError = false;
            ErrorMessage = string.Empty;

            var schoolDate = new SchoolDate
            {
                Title = NewTitle.Trim(),
                Date = DateTime.SpecifyKind(NewDate.Date, DateTimeKind.Utc), //(Microsoft Learn, 2026b).
                Description = NewDescription.Trim()
            };

            var saved = await _api.AddSchoolDateAsync(schoolDate);

            if (saved == null)
                throw new InvalidOperationException(
                  L.T("The server returned no saved record."));

            var dates = await _api.GetSchoolDatesAsync();

            SchoolDates.Clear();

            foreach (var item in dates.OrderBy(x => x.Date))
                SchoolDates.Add(item);

            IsAddFormVisible = false; //(Microsoft Learn, 2026b).
            NewTitle = string.Empty;
            NewDescription = string.Empty;
            NewDate = DateTime.Today;

            await Shell.Current.DisplayAlertAsync(
              L.T("Success"),
              L.T("The school date was saved successfully."),
              L.T("OK"));
        }
        catch (Exception ex)
        {
            HasError = true;
            ErrorMessage = ex.Message;

            System.Diagnostics.Debug.WriteLine(
                $"SCHOOL DATE SAVE ERROR: {ex}");
        }
        finally
        {
            SetBusy(false);
        }
    }

    private void SetBusy(bool value)
    {
        IsBusy = value;

        if (SaveDateCommand is Command save)
            save.ChangeCanExecute(); //(Microsoft Learn, 2026a).

        if (RefreshCommand is Command refresh)
            refresh.ChangeCanExecute();
    }
}

/*Reference List
 
 Microsoft Corporation, "Commanding in .NET MAUI: The ICommand Interface," Microsoft Learn, 2026. [Online]. Available: https://learn.microsoft.com/en-us/dotnet/maui/xaml/fundamentals/commanding

 Microsoft Corporation, "Handling DateTime in .NET Serialization," Microsoft Learn, 2026. [Online]. Available: https://learn.microsoft.com/en-us/dotnet/standard/datetime/choosing-between-datetime

 Microsoft Corporation, "Model-View-ViewModel (MVVM) in .NET MAUI," Microsoft Learn, 2026. [Online]. Available: https://learn.microsoft.com/en-us/dotnet/maui/xaml/mvvm
 */