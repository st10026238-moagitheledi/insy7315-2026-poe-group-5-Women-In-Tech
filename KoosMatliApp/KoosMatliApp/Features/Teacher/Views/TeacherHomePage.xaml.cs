using KoosMatliApp.Features.Teacher.ViewModels;

namespace KoosMatliApp.Features.Teacher.Views;

public partial class TeacherHomePage : ContentPage
{
    public TeacherHomePage()
    {
        InitializeComponent();

        BindingContext = new TeacherHomeViewModel();
    }
}