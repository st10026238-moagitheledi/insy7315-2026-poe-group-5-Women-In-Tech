using KoosMatliApp.Features.Teacher.ViewModels;


namespace KoosMatliApp.Features.Teacher.Views;

public partial class HomeworkPage : ContentPage, IQueryAttributable
{
    private readonly HomeworkViewModel _viewModel;

    public HomeworkPage()
    {
        InitializeComponent();

        _viewModel =
            new HomeworkViewModel();

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


    protected override void OnAppearing()
    {
        base.OnAppearing();

        _viewModel.RefreshHomework();
    }


    private async void OnBackTapped(
        object sender,
        TappedEventArgs e)
    {
        await Shell.Current.GoToAsync("..");
    }


    private async void OnAddHomeworkClicked(
        object sender,
        EventArgs e)
    {
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
    }

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
                "There is no file attached to this homework.",
                "OK");

            return;
        }


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
        }
    }
}