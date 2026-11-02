using KoosMatliApp.Models;
using KoosMatliApp.Services;
using System.Collections.ObjectModel;

namespace KoosMatliApp.ViewModels;

public class StaffViewModel : BaseViewModel //(Microsoft Learn, 2026a).
{
    private readonly IAdminApiService _api;
    private string _selectedTab = "Teachers";

    public ObservableCollection<StaffMember> Staff { get; } = new();
    public ObservableCollection<StaffMember> Teachers { get; } = new(); //(Microsoft Learn, 2026b).
    public ObservableCollection<SchoolClass> Classes { get; } = new();
    public ObservableCollection<Subject> Subjects { get; } = new();

    public string SelectedTab
    {
        get => _selectedTab;
        private set
        {
            if (SetProperty(ref _selectedTab, value))
            {
                OnPropertyChanged(nameof(ShowTeachers));
                OnPropertyChanged(nameof(ShowClasses));
                OnPropertyChanged(nameof(ShowSubjects));
            }
        }
    }

    public bool ShowTeachers => SelectedTab == "Teachers";
    public bool ShowClasses => SelectedTab == "Classes"; //(Microsoft Learn, 2026b).
    public bool ShowSubjects => SelectedTab == "Subjects";

    public StaffViewModel(IAdminApiService api)
    {
        _api = api;
        Title = L.T("Staff & Scheduling");
    }

    public async Task InitializeAsync()
    {
        if (IsBusy) return; //(Microsoft Learn, 2026c).

        IsBusy = true;
        HasError = false;
        ErrorMessage = string.Empty;

        try
        {
            var staffList = await _api.GetStaffAsync();

            Staff.Clear();
            Teachers.Clear();
            Classes.Clear();
            Subjects.Clear();

            foreach (var member in staffList)
            {
                Staff.Add(member);

                if (string.Equals(member.Position, "Teacher",
                    StringComparison.OrdinalIgnoreCase))
                {
                    Teachers.Add(member);
                }
            }

            foreach (var schoolClass in await _api.GetClassesAsync()) Classes.Add(schoolClass);
            foreach (var subject in await _api.GetSubjectsAsync()) Subjects.Add(subject);//(Microsoft Learn, 2026c).

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

    public void ShowTeachersTab() => SelectedTab = "Teachers";
    public void ShowClassesTab() => SelectedTab = "Classes";
    public void ShowSubjectsTab() => SelectedTab = "Subjects";
}
/*Reference List
 
 Microsoft Corporation, "Model-View-ViewModel (MVVM) in .NET MAUI," Microsoft Learn, 2026. [Online]. Available: https://learn.microsoft.com/en-us/dotnet/maui/xaml/mvvm

 Microsoft Corporation, "ObservableCollection Class (System.Collections.ObjectModel)," Microsoft Learn, 2026. [Online]. Available: https://learn.microsoft.com/en-us/dotnet/api/system.collections.objectmodel.observablecollection-1

 Microsoft Corporation, "Asynchronous Programming Patterns in .NET MAUI Apps," Microsoft Learn, 2026. [Online]. Available: https://learn.microsoft.com/en-us/dotnet/maui/data-cloud/async
 */