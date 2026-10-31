namespace KoosMatliApp.Models;

public class AdmissionApplication : System.ComponentModel.INotifyPropertyChanged
{
    public int Id { get; set; }

    public string ApplicationNumber { get; set; } = string.Empty;

    public string LearnerFirstName { get; set; } = string.Empty;
    public string LearnerLastName { get; set; } = string.Empty;

    public DateTime DateOfBirth { get; set; }

    public string GradeApplyingFor { get; set; } = string.Empty;

    public string GuardianName { get; set; } = string.Empty; ////(Microsoft Learn, 2026).
    public string GuardianEmail { get; set; } = string.Empty;
    public string GuardianPhone { get; set; } = string.Empty;

    public DateTime DateSubmitted { get; set; }

    private string _status = "New";
    public event System.ComponentModel.PropertyChangedEventHandler? PropertyChanged;
    public string Status { get => _status; set { if (_status == value) return; _status = value; PropertyChanged?.Invoke(this, new(nameof(Status))); } }

    public string LearnerFullName =>
        $"{LearnerFirstName} {LearnerLastName}";
}

/*
 Reference List
 
 Microsoft Corporation, "Model-View-ViewModel (MVVM) in .NET MAUI," Microsoft Learn, 2026. [Online]. Available: https://learn.microsoft.com/en-us/dotnet/maui/xaml/mvvm
 */