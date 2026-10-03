using KoosMatliApp.Features.Teacher.ViewModels;

namespace KoosMatliApp.Features.Teacher.Views;

public partial class ClassesPage : ContentPage
{
    public ClassesPage()
    {
        InitializeComponent();

        BindingContext = new ClassesViewModel();
    }
}