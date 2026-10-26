using KoosMatliApp.Models;
using KoosMatliApp.Services;
using System.Collections.ObjectModel;

namespace KoosMatliApp.ViewModels;

public class UsersViewModel : BaseViewModel
{
    private readonly IAdminApiService _api;

    public ObservableCollection<User> Users { get; } = new();

    public UsersViewModel(IAdminApiService api)
    {
        _api = api;
        Title = "Users";
    }

    public async Task InitializeAsync(string? role = null)
    {
        if (IsBusy) return;
        IsBusy = true;
        var list = await _api.GetUsersAsync(role);
        Users.Clear();
        foreach (var u in list) Users.Add(u);
        IsBusy = false;
    }
}
