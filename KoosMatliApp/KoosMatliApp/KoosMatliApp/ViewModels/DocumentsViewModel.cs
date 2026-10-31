using KoosMatliApp.Models;
using KoosMatliApp.Services;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace KoosMatliApp.ViewModels;

public class DocumentsViewModel : BaseViewModel
{
    private readonly IAdminApiService _api;

    public ObservableCollection<SchoolDocument> Documents { get; } = new();

    public ICommand ViewDocumentCommand { get; }
    public ICommand UploadCommand { get; }
    public ICommand BackCommand { get; }

    public DocumentsViewModel(IAdminApiService api)
    {
        _api = api;
        Title = L.T("Documents");

        ViewDocumentCommand = new Command<SchoolDocument>(
            async document => await ViewDocumentAsync(document));

        UploadCommand = new Command(async () => await ExplainUploadStatusAsync());

        BackCommand = new Command(async () =>
        {
            await Shell.Current.GoToAsync("//dashboard");
        });
    }

    public async Task InitializeAsync()
    {
        if (IsBusy)
            return;

        IsBusy = true;
        HasError = false;
        ErrorMessage = string.Empty;

        try
        {
            var list = await _api.GetDocumentsAsync();

            Documents.Clear();

            foreach (var document in list.OrderBy(d => d.Title))
                Documents.Add(document);
        }
        catch (Exception ex)
        {
            HasError = true;
            ErrorMessage = ex.Message;
        }
        finally
        {
            IsBusy = false;
        }
    }

    private static async Task ViewDocumentAsync(SchoolDocument? document)
    {
        if (document is null || string.IsNullOrWhiteSpace(document.FileUrl)) {
            await Shell.Current.DisplayAlertAsync(L.T("Error"), L.T("No file URL is available."), L.T("OK")); return;
        }
        try {
            var uri = new Uri(App.Services!.GetRequiredService<AppConfiguration>().ApiUri, document.FileUrl);
            if (uri.Scheme != "https") throw new InvalidOperationException(L.T("Only HTTPS document links can be opened."));
            await Browser.Default.OpenAsync(uri, BrowserLaunchMode.SystemPreferred);
        } catch (Exception ex) { await Shell.Current.DisplayAlertAsync(L.T("Error"), ex.Message, L.T("OK")); }
    }
    private static Task ExplainUploadStatusAsync() => Shell.Current.DisplayAlertAsync(
        L.T("Documents"), L.T("Document upload requires a file-storage service."), L.T("OK"));
}
