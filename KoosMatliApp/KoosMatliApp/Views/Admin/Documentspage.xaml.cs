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
            var file = await FilePicker.Default.PickAsync(
                new PickOptions
                {
                    PickerTitle =
                        "Choose PDF, DOCX, TXT, PNG or JPG (up to 10 MB)"
                });

            if (file is null)
                return;

            await _client.UploadAsync(file);
            await LoadAsync();
        });

        _refresh.Clicked += async (_, _) => await RunAsync(LoadAsync);

        Content = new ScrollView
        {
            Content = new VerticalStackLayout
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