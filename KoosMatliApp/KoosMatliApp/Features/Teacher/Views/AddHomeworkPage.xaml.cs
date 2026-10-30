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
                            "Choose homework file"
                    });


            if (result == null)
            {
                return;
            }


            var attachmentsFolder =
                Path.Combine(
                    FileSystem.AppDataDirectory,
                    "HomeworkAttachments");


            Directory.CreateDirectory(
                attachmentsFolder);


            var savedFileName =
                $"{Guid.NewGuid()}_{result.FileName}";


            var savedFilePath =
                Path.Combine(
                    attachmentsFolder,
                    savedFileName);


            await using var sourceStream =
                await result.OpenReadAsync();


            await using var destinationStream =
                File.Create(
                    savedFilePath);


            await sourceStream.CopyToAsync(
                destinationStream);


            _viewModel.AttachmentName =
                result.FileName;


            _viewModel.AttachmentPath =
                savedFilePath;
        }
        catch (Exception ex)
        {
            await DisplayAlert(
                "File Error",
                ex.Message,
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