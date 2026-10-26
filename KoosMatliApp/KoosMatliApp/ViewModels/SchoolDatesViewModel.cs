using KoosMatliApp.Models;
using KoosMatliApp.Services;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

namespace KoosMatliApp.ViewModels;

public class SchoolDatesViewModel : BaseViewModel
{
    private readonly IAdminApiService _api;

    private string _newTitle = string.Empty;
    private string _newDescription = string.Empty;
    private DateTime _newDate = DateTime.Today;
    private bool _isAddFormVisible;

    public ObservableCollection<SchoolDate> SchoolDates { get; } = new();

    public string NewTitle
    {
        get => _newTitle;
        set
        {
            if (_newTitle == value) return;
            _newTitle = value;
            OnPropertyChanged();
        }
    }

    public string NewDescription
    {
        get => _newDescription;
        set
        {
            if (_newDescription == value) return;
            _newDescription = value;
            OnPropertyChanged();
        }
    }

    public DateTime NewDate
    {
        get => _newDate;
        set
        {
            if (_newDate == value) return;
            _newDate = value;
            OnPropertyChanged();
        }
    }

    public bool IsAddFormVisible
    {
        get => _isAddFormVisible;
        set
        {
            if (_isAddFormVisible == value) return;
            _isAddFormVisible = value;
            OnPropertyChanged();
        }
    }

    public ICommand ShowAddFormCommand { get; }
    public ICommand CancelAddCommand { get; }
    public ICommand SaveDateCommand { get; }
    public ICommand RefreshCommand { get; }

    public SchoolDatesViewModel(IAdminApiService api)
    {
        _api = api;

        Title = "School Dates";

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

        SaveDateCommand = new Command(async () => await SaveDateAsync());

        RefreshCommand = new Command(async () => await LoadDatesAsync());
    }

    public async Task InitializeAsync()
    {
        await LoadDatesAsync();
    }

    private async Task LoadDatesAsync()
    {
        if (IsBusy)
            return;

        try
        {
            IsBusy = true;
            HasError = false;
            ErrorMessage = string.Empty;

            var dates = await _api.GetSchoolDatesAsync();

            SchoolDates.Clear();

            foreach (var schoolDate in dates.OrderBy(d => d.Date))
                SchoolDates.Add(schoolDate);
        }
        catch (Exception ex)
        {
            HasError = true;
            ErrorMessage = "Unable to load school dates. Please try again.";

            System.Diagnostics.Debug.WriteLine(
                $"SCHOOL DATES LOAD ERROR: {ex}");
        }
        finally
        {
            IsBusy = false;
        }
    }

    private async Task SaveDateAsync()
    {
        if (IsBusy)
            return;

        if (string.IsNullOrWhiteSpace(NewTitle))
        {
            HasError = true;
            ErrorMessage = "Please enter a title for the school date.";
            return;
        }

        try
        {
            IsBusy = true;
            HasError = false;
            ErrorMessage = string.Empty;

            var schoolDate = new SchoolDate
            {
                Title = NewTitle.Trim(),
                Date = NewDate.Date,
                Description = NewDescription.Trim()
            };

            var savedDate = await _api.AddSchoolDateAsync(schoolDate);

            if (savedDate == null)
            {
                HasError = true;
                ErrorMessage = "The API did not return the saved school date.";
                return;
            }

            SchoolDates.Add(savedDate);

            var sortedDates = SchoolDates.OrderBy(d => d.Date).ToList();
            SchoolDates.Clear();

            foreach (var item in sortedDates)
                SchoolDates.Add(item);

            IsAddFormVisible = false;
            NewTitle = string.Empty;
            NewDescription = string.Empty;
            NewDate = DateTime.Today;

            await Shell.Current.DisplayAlertAsync(
            "Success",
            "The school date was saved successfully.",
            "OK");
        }
        catch (Exception ex)
        {
            HasError = true;
            ErrorMessage = "Unable to save the school date. Please try again.";

            System.Diagnostics.Debug.WriteLine(
                $"SCHOOL DATE SAVE ERROR: {ex}");
        }
        finally
        {
            IsBusy = false;
        }
    }
}