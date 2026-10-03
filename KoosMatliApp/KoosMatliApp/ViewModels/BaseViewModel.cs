using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace KoosMatliApp.ViewModels;

public class BaseViewModel : INotifyPropertyChanged
{
    bool isBusy;
    public bool IsBusy
    {
        get => isBusy;
        set { isBusy = value; OnPropertyChanged(); }
    }

    string title = string.Empty;
    public string Title
    {
        get => title;
        set { title = value; OnPropertyChanged(); }
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    protected void OnPropertyChanged([CallerMemberName] string? name = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
