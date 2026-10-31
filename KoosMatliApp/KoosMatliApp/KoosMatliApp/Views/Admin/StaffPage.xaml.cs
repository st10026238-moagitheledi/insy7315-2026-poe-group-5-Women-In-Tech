using KoosMatliApp.ViewModels;

namespace KoosMatliApp.Views.Admin;

public partial class StaffPage : ContentPage
{
    public StaffPage()
    {
        InitializeComponent();

        BindingContext = App.Services?.GetService(typeof(StaffViewModel))
            as StaffViewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        if (BindingContext is StaffViewModel vm)
            await vm.InitializeAsync();
    }

    private void Teachers_Clicked(object sender, EventArgs e) =>
        (BindingContext as StaffViewModel)?.ShowTeachersTab();

    private void Classes_Clicked(object sender, EventArgs e) =>
        (BindingContext as StaffViewModel)?.ShowClassesTab();

    private void Subjects_Clicked(object sender, EventArgs e) =>
        (BindingContext as StaffViewModel)?.ShowSubjectsTab();
}
