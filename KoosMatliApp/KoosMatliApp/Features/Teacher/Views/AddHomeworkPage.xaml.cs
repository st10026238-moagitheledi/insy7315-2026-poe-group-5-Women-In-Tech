using Microsoft.Maui.Storage;
using KoosMatliApp.Features.Teacher.ViewModels;

namespace KoosMatliApp.Features.Teacher.Views;

public partial class AddHomeworkPage : ContentPage, IQueryAttributable
{
    private readonly AddHomeworkViewModel _viewModel;

    public AddHomeworkPage()
    {
        InitializeComponent();

        _viewModel =
            new AddHomeworkViewModel();

        BindingContext = _viewModel;
    }


    public void ApplyQueryAttributes(
        IDictionary<string, object> query)
    {
        var grade =
            query.TryGetValue(
                "Grade",
                out var gradeValue)
            ? gradeValue?.ToString() ?? string.Empty
            : string.Empty;


        var subject =
            query.TryGetValue(
                "Subject",
                out var subjectValue)
            ? subjectValue?.ToString() ?? string.Empty
            : string.Empty;


        _viewModel.LoadClassInformation(
            grade,
            subject);
    }


    private async void OnBackTapped(
        object sender,
        TappedEventArgs e)
    {
        await Shell.Current.GoToAsync("..");
    }


    private async void OnChooseFileClicked(
        object sender,
        EventArgs e)
    {
        try
        {
            var result =
                await FilePicker.Default.PickAsync(
                    new PickOptions
                    {
                        PickerTitle =
                            "Select homework file"
                    });


            if (result != null)
            {
                _viewModel.AttachmentName =
                    result.FileName;
            }
        }
        catch (Exception)
        {
            await DisplayAlert(
                "File",
                "The file could not be selected.",
                "OK");
        }
    }


    private async void OnSaveHomeworkClicked(
        object sender,
        EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(
                _viewModel.Title))
        {
            await DisplayAlert(
                "Homework",
                "Please enter a homework title.",
                "OK");

            return;
        }


        if (string.IsNullOrWhiteSpace(
                _viewModel.Description))
        {
            await DisplayAlert(
                "Homework",
                "Please enter homework instructions.",
                "OK");

            return;
        }


        _viewModel.SaveHomework();


        await DisplayAlert(
            "Homework",
            "Homework has been added successfully.",
            "OK");


        await Shell.Current.GoToAsync("..");
    }
}