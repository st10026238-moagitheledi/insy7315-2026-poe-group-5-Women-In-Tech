namespace KoosMatliApp.AdminThemeResources;
public partial class AdminControlStyles : ResourceDictionary
{
    public AdminControlStyles()
    {
        MergedDictionaries.Add(new AdminColors());
        InitializeComponent();
    }
}
