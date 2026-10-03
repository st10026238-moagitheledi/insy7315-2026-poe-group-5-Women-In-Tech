using KoosMatliApp.Models;
using KoosMatliApp.Services;
using System.Collections.ObjectModel;

namespace KoosMatliApp.ViewModels;

public class DocumentsViewModel : BaseViewModel
{
    private readonly IAdminApiService _api;

    public ObservableCollection<SchoolDocument> Documents { get; } = new();

    public DocumentsViewModel(IAdminApiService api)
    {
        _api = api;
        Title = "Documents";
    }

    public async Task InitializeAsync()
    {
        if (IsBusy) return;
        IsBusy = true;
        var list = await _api.GetDocumentsAsync();
        Documents.Clear();
        foreach (var d in list) Documents.Add(d);
        IsBusy = false;
    }
}
