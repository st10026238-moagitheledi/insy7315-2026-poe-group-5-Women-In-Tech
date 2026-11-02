using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace KoosMatliApp.ViewModels
{
    public abstract class BaseViewModel : INotifyPropertyChanged //(Microsoft Learn, 2026a).
    {
        private string _title = string.Empty;
        private bool _isBusy;
        private bool _hasError;
        private string _errorMessage = string.Empty;

        public event PropertyChangedEventHandler? PropertyChanged;

        public string Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
        }

        public bool IsBusy
        {
            get => _isBusy;
            set
            {
                if (SetProperty(ref _isBusy, value))
                    OnPropertyChanged(nameof(IsNotBusy));
            }
        }

        public bool IsNotBusy => !_isBusy;

        public bool HasError
        {
            get => _hasError;
            set => SetProperty(ref _hasError, value);
        }

        public string ErrorMessage //(Microsoft Learn, 2026c).
        {
            get => _errorMessage;
            set => SetProperty(ref _errorMessage, value);
        }

        protected void OnPropertyChanged(
            [CallerMemberName] string? propertyName = null) //(Microsoft Learn, 2026b).
        {
            PropertyChanged?.Invoke(
                this,
                new PropertyChangedEventArgs(propertyName));
        }

        protected bool SetProperty<T>(
            ref T backingField,
            T value,
            [CallerMemberName] string? propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(backingField, value))
                return false;

            backingField = value;
            OnPropertyChanged(propertyName);
            return true;
        }
    }
}

/* Reference List
 * 
 * Microsoft Corporation, "Model-View-ViewModel (MVVM) in .NET MAUI: Property Chang Notifications," Microsoft Learn, 2026. [Online]. Available: https://learn.microsoft.com/en-us/dotnet/maui/xaml/mvvm

   Microsoft Corporation, "CallerMemberNameAttribute Class (System.Runtime.CompilerServices)," Microsoft Learn, 2026. [Online]. Available: https://learn.microsoft.com/en-us/dotnet/api/system.runtime.compilerservices.callermembernameattribute

   Microsoft Corporation, "INotifyPropertyChanged Interface (System.ComponentModel)," Microsoft Learn, 2026. [Online]. Available: https://learn.microsoft.com/en-us/dotnet/api/system.componentmodel.inotifypropertychanged*/
