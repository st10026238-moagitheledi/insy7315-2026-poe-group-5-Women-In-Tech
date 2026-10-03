using KoosMatliApp.Features.Teacher.Models;
using KoosMatliApp.Features.Teacher.ViewModels;

namespace KoosMatliApp.Features.Teacher.Views;

public partial class ClassDetailsPage : ContentPage, IQueryAttributable
{
    private readonly ClassDetailsViewModel _viewModel;

    public ClassDetailsPage()
    {
        InitializeComponent();

        _viewModel = new ClassDetailsViewModel();

        BindingContext = _viewModel;
    }

    public void ApplyQueryAttributes(
        IDictionary<string, object> query)
    {
        if (query.TryGetValue("SelectedClass", out var selectedClass) &&
            selectedClass is TeacherClassItem classItem)
        {
            _viewModel.LoadClass(classItem);
        }
    }

    private async void OnBackTapped(
        object sender,
        TappedEventArgs e)
    {
        await Shell.Current.GoToAsync("..");
    }
}