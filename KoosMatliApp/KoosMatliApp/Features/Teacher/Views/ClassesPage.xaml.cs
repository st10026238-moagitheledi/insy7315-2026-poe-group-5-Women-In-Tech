using KoosMatliApp.Features.Teacher.Models;
using KoosMatliApp.Features.Teacher.ViewModels;

namespace KoosMatliApp.Features.Teacher.Views;

public partial class ClassesPage : ContentPage
{
    private readonly ClassesViewModel _viewModel;

    public ClassesPage()
    {
        InitializeComponent();

        _viewModel =
            new ClassesViewModel();

        BindingContext = _viewModel;
    }


    protected override void OnAppearing()
    {
        base.OnAppearing();

        _viewModel.RefreshLearnerCounts();
    }


    private async void OnClassTapped(
        object sender,
        TappedEventArgs e)
    {
        if (sender is Border border &&
            border.BindingContext is TeacherClassItem selectedClass)
        {
            var navigationParameters =
                new Dictionary<string, object>
                {
                    {
                        "SelectedClass",
                        selectedClass
                    }
                };

            await Shell.Current.GoToAsync(
                nameof(ClassDetailsPage),
                navigationParameters);
        }
    }
}