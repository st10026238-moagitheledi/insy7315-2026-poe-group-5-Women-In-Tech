using KoosMatliApp.Services;

namespace KoosMatliApp.Views.Admin;

public partial class Documentspage : ContentPage
{
    private readonly VerticalStackLayout _list = new() { Spacing = 12 };
    private readonly Label _status = new() { TextColor = Colors.Red };

    private readonly Button _upload = new()
    {
        Text = AdminText.T("Upload Document")
    };

    private readonly Button _refresh = new()
    {
        Text = L.T("Refresh")
    };

    private readonly AdminExtrasClient _client;
    private readonly IAdminApiService _api;
    private bool _busy;

    public Documentspage()
    {
        Title = L.T("Documents");

        _client = new AdminExtrasClient(
            App.Services!.GetRequiredService<HttpClient>());

        _api = App.Services!.GetRequiredService<IAdminApiService>();

        _upload.Clicked += async (_, _) => await RunAsync(async () =>
        {
            var file = await FilePicker.Default.PickAsync( //(Microsoft Learn, 2026b).
                new PickOptions
                {
                    PickerTitle =
                        "Choose PDF, DOCX, TXT, PNG or JPG (up to 10 MB)" //(Microsoft Learn, 2026b).
                });

            if (file is null)
                return;

            await _client.UploadAsync(file); //(Microsoft Learn, 2026b).
            await LoadAsync();
        });

        _refresh.Clicked += async (_, _) => await RunAsync(LoadAsync);

        Content = new ScrollView //(Microsoft Learn, 2026a).
        {
            Content = new VerticalStackLayout //(Microsoft Learn, 2026a).
            {
                Padding = 20,
                Spacing = 16,
                Children =
                {
                    new Label
                    {
                        Text = L.T("School Documents"),
                        FontSize = 28,
                        FontAttributes = FontAttributes.Bold
                    },
                    _upload,
                    _refresh,
                    _status,
                    _list
                }
            }
        };
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await RunAsync(LoadAsync);
    }

    private async Task RunAsync(Func<Task> action)
    {
        if (_busy)
            return;

        if (!App.Services!
                .GetRequiredService<AdminSession>().IsConnected)
        {
            _status.Text = L.T(
                "Connect the signed-in user before loading admin data.");
            return;
        }

        _busy = true;
        _upload.IsEnabled = _refresh.IsEnabled = false;
        _status.Text = "";

        try
        {
            await action();
        }
        catch (Exception ex)
        {
            _status.Text = ex.Message;
        }
        finally
        {
            _busy = false;
            _upload.IsEnabled = _refresh.IsEnabled = true;
        }
    }

    private async Task LoadAsync()
    {
        var documents = await _api.GetDocumentsAsync();

        _list.Clear();

        if (documents.Count == 0)
            _list.Add(new Label { Text = L.T("No documents found.") });

        foreach (var item in documents.OrderBy(x => x.Title))
        {
            var open = new Button { Text = L.T("View") };

            open.Clicked += async (_, _) =>
                await RunAsync(() => _client.OpenAsync(item));

            _list.Add(new Border
            {
                Padding = 14,
                Content = new VerticalStackLayout
                {
                    Spacing = 8,
                    Children =
                    {
                        new Label
                        {
                            Text = item.Title,
                            FontAttributes = FontAttributes.Bold
                        },
                        new Label { Text = item.FileName },
                        open
                    }
                }
            });
        }
    }

    // Retained for the existing compiled XAML file.
    private void ViewDocument_Clicked(object sender, EventArgs e) { }
}

/*Reference List
 
 Microsoft Corporation, "Creating .NET MAUI Pages in C#," Microsoft Learn, 2026. [Online]. Available: https://learn.microsoft.com/en-us/dotnet/maui/user-interface/controls/

 Microsoft Corporation, "File Picker in .NET MAUI Essentials," Microsoft Learn, 2026. [Online]. Available: https://learn.microsoft.com/en-us/dotnet/maui/platform-integration/storage/file-picker

 Microsoft Corporation, "Asynchronous Programming Patterns in .NET MAUI Apps," Microsoft Learn, 2026. [Online]. Available: https://learn.microsoft.com/en-us/dotnet/maui/data-cloud/async
 */