using KoosMatliApp.Models;
using KoosMatliApp.Services;
using System.Collections.ObjectModel;

namespace KoosMatliApp.ViewModels;

public class SchoolDetailsViewModel : BaseViewModel
{
    private readonly IAdminApiService _api;

    public SchoolDetails? Details { get; private set; }
    public ObservableCollection<SchoolDate> Dates { get; } = new();
    public ObservableCollection<CalendarEvent> Events { get; } = new();

    public SchoolDetailsViewModel(IAdminApiService api)
    {
        _api = api;
        Title = "School";
    }

    public async Task InitializeAsync()
    {
        if (IsBusy) return;
        IsBusy = true;
        Details = await _api.GetSchoolDetailsAsync();
        OnPropertyChanged(nameof(Details));
        Dates.Clear();
        var dates = await _api.GetSchoolDatesAsync();
        foreach (var d in dates) Dates.Add(d);
        Events.Clear();
        var ev = await _api.GetCalendarEventsAsync();
        foreach (var e in ev) Events.Add(e);
        IsBusy = false;
    }
}
