using KoosMatliApp.Models;
using KoosMatliApp.Services;
using System.Collections.ObjectModel;

namespace KoosMatliApp.ViewModels;

public class StaffViewModel : BaseViewModel
{
    private readonly IAdminApiService _api;

    public ObservableCollection<StaffMember> Staff { get; } = new();

    public StaffViewModel(IAdminApiService api)
    {
        _api = api;
        Title = "Staff";
    }

    public async Task InitializeAsync()
    {
        if (IsBusy) return;
        IsBusy = true;
        var list = await _api.GetStaffAsync();
        Staff.Clear();
        foreach (var s in list) Staff.Add(s);
        IsBusy = false;
    }
}
