using Microsoft.Extensions.DependencyInjection;
using KoosMatliApp.Features.Teacher.ViewModels;

namespace KoosMatliApp.Features.Teacher.Views;

public partial class HomeworkPage
    : ContentPage, IQueryAttributable
{
    private HomeworkViewModel?
        _viewModel;

    private string _grade =
        string.Empty;

    private string _subject =
        string.Empty; // (Microsoft, 2026)


    public HomeworkPage()
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
    } // (Microsoft, 2026)


    private void ResolveViewModel()
    {
        if (_viewModel != null)
        {
            return;
        }


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
                <HomeworkViewModel>();


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
                : string.Empty;


        _subject =
            query.TryGetValue(
                "Subject",
                out var subjectValue)
                ? subjectValue?.ToString() ??
                    string.Empty
                : string.Empty; // (Microsoft, 2026)


        _viewModel?
            .LoadClassInformation(
                _grade,
                _subject);
    }


    protected override async void OnAppearing()
    {
        base.OnAppearing();


        ResolveViewModel();


        if (_viewModel == null)
        {
            return;
        }


        var success =
            await _viewModel
                .RefreshHomeworkAsync();


        if (!success &&
            !string.IsNullOrWhiteSpace(
                _viewModel.ErrorMessage))
        {
            await DisplayAlert(
                "Homework",
                _viewModel.ErrorMessage,
                "OK");
        } // (Microsoft, 2026)
    }


    private async void OnBackTapped(
        object sender,
        TappedEventArgs e)
    {
        await Shell.Current.GoToAsync(
            "..");
    }


    private async void OnAddHomeworkClicked(
        object sender,
        EventArgs e)
    {
        if (_viewModel == null)
        {
            return;
        }


        var parameters =
            new Dictionary<string, object>
            {
                {
                    "Grade",
                    _viewModel.Grade
                },

                {
                    "Subject",
                    _viewModel.Subject
                }
            };


        await Shell.Current.GoToAsync(
            nameof(AddHomeworkPage),
            parameters);
    } // (Microsoft, 2026)


    private async void OnOpenAttachmentClicked(
        object sender,
        EventArgs e)
    {
        if (sender is not Button button)
        {
            return;
        }


        var filePath =
            button.CommandParameter
                ?.ToString();


        if (string.IsNullOrWhiteSpace(
                filePath))
        {
            await DisplayAlert(
                "No Attachment",
                "This homework does not have a file stored on this device.",
                "OK");

            return;
        } // (Microsoft, 2026)


        if (!File.Exists(
                filePath))
        {
            await DisplayAlert(
                "File Not Found",
                "The attached file could not be found on this device.",
                "OK");

            return;
        }


        try
        {
            await Launcher.Default.OpenAsync(
                new OpenFileRequest
                {
                    File =
                        new ReadOnlyFile(
                            filePath)
                });
        }
        catch (Exception ex)
        {
            await DisplayAlert(
                "Unable to Open File",
                ex.Message,
                "OK");
        } // (Microsoft, 2026)
    }
}

//Reference List:
//1. Microsoft, 2026. Introduction to .NET MAUI. [online] Available at: https://learn.microsoft.com/en-us/dotnet/architecture/maui/introduction [Accessed 31 October 2026].

