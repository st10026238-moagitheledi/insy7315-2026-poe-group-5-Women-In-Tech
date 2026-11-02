using System.Collections.ObjectModel;
using KoosMatliApp.Models;
using KoosMatliApp.Services;

namespace KoosMatliApp.ViewModels; //(Microsoft Learn, 2026a).

public class UsersViewModel : BaseViewModel
{
    private readonly IAdminApiService _api;
    private readonly List<User> _allUsers = new();
    private string _searchText = string.Empty;
    private string? _selectedRole;

    public ObservableCollection<User> Users { get; } = new(); //(Microsoft Learn, 2026b).

    public string SearchText
    {
        get => _searchText;
        set
        {
            if (SetProperty(ref _searchText, value))
                ApplyFilters(); // (Microsoft Learn, 2026b).
        }
    }

    public IReadOnlyList<User> FilteredUsers => Users.ToList();

    public UsersViewModel(IAdminApiService api)
    {
        _api = api;
        Title = L.T("Users");
    }

    public async Task InitializeAsync()
    {
        if (IsBusy) return; //(Microsoft Learn, 2026c).

        IsBusy = true;
        HasError = false;
        ErrorMessage = string.Empty;

        try
        {
            var list = await _api.GetUsersAsync();

            _allUsers.Clear(); //(Microsoft Learn, 2026b).
            _allUsers.AddRange(list);

            ApplyFilters();
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

    public void FilterByRole(string? role)
    {
        _selectedRole = _selectedRole == role ? null : role;
        ApplyFilters();
    }

    private void ApplyFilters()
    {
        var filtered = _allUsers.Where(user =>
            (string.IsNullOrWhiteSpace(_selectedRole) ||
             user.Role.Equals(_selectedRole, StringComparison.OrdinalIgnoreCase)) &&
            (string.IsNullOrWhiteSpace(SearchText) ||
             user.FullName.Contains(SearchText, StringComparison.OrdinalIgnoreCase) ||
             user.Email.Contains(SearchText, StringComparison.OrdinalIgnoreCase) ||
             user.Role.Contains(SearchText, StringComparison.OrdinalIgnoreCase) ||
             user.PhoneNumber.Contains(SearchText, StringComparison.OrdinalIgnoreCase)))
            .ToList();

        Users.Clear();

        foreach (var user in filtered)
            Users.Add(user);

        OnPropertyChanged(nameof(FilteredUsers));
    }
}

/*Reference List
 
 Microsoft Corporation, "Model-View-ViewModel (MVVM) in .NET MAUI," Microsoft Learn, 2026. [Online]. Available: https://learn.microsoft.com/en-us/dotnet/maui/xaml/mvvm

 Microsoft Corporation, "ObservableCollection Class (System.Collections.ObjectModel)," Microsoft Learn, 2026. [Online]. Available: https://learn.microsoft.com/en-us/dotnet/api/system.collections.objectmodel.observablecollection-1

 Microsoft Corporation, "Language Integrated Query (LINQ) Overview," Microsoft Learn, 2026. [Online]. Available: https://learn.microsoft.com/en-us/dotnet/csharp/programming-guide/concepts/linq/

 Microsoft Corporation, "Asynchronous Programming Patterns in .NET MAUI Apps," Microsoft Learn, 2026. [Online]. Available: https://learn.microsoft.com/en-us/dotnet/maui/data-cloud/async
 */