using Microsoft.Extensions.DependencyInjection;
using Microsoft.Maui.Storage;
using KoosMatliApp.Features.Teacher.ViewModels;

namespace KoosMatliApp.Features.Teacher.Views;

public partial class AddHomeworkPage
    : ContentPage, IQueryAttributable
{
    private AddHomeworkViewModel?
        _viewModel;

    private string _grade =
        string.Empty;

    private string _subject =
        string.Empty; // (Microsoft, 2026)


    public AddHomeworkPage()
    {
        InitializeComponent();


        HandlerChanged +=
            OnHandlerChanged;
    }


    private void OnHandlerChanged(
        object? sender,
        EventArgs e)
    {
        ResolveViewModel();
    }


    private void ResolveViewModel()
    {
        if (_viewModel != null)
        {
            return;
        } // (Microsoft, 2026)


        var services =
            Handler?
                .MauiContext?
                .Services;


        if (services == null)
        {
            return;
        }


        _viewModel =
            services.GetRequiredService
                <AddHomeworkViewModel>();


        BindingContext =
            _viewModel;


        _viewModel.LoadClassInformation(
            _grade,
            _subject);
    }


    public void ApplyQueryAttributes(
        IDictionary<string, object> query)
    {
        _grade =
            query.TryGetValue(
                "Grade",
                out var gradeValue)
                ? gradeValue?.ToString() ??
                    string.Empty
                : string.Empty; // (Microsoft, 2026)


        _subject =
            query.TryGetValue(
                "Subject",
                out var subjectValue)
                ? subjectValue?.ToString() ??
                    string.Empty
                : string.Empty;


        _viewModel?
            .LoadClassInformation(
                _grade,
                _subject);
    }


    private async void OnBackTapped(
        object sender,
        TappedEventArgs e)
    {
        await Shell.Current.GoToAsync(
            "..");
    }


    private async void OnChooseFileClicked(
        object sender,
        EventArgs e)
    {
        ResolveViewModel();


        if (_viewModel == null)
        {
            return;
        } // (Microsoft, 2026)


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
                "OK"); // (Microsoft, 2026)
        }
    }


    private async void OnSaveHomeworkClicked(
        object sender,
        EventArgs e)
    {
        ResolveViewModel();


        if (_viewModel == null)
        {
            return;
        }


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
        } // (Microsoft, 2026)


        var success =
            await _viewModel
                .SaveHomeworkAsync();


        if (!success)
        {
            await DisplayAlert(
                "Homework",
                _viewModel.ErrorMessage,
                "OK");

            return;
        }


        await DisplayAlert(
            "Homework",
            "Homework has been added successfully.",
            "OK");


        await Shell.Current.GoToAsync(
            ".."); // (Microsoft, 2026)
    }
}

//Reference List:
//1. Microsoft, 2026. Introduction to .NET MAUI. [online] Available at: https://learn.microsoft.com/en-us/dotnet/architecture/maui/introduction [Accessed 31 October 2026].

